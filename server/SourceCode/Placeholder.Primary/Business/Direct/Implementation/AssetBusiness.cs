using System;

namespace Placeholder.Primary.Business.Direct.Implementation
{
    public partial class AssetBusiness
    {
        public string ResolveMimeType(string fileName)
        {
            return base.ExecuteFunction("ResolveMimeType", delegate ()
            {
                if (string.IsNullOrEmpty(fileName))
                {
                    return string.Empty;
                }

                string extension = System.IO.Path.GetExtension(fileName).ToLower();
                switch (extension)
                {
                    case ".jar":
                    case ".exe":
                    case ".vbs":
                    case ".bat":
                    case ".cmd":
                    case ".sh":
                    case ".pif":
                    case ".application":
                    case ".gadget":
                    case ".msi":
                    case ".msp":
                    case ".com":
                    case ".hta":
                    case ".cpl":
                    case ".msc":
                    case ".vb":
                    case ".vbe":
                    case ".js":
                    case ".je":
                    case ".ws":
                    case ".wsf":
                    case ".wsc":
                    case ".wsh":
                    case ".ps1":
                    case ".ps1xml":
                    case ".ps2":
                    case ".ps2xml":
                    case ".psc1":
                    case ".psc2":
                    case ".msh":
                    case ".msh1":
                    case ".msh2":
                    case ".mshxml":
                    case ".msh1xml":
                    case ".msh2xml":
                    case ".scf":
                    case ".lnk":
                    case ".inf":
                    case ".reg":
                    case ".dmg":
                        throw new Exception("File Type Prohibited");
                    case ".au":
                        return "audio/basic";
                    case ".bmp":
                        return "image/bmp";
                    case ".bz2":
                        return "application/x-bzip2";
                    case ".css":
                        return "text/css";
                    case ".dtd":
                        return "application/xml-dtd";
                    case ".doc":
                        return "application/msword";
                    case ".gif":
                        return "image/gif";
                    case ".gz":
                        return "application/x-gzip";
                    case ".hqx":
                        return "application/mac-binhex40";
                    case ".html":
                        return "text/html";

                    case ".midi":
                        return "audio/x-midi";
                    case ".avi":
                        return "video/msvideo, video/avi, video/x-msvideo";
                    case ".mp3":
                        return "audio/mpeg";
                    case ".mpeg":
                        return "video/mpeg";
                    case ".ogg":
                        return "audio/vorbis, application/ogg";
                    case ".pdf":
                        return "application/pdf";
                    case ".pl":
                        return "application/x-perl";
                    case ".png":
                        return "image/png";
                    case ".ppt":
                        return "application/vnd.ms-powerpoint";
                    case ".ps":
                        return "application/postscript";
                    case ".qt":
                        return "video/quicktime";
                    case ".ra":
                        return "audio/x-pn-realaudio, audio/vnd.rn-realaudio";
                    case ".ram":
                        return "audio/x-pn-realaudio, audio/vnd.rn-realaudio";
                    case ".rdf":
                        return "application/rdf, application/rdf+xml";
                    case ".rtf":
                        return "application/rtf";
                    case ".sgml":
                        return "text/sgml";
                    case ".sit":
                        return "application/x-stuffit";
                    case ".svg":
                        return "image/svg+xml";
                    case ".swf":
                        return "application/x-shockwave-flash";
                    case ".tar.gz":
                        return "application/x-tar";
                    case ".tgz":
                        return "application/x-tar";
                    case ".tiff":
                        return "image/tiff";
                    case ".tsv":
                        return "text/tab-separated-values";
                    case ".txt":
                        return "text/plain";
                    case ".wav":
                        return "audio/wav, audio/x-wav";
                    case ".xls":
                        return "application/vnd.ms-excel";
                    case ".xml":
                        return "application/xml";
                    case ".zip":
                        return "application/zip, application/x-compressed-zip";
                    case ".jpeg":
                        return "image/jpeg";
                    case ".jpg":
                        return "image/jpeg";
                    case ".docx":
                        return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                    case ".text":
                    case ".log":
                        return "text/plain";
                    case ".mp4":
                    case ".m4v":
                    case ".f4v":
                    case ".f4p":
                    case ".mov":
                        return "video/mp4";
                    case "m4a":
                    case "f4a":
                    case "f4b":
                        return "audio/mp4";

                    default:
                        return "application/octet-stream";
                }
            });
        }
    }
}
