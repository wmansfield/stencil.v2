using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Placeholder.SDK.WebGen
{
    public class ModelTranslatorTypeScript
    {
        public ModelTranslatorTypeScript(string projectName, string inputFolder, string outputFolder)
        {
            this.ProjectName = projectName;
            this.InputFolder = inputFolder;
            this.OutputFolder = outputFolder;
        }
        public string InputFolder { get; set; }
        public string OutputFolder { get; set; }
        public string ProjectName { get; set; }

        public void Process(bool onlyEnum)
        {
            Console.WriteLine("WebGen: Starting with input folder: " + this.InputFolder);
            Console.WriteLine("        - input folder: " + this.InputFolder);
            Console.WriteLine("        - output folder: " + this.OutputFolder);

            List<string> fileNamesInternal = new List<string>();

            List<string> files = Directory.EnumerateFiles(this.InputFolder, "*.cs", SearchOption.AllDirectories).ToList();
            files.Sort();
            files.Reverse();// for partial support, _ first
            foreach (string inputFile in files)
            {
                string newFile = inputFile;
                int ix = inputFile.IndexOf(this.InputFolder);
                if (ix >= 0)
                {
                    newFile = inputFile.Substring(ix + this.InputFolder.Length).TrimStart('\\');
                }
                newFile = Path.ChangeExtension(newFile, ".ts");

                Directory.CreateDirectory(this.OutputFolder);

                string outputFile = Path.Combine(this.OutputFolder, newFile);

                Directory.CreateDirectory(Path.GetDirectoryName(outputFile));
                List<string> foundClasses = this.ProcessFile(inputFile, outputFile, true, false, onlyEnum);
                if (foundClasses.Count > 0)
                {
                    fileNamesInternal.Add(newFile);
                }

            }

            this.WriteIndexFile(this.OutputFolder, fileNamesInternal);

            Console.WriteLine("WebGen: Complete");
        }

        private void WriteIndexFile(string outputFolder, List<string> fileNames)
        {
            HashSet<string> files = new HashSet<string>();
            foreach (var item in fileNames)
            {
                files.Add(item);
            }
            fileNames = files.ToList();
            List<string> results = new List<string>();
            foreach (var item in fileNames)
            {
                results.Add(string.Format("export * from './{0}';", Path.ChangeExtension(item, "").Trim('.').Replace(@"\", "/")));
            }

            string output = Path.Combine(outputFolder, "index.ts");

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

        private List<string> ProcessFile(string inputFile, string outputFile, bool includeInternal, bool includeBuilder, bool onlyEnum)
        {
            HashSet<string> classNames = new HashSet<string>();
            List<string> results = new List<string>();
            string[] text = File.ReadAllLines(inputFile);

            bool inClass = false;
            bool inEnum = false;
            int bracketCount = 0;
            int indent = 0;
            string previousLine = string.Empty;
            bool inheriting = false;
            string fileName = Path.GetFileName(outputFile);

            int latestClassIndex = 0;
            List<string> fields = new List<string>();
            List<string> fieldTypes = new List<string>();
            HashSet<string> imports = new HashSet<string>();
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
                if (line.StartsWith("public enum"))
                {
                    string[] classLine = line.Split(':');
                    string className = classLine[0]
                                            .Replace(" ", "")
                                            .Replace("publicenum", "")
                                            .Replace("publicpartialenum", "");

                    classNames.Add(className);
                    inheriting = false;
                    results.Add(Indent(indent) + string.Format("export enum {0} {{", className));
                    indent++;

                    fields.Clear();
                    fieldTypes.Clear();
                    imports.Clear();
                    inEnum = true;
                    continue;
                }
                if (!onlyEnum && (line.StartsWith("public class") || line.StartsWith("public partial class")))
                {
                    latestClassIndex = results.Count - 0;

                    string[] classLine = line.Split(':');
                    string className = classLine[0]
                                            .Replace(" ", "")
                                            .Replace("publicclass", "")
                                            .Replace("publicpartialclass", "")
                                            .Replace(":EndpointBase", "")
                                            .Replace("_Core", ""); ;
                    classNames.Add(className);
                    if (classLine.Length == 1)
                    {
                        inheriting = false;
                        results.Add(Indent(indent) + string.Format("export class {0} {{", className));
                    }
                    else
                    {
                        string baseClass = classLine[1].Replace(":", "").Trim();
                        if (baseClass != "SDKModel")
                        {
                            inheriting = true;
                            string path = ".";
                            if (outputFile.Contains("Responses\\") || outputFile.Contains("Requests\\") || outputFile.Contains("Responses/") || outputFile.Contains("Requests/"))
                            {
                                path = "..";
                            }
                            results.Add(Indent(indent) + string.Format("import {{ {0} }} from \"{1}/{0}\"", baseClass.Replace("[]", "").Replace("List<", "").Replace(">", ""), path));
                            results.Add(Indent(indent) + string.Format("export class {0} extends {1} {{", className, baseClass));
                        }
                        else
                        {
                            results.Add(Indent(indent) + string.Format("export class {0} {{", className));
                        }
                    }
                    indent++;

                    fields.Clear();
                    fieldTypes.Clear();
                    imports.Clear();
                    inClass = true;
                    continue;
                }
                if (inEnum)
                {
                    if (line == "{")
                    {
                        bracketCount++;
                        continue;
                    }
                    if (line == "}")
                    {
                        bracketCount--;
                    }
                    if (bracketCount == 0)
                    {
                        indent--;
                        results.Add(Indent(indent) + "}");

                        inEnum = false;
                        continue;
                    }

                    results.Add(Indent(indent) + string.Format("{0}", line.Trim()));
                }

                if (inClass)
                {
                    if (line == "{")
                    {
                        bracketCount++;
                        continue;
                    }
                    if (line == "}")
                    {
                        bracketCount--;
                    }

                    if (bracketCount == 0)
                    {
                        // clean up extra trailing ,
                        results[results.Count - 1] = results[results.Count - 1].Trim(',');


                        string para = string.Empty;
                        for (int p = 0; p < fields.Count; p++)
                        {
                            string item = fields[p];
                            string kind = fieldTypes[p];

                            // disable default values
                            para += string.Format("{0}=null,", item);
                            /*
                            if (kind == "int" || kind == "long" || kind == "number")
                            {
                                para += string.Format("{0}=0,", item);
                            }
                            else if (kind == "bool")
                            {
                                para += string.Format("{0}=false,", item);
                            }
                            else if (kind == "string" || kind == "date" || kind == "guid")
                            {
                                para += string.Format("{0}='',", item);
                            }
                            else
                            {
                                if (kind.Contains("[]"))
                                {
                                    para += string.Format("{0}=null,", item);
                                }
                                else
                                {
                                    para += string.Format("{0}={{}},", item);
                                }
                            }
                            */
                        }
                        para = "{" + para.Trim(',') + "} = {}";
                        results.Add(Indent(indent) + string.Format("constructor({0}){{", para));
                        if (inheriting)
                        {
                            results.Add(Indent(indent + 1) + "super();");
                        }
                        results.Add(Indent(indent + 1) + string.Format("Object.assign(this, {{ {0} }});", string.Join(", ", fields)));
                        indent++;

                        if (includeBuilder)
                        {
                            foreach (var item in fields)
                            {
                                results.Add(Indent(indent) + string.Format("this.{0} = {0};", item));
                            }
                        }
                        indent--;
                        results.Add(Indent(indent) + "}");


                        indent--;
                        results.Add(Indent(indent) + "}");
                        inClass = false;
                        if (imports.Count > 0)
                        {
                            string path = ".";
                            if (outputFile.Contains("Responses\\") || outputFile.Contains("Requests\\") || outputFile.Contains("Responses/") || outputFile.Contains("Requests/"))
                            {
                                path = "..";
                            }
                            foreach (string item in imports)
                            {
                                if (item.Replace("[]", "") != "string")
                                {
                                    results.Insert(latestClassIndex, string.Format("import {{ {0} }} from \"{1}/{0}\"; ", item.Replace("[]", "").Replace("List<", "").Replace(">", ""), path));
                                }
                            }
                        }

                        imports.Clear();
                        continue;
                    }

                    if (line.Contains("get; set;"))
                    {
                        line = line.Replace(" virtual", "");
                        line = line.Replace("Guid", "string");
                        // silly, but who cares
                        if (line.Contains("Dictionary<string, Dictionary<string, string>>"))
                        {
                            line = line.Replace("Dictionary<string, Dictionary<string, string>>", "any");
                        }
                        if (line.Contains("Dictionary<string, string>"))
                        {
                            line = line.Replace("Dictionary<string, string>", "any");
                        }
                        if (line.Contains("List<"))
                        {
                            line = line.Replace("List<", "").Replace(">", "[]");
                        }
                        Match matches = Regex.Match(line.Replace("get; set;", "").Trim().TrimEnd('}').Trim().TrimEnd('{').Trim(), @"(public|private) (.*?) (.*?)$");
                        if (matches.Success)
                        {
                            string kind = matches.Groups[2].Value.Trim('?');
                            string name = matches.Groups[3].Value;
                            fields.Add(name);
                            fieldTypes.Add(kind);
                            if (kind == "bool")
                            {
                                results.Add(Indent(indent) + string.Format("{0}?:boolean;", name, kind));
                            }
                            else if (kind == "string")
                            {
                                results.Add(Indent(indent) + string.Format("{0}?:string;", name, kind));
                            }
                            else if (kind == "DateTime" || kind == "DateTimeOffset")
                            {
                                results.Add(Indent(indent) + string.Format("{0}?:Date;", name, kind));
                            }
                            else if (kind == "int" || kind == "long" || kind == "decimal" || kind == "float")
                            {
                                results.Add(Indent(indent) + string.Format("{0}?:number;", name, kind));
                            }
                            else
                            {
                                imports.Add(kind);
                                results.Add(Indent(indent) + string.Format("{0}:{1};", name, kind));
                            }

                        }
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
