using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Placeholder.SDK.WebGen
{
    public class EndpointTranslatorTypeScript
    {
        public EndpointTranslatorTypeScript(string projectName, string inputFolder, string outputFolder)
        {
            this.ProjectName = projectName;
            this.InputFolder = inputFolder;
            this.OutputFolder = outputFolder;
        }
        public string InputFolder { get; set; }
        public string OutputFolder { get; set; }
        public string ProjectName { get; set; }

        public void Process(bool internalSDK, bool publicSDK, bool includeInputClasses)
        {
            Console.WriteLine("WebGen: Starting with input folder: " + this.InputFolder);
            Console.WriteLine("        - input folder: " + this.InputFolder);
            Console.WriteLine("        - output folder: " + this.OutputFolder);

            List<string> fileNamesInternal = new List<string>();
            List<string> fileNamesPublic = new List<string>();

            List<string> files = Directory.EnumerateFiles(this.InputFolder, "*.cs", SearchOption.AllDirectories).ToList();
            files.Sort();
            //files.Reverse();// for partial support, _ files go first [but .net core and .net standard may invert the default]
            foreach (string inputFile in files)
            {
                string newFile = inputFile;
                int ix = inputFile.IndexOf(this.InputFolder);
                if (ix >= 0)
                {
                    newFile = inputFile.Substring(ix + this.InputFolder.Length);
                }
                newFile = Path.ChangeExtension(newFile, ".ts");
                if (newFile.ToLower().Contains("endpoint"))
                {
                    newFile = Path.Combine("endpoints", newFile);
                }

                // internal
                string outputFile = string.Empty;
                List<string> foundClasses = null;
                if (internalSDK)
                {
                    if (publicSDK)
                    {
                        outputFile = Path.Combine(this.OutputFolder, "Internal", newFile); // using both, split
                    }
                    else
                    {
                        outputFile = Path.Combine(this.OutputFolder, newFile);
                    }

                    Directory.CreateDirectory(Path.GetDirectoryName(outputFile));
                    foundClasses = this.ProcessFile(inputFile, outputFile, true, includeInputClasses);
                    if (foundClasses.Count > 0)
                    {
                        fileNamesInternal.Add(newFile);
                    }
                }

                // public
                if (publicSDK)
                {
                    if (internalSDK)
                    {
                        outputFile = Path.Combine(this.OutputFolder, "Public", newFile); // using both, split
                    }
                    else
                    {
                        outputFile = Path.Combine(this.OutputFolder, newFile);
                    }
                    Directory.CreateDirectory(Path.GetDirectoryName(outputFile));
                    foundClasses = this.ProcessFile(inputFile, outputFile, false, includeInputClasses);
                    if (foundClasses.Count > 0)
                    {
                        fileNamesPublic.Add(newFile);
                    }
                }
            }
            if (internalSDK)
            {
                if (publicSDK)
                {
                    this.WriteMappingFile(Path.Combine(this.OutputFolder, "Internal"), fileNamesInternal);// using both, split
                }
                else
                {
                    this.WriteMappingFile(this.OutputFolder, fileNamesInternal);
                }

            }
            if (publicSDK)
            {
                if (internalSDK)
                {
                    this.WriteMappingFile(Path.Combine(this.OutputFolder, "Public"), fileNamesPublic);
                }
                else
                {
                    this.WriteMappingFile(this.OutputFolder, fileNamesPublic);
                }
            }
            Console.WriteLine("WebGen: Complete");
        }

        private void WriteMappingFile(string outputFolder, List<string> fileNames)
        {
            HashSet<string> files = new HashSet<string>();
            foreach (string item in fileNames)
            {
                files.Add(item.Replace("_Core", ""));
            }
            fileNames = files.ToList();
            List<string> results = new List<string>();
            foreach (string item in fileNames)
            {
                results.Add(string.Format("import {{{0}}} from './{1}';", Path.GetFileNameWithoutExtension(item), Path.ChangeExtension(item, "").Trim('.').Replace(@"\", "/")));
            }
            results.Add(string.Format("import {{{0}Request}} from './{1}-request';", this.ProjectName, this.ProjectName.ToLower()));
            //results.Add(string.Format("import {{I{0}SDK}} from './{1}-sdk-interface';", this.ProjectName, this.ProjectName.ToLower()));
            results.Add("import { Md5 } from 'ts-md5/dist/md5';");

            results.Add(string.Format("export class {0}Endpoints {{", this.ProjectName));

            foreach (string item in fileNames)
            {
                results.Add(string.Format("    {0}: {1};", Path.GetFileNameWithoutExtension(item).Replace("Endpoint", ""), Path.GetFileNameWithoutExtension(item)));
            }

            results.Add("    ");
            results.Add("    constructEndpoints(api){");
            results.Add("        ");
            foreach (string item in fileNames)
            {
                results.Add(string.Format("        this.{0} = new {1}(api);", Path.GetFileNameWithoutExtension(item).Replace("Endpoint", ""), Path.GetFileNameWithoutExtension(item)));
            }
            results.Add("    }");

            results.Add(string.Format("    execute(request: {0}Request) {{", this.ProjectName));
            results.Add("        throw new Error(\"You must override execute with a framework specific implementation\");");
            results.Add("    }");

            results.Add("    injectAuthTokens(headers: any, api_key: string, api_secret: string):any {");
            results.Add("        if (api_key && api_secret) {");
            results.Add("            headers[\"x-api-key\"] = api_key;");
            results.Add("            headers[\"x-api-signature\"] = Md5.hashStr(api_key + api_secret + (Date.now() / 1000 | 0));");
            results.Add("        }");
            results.Add("        return headers;");
            results.Add("    }");


            results.Add("}");

            string output = Path.Combine(outputFolder, string.Format("{0}-endpoints.ts", this.ProjectName.ToLower()));

            if (File.Exists(output))
            {
                string[] foundContent = File.ReadAllLines(output);
                if (foundContent.SequenceEqual(results))
                {
                    return;
                }
            }

            File.WriteAllLines(output, results);
        }
        private List<string> ProcessFile(string inputFile, string outputFile, bool includeInternal, bool includeInputClasses)
        {
            HashSet<string> classNames = new HashSet<string>();
            List<string> results = new List<string>();
            string[] text = File.ReadAllLines(inputFile);

            bool isEndpoint = inputFile.ToLower().Contains("endpoint");
            bool inClass = false;
            bool inMethod = false;
            bool inPassThrough = false;
            int bracketCount = 0;
            int indent = 0;
            string previousLine = string.Empty;
            bool isPartialClass = false;
            string partialMasterFile = string.Empty;

            HashSet<string> imports = new HashSet<string>();


            string fileName = Path.GetFileName(outputFile);
            if (!fileName.Contains("_"))
            {
                string masterFile = Path.GetFileNameWithoutExtension(inputFile) + "_Core" + Path.GetExtension(inputFile);
                masterFile = Path.Combine(Path.GetDirectoryName(inputFile), masterFile);
                if (File.Exists(masterFile))
                {
                    partialMasterFile = outputFile;
                    isPartialClass = true;
                }
            }
            else
            {
                string coreFile = fileName.Substring(0, fileName.IndexOf("_")) + Path.GetExtension(outputFile);
                outputFile = Path.Combine(Path.GetDirectoryName(outputFile), coreFile);
            }

            for (int i = 0; i < text.Length; i++)
            {
                string rawLine = text[i];
                string line = rawLine.Trim();
                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }

                if (line.StartsWith("#if !WEB") && text[i + 1].Contains("namespace")) // ignored code
                {
                    return new List<string>();
                }
                if (!includeInternal && line.StartsWith("#if SDK_PRIVATE") && text[i + 1].Contains("namespace")) // ignored code
                {
                    return new List<string>();
                }
                if (line.StartsWith("public class") || line.StartsWith("public partial class"))
                {
                    string className = line
                                            .Replace(" ", "")
                                            .Replace("publicclass", "")
                                            .Replace("publicpartialclass", "")
                                            .Replace(":EndpointBase", "")
                                            .Replace("Endpoint", "")
                                            .Replace("_Core", ""); ;
                    classNames.Add(className);
                    if (isPartialClass)
                    {
                        indent++;
                    }
                    else
                    {
                        if (isEndpoint)
                        {
                            results.Add(Indent(indent) + string.Format("import {{{0}Request}} from '../{1}-request';", this.ProjectName, this.ProjectName.ToLower()));
                            results.Add(Indent(indent) + string.Format("import {{I{0}SDK}} from '../{1}-sdk-interface';", this.ProjectName, this.ProjectName.ToLower()));
                            results.Add(string.Empty);
                        }
                        results.Add(Indent(indent) + string.Format("export class {0}Endpoint {{", className));
                        indent++;
                        results.Add(Indent(indent) + string.Format("constructor(private api:I{0}SDK){{", this.ProjectName));
                        indent++;
                        results.Add(Indent(indent) + "this.api = api;");
                        indent--;
                        results.Add(Indent(indent) + "}");
                        results.Add(Indent(indent) + "// <Standard Methods>");
                    }
                    imports.Clear();
                    inClass = true;
                    continue;
                }

                if (inClass)
                {

                    if (line == "{")
                    {
                        if (inPassThrough)
                        {
                            results.Add(Indent(indent) + "{");
                        }
                        bracketCount++;
                        continue;
                    }
                    if (line == "}")
                    {
                        bracketCount--;
                    }

                    if (bracketCount == 0)
                    {

                        if (imports.Count > 0)
                        {
                            if (isPartialClass)
                            {
                                // add it during merge
                            }
                            else
                            {
                                // bad process, ah well
                                foreach (string item in imports)
                                {
                                    results.Insert(0, string.Format("import {{ {0} }} from '../models/Requests/{0}';", item));
                                }
                            }
                        }

                        // clean up extra trailing ,
                        if (results.Count > 0)
                        {
                            results[results.Count - 1] = results[results.Count - 1].Trim(',');
                        }

                        if (!isPartialClass)
                        {
                            results.Add(Indent(indent) + "// </Standard Methods>");
                            indent--;
                            results.Add(Indent(indent) + "}");
                        }
                        inClass = false;
                        continue;
                    }

                    if (inMethod)
                    {
                        if (bracketCount == 1)
                        {
                            // done with method
                            indent--;
                            results.Add(Indent(indent) + "}");
                            inMethod = false;
                            inPassThrough = false;
                        }
                        else
                        {
                            if (line.Contains("RestRequest"))
                            {
                                // starting method
                                results.Add(Indent(indent) + string.Format("var request = new {0}Request();", this.ProjectName));
                                results.Add(Indent(indent) + string.Format("request.method = '{0}';", GetMethod(line)));
                            }
                            else if (line.Contains("request.Resource"))
                            {
                                string resource = line
                                                    .Replace(" ", "")
                                                    .Replace("request.Resource=", "")
                                                    .Replace("\"", "")
                                                    .Replace(";", "");
                                results.Add(Indent(indent) + string.Format("request.resource = '{0}';", resource));
                            }
                            else if (line.Contains("AddUrlSegment"))
                            {
                                string[] values = line.Replace("request.AddUrlSegment", "")
                                    .Replace(";", "")
                                    .Replace(".ToString()", "")
                                    .Trim('(')
                                    .Trim(')')
                                    .Split(',');
                                if (values[1].Contains("(")) // we must be casting something, ignore casts in js
                                {
                                    values[1] = Regex.Replace(values[1], @"(\(int.*?\))", "").Trim().Trim('(');
                                }
                                if (values[1].Trim() == "priority")
                                {
                                    values[1] = values[1].Trim() + ".toString()";
                                }

                                results.Add(Indent(indent) + string.Format("request.resource = request.resource.replace(/{{{0}}}/g, {1}.toString());", values[0].Trim('"'), values[1].Trim()));
                            }
                            else if (line.Contains("AddParameter"))
                            {
                                string[] values = line.Replace(".HasValue", "")
                                    .Replace("request.AddParameter", "")
                                    .Replace(";", "")
                                    .Replace(".ToString()", "")
                                    .Replace("string.Empty", "\"\"")
                                    .Trim('(')
                                    .Trim(')')
                                    .Split(',');

                                if (values[1].Contains("(")) // we must be casting something, ignore casts in js
                                {
                                    values[1] = Regex.Replace(values[1], @"(\(int.*?\))", "");
                                }
                                results.Add(Indent(indent) + string.Format("request.params[{0}] = {1};", values[0], values[1]));
                            }
                            else if (line.Contains("AddJsonBody"))
                            {
                                string value = line.Replace("request.AddJsonBody", "")
                                    .Replace(";", "")
                                    .Replace(".ToString()", "")
                                    .Trim('(')
                                    .Trim(')');
                                if (value.StartsWith("new"))
                                {
                                    value = Regex.Replace(value, "(new .*?{)", "{").Replace("=", ":").Replace(" :", ":");
                                }
                                if (value == "package")
                                {
                                    value = "item";
                                }
                                results.Add(Indent(indent) + string.Format("request.payload = JSON.stringify({0});", value));
                            }
                            else if (line.Contains("Sdk.Execute"))
                            {
                                results.Add(Indent(indent) + "return this.api.execute(request);");
                            }
                            else if (line.Contains("Sdk.Download"))
                            {
                                results.Add(Indent(indent) + "return this.api.execute(request);");
                            }
                            else
                            {
                                //assume its pass through
                                inPassThrough = true;
                                results.Add(Indent(indent) + line
                                    .Replace("string ", "string ")
                                    .Replace(".ToString()", "")
                                    .Replace("this.Sdk.BaseUrl", "this.api.baseUrl")
                                    .Replace("Sdk.BaseUrl", "this.api.baseUrl"));
                            }
                        }
                    }
                    else if (line.StartsWith("public Task"))  // Process as method
                    {
                        string signature = Regex.Replace(line, @"(public Task.*?> )", "");

                        string[] split = signature
                                            .Trim(')')
                                            .Split('(');
                        string[] parameterSplit = split[1].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        List<string> parameters = new List<string>();
                        foreach (string item in parameterSplit)
                        {
                            Match matches = Regex.Match(item.Trim(), @"(.*?) (.*?)$");
                            if (matches.Success)
                            {
                                string kind = matches.Groups[1].Value.Trim();
                                string name = matches.Groups[2].Value;
                                if (name == "package")
                                {
                                    name = "item";
                                }
                                string defaultValue = string.Empty;
                                if (name.Contains("="))
                                {
                                    string[] splits = name.Replace(" ", "").Split('=');
                                    name = splits[0];
                                    defaultValue = splits[1];
                                }
                                string parameterItem = string.Empty;
                                if (kind == "Guid")
                                {
                                    parameterItem = string.Format("{0}:string", name, kind);
                                }
                                else if (kind == "int" || kind == "long" || kind == "decimal")
                                {
                                    parameterItem = string.Format("{0}:number", name, kind);
                                }
                                else if (kind == "bool")
                                {
                                    parameterItem = string.Format("{0}:boolean", name, kind);
                                }
                                else
                                {
                                    if (includeInputClasses && kind.ToLower().Contains("input"))
                                    {
                                        imports.Add(kind);
                                        parameterItem = string.Format("{0}:{1}", name, kind);
                                    }
                                    else
                                    {
                                        parameterItem = string.Format("{0}:any", name, kind);
                                    }
                                }
                                if (!string.IsNullOrEmpty(defaultValue))
                                {
                                    if (defaultValue.Contains(".")) // enum default
                                    {
                                        parameterItem += " = null";
                                    }
                                    else
                                    {
                                        parameterItem += " = " + defaultValue;
                                    }
                                }
                                parameters.Add(parameterItem);
                            }
                        }

                        string methodName = Char.ToLowerInvariant(split[0][0]) + split[0].Substring(1);
                        if (!methodName.Contains("<")) // skip generic overloads
                        {
                            results.Add(Indent(indent) + string.Format("{0}({1}) {{", methodName, string.Join(", ", parameters)));

                            inMethod = true;
                            indent++;
                        }
                    }
                    else if (line.StartsWith("public string"))  // Process as string method
                    {
                        string signature = line.Replace(@"public string ", "");
                        string[] split = signature
                                            .Trim(')')
                                            .Split('(');
                        string[] parameterSplit = split[1].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        List<string> parameters = new List<string>();
                        foreach (string item in parameterSplit)
                        {
                            string paramItem = Regex.Replace("," + item.Trim(), @",(.*?) (.*?)", "$2");

                            if (paramItem == "package")
                            {
                                paramItem = "item";
                            }
                            parameters.Add(paramItem);
                        }

                        results.Add(Indent(indent) + string.Format("{0}({1}) {{", split[0], string.Join(", ", parameters)));
                        inMethod = true;
                        indent++;
                    }
                }

            }

            if (results.Count > 0)
            {
                // short circuit if match
                if (File.Exists(outputFile))
                {
                    string[] foundContent = File.ReadAllLines(outputFile);
                    if (foundContent.SequenceEqual(results))
                    {
                        return classNames.ToList(); // no need to write it
                    }
                }

                if (isPartialClass)
                {
                    string content = File.ReadAllText(partialMasterFile);
                    string newContent = "// <Extended Methods>\n" + string.Join("\n", results) + "\n   // </Extended Methods>\n\n   // <Standard Methods>";
                    content = content.Replace("// <Standard Methods>", newContent);

                    if (imports.Count > 0)
                    {
                        List<string> importLines = new List<string>();
                        foreach (string item in imports)
                        {
                            importLines.Insert(0, string.Format("import {{ {0} }} from '../models/Requests/{0}';", item));
                        }
                        content = string.Join("\n", importLines) + "\n" + content;
                    }
                    File.WriteAllText(partialMasterFile, content);
                    return classNames.ToList();
                }

                // write normal
                File.WriteAllText(outputFile, string.Join("\n", results));
            }

            return classNames.ToList();
        }
        private string Indent(int indent)
        {
            if (indent == 0) { return string.Empty; }
            return new string(' ', indent * 3);
        }
        private string GetMethod(string line)
        {
            if (line.Contains("PUT"))
            {
                return "PUT";
            }
            if (line.Contains("POST"))
            {
                return "POST";
            }
            if (line.Contains("DELETE"))
            {
                return "DELETE";
            }
            return "GET";
        }

    }
}
