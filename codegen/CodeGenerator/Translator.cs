using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace CodeGenerator
{
	public class Translator
	{
		public event EventHandler<TranslatorEventArgs> Error;

		public event EventHandler<TranslatorEventArgs> Notice;

		public event EventHandler<TranslatorEventArgs> Progress;

		public string DataFile
		{
			get;
			set;
		}

		public List<Template> Templates
		{
			get;
			set;
		}

		public string OutputFolder
		{
			get;
			set;
		}

		public Translator()
		{
			this.Templates = new List<Template>();
		}

		protected virtual void OnError(string errorMessage)
		{
			if (this.Error != null)
			{
				this.Error(this, new TranslatorEventArgs(errorMessage));
			}
		}

		protected virtual void OnNotice(string processMessage)
		{
			if (this.Notice != null)
			{
				this.Notice(this, new TranslatorEventArgs(processMessage));
			}
		}

		protected virtual void OnProgessNotification(decimal percentageComplete)
		{
			if (this.Progress != null)
			{
				this.Progress(this, new TranslatorEventArgs(percentageComplete));
			}
		}

		public void GenFiles()
		{
			this.OnNotice("Parsing Templates");
			string transformedDoc = this.Transform();
			this.OnNotice("Writing Files");
			this.ParseDocument(transformedDoc);
			this.OnNotice("Complete");
		}

		protected void ParseDocument(string transformedDoc)
		{
			try
			{
				int num = 0;
				Regex regex = new Regex("'''\\[STARTFILE:(BASE:|)(?<FileName>.*?)\\](?<Data>(.|\\W)*?)'''\\[ENDFILE\\]");
				MatchCollection matchCollection = regex.Matches(transformedDoc);
				this.OnProgessNotification(num);
				foreach (Match match in matchCollection)
				{
					FileInfo fileInfo = new FileInfo(this.OutputFolder + "\\" + match.Groups["FileName"].ToString());
					string directoryName = fileInfo.DirectoryName;
					try
					{
						if (!Directory.Exists(directoryName))
						{
							Directory.CreateDirectory(directoryName);
						}
						StreamWriter streamWriter = new StreamWriter(this.OutputFolder + "\\" + match.Groups["FileName"].ToString());
						streamWriter.Write(HttpUtility.HtmlDecode(match.Groups["Data"].ToString()));
						streamWriter.Close();
						num++;
						this.OnProgessNotification(decimal.Multiply(decimal.Divide(num, new decimal(matchCollection.Count)), 100m));
					}
					catch (Exception ex)
					{
						this.OnError(ex.Message + "\rProcessing will continue to the next file.");
					}
				}
				this.OnProgessNotification(0m);
			}
			catch (Exception ex)
			{
				this.OnError("Error Loading Document\r\nError: " + ex.Message);
			}
		}

		protected string Transform()
		{
			string result;
			try
			{
				XslTransform xslTransform = new XslTransform();
				StringBuilder stringBuilder = new StringBuilder();
				using (StringWriter stringWriter = new StringWriter(stringBuilder))
				{
					using (XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter))
					{
						XmlResolver resolver = new XmlUrlResolver();
						XPathDocument input = new XPathDocument(this.DataFile);
						List<Template> list = new List<Template>();
						foreach (Template current in this.Templates)
						{
							if (current.IsSelected)
							{
								list.Add(current);
							}
						}
						int num = 0;
						this.OnProgessNotification(num);
						foreach (Template current2 in list)
						{
							try
							{
								xslTransform.Load(current2.Location);
								xslTransform.Transform(input, null, xmlTextWriter, resolver);
							}
							catch (Exception ex)
							{
								this.OnError("Error Transforming Template " + current2.Location + ".\rProcessing will continue.\r\nError: " + ex.Message);
							}
							num++;
							this.OnProgessNotification(decimal.Multiply(decimal.Divide(num, new decimal(list.Count)), 100m));
						}
						result = stringBuilder.ToString();
					}
				}
			}
			catch (Exception ex)
			{
				this.OnError("Error Transforming Templates.\r\nProcessing will stop.\r\nError: " + ex.Message);
				result = string.Empty;
			}
			return result;
		}
	}
}
