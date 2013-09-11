using System.Web.Script.Serialization;

namespace WebBuilder.Utils
{
    public class CmdParameter
    {
        private static JavaScriptSerializer serializer = new JavaScriptSerializer();

        public static CmdParameter Create(string parameter)
        {
            return serializer.Deserialize<CmdParameter>(parameter);
        }

        public string platform { get; set; }

        public string inDir { get; set; }

        public string outDir { get; set; }

        public bool optimize { get; set; }

        public bool obfuscate { get; set; }

        public string encoding { get; set; }

        public int lineBreak { get; set; }

        public bool ignoreEval { get; set; }

        public bool removeComments { get; set; }

        public bool addMark { get; set; }

        public string docDir { get; set; }
    }
}

