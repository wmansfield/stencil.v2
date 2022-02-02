using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace CodeGenerator
{
	public static class Utility
	{
		public static string SerializeToXml<T>(T item)
		{
			string result;
			if (item == null)
			{
				result = string.Empty;
			}
			else
			{
				StringBuilder stringBuilder = new StringBuilder();
				using (StringWriter stringWriter = new StringWriter(stringBuilder))
				{
					using (XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter))
					{
						xmlTextWriter.Formatting = Formatting.Indented;
						new XmlSerializer(typeof(T)).Serialize(xmlTextWriter, item);
					}
				}
				result = stringBuilder.ToString();
			}
			return result;
		}

		public static void SerializeToXml<T>(T item, FileInfo outFile) where T : new()
		{
			if (item == null)
			{
				item = ((default(T) == null) ? Activator.CreateInstance<T>() : default(T));
			}
			using (TextWriter textWriter = new StreamWriter(outFile.FullName))
			{
				using (XmlTextWriter xmlTextWriter = new XmlTextWriter(textWriter))
				{
					xmlTextWriter.Formatting = Formatting.Indented;
					new XmlSerializer(typeof(T)).Serialize(xmlTextWriter, item);
				}
			}
		}

		public static T DeserializeFromXml<T>(string serializedXML) where T : new()
		{
			T result;
			using (StringReader stringReader = new StringReader(serializedXML))
			{
				using (XmlReader xmlReader = new XmlTextReader(stringReader))
				{
					result = (T)((object)new XmlSerializer(typeof(T)).Deserialize(xmlReader));
				}
			}
			return result;
		}

		public static T DeserializeFromXml<T>(FileInfo inFile) where T : new()
		{
			T result;
			using (TextReader textReader = new StreamReader(inFile.FullName))
			{
				using (XmlReader xmlReader = new XmlTextReader(textReader))
				{
					result = (T)((object)new XmlSerializer(typeof(T)).Deserialize(xmlReader));
				}
			}
			return result;
		}

		public static T XmlClone<T>(T item) where T : new()
		{
			T result;
			if (item == null)
			{
				result = item;
			}
			else
			{
				result = Utility.DeserializeFromXml<T>(Utility.SerializeToXml<T>(item));
			}
			return result;
		}
	}
}
