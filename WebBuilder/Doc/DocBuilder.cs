using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using WebBuilder.Utils;

namespace WebBuilder.Doc
{
    //--{inDir:'C:\wb',docDir:'C:\wb_doc'}
    public class DocBuilder
    {
        private List<Entity> EntityList = new List<Entity>();
        private CmdParameter cmdParameter { get; set; }
        public DocBuilder(CmdParameter cmdParameter)
        {
            this.cmdParameter = cmdParameter;
        }
        public void Excute()
        {
            Console.WriteLine("Generate Doc Start.");
            DirIterater iterater = new DirIterater(this.cmdParameter.inDir);
            iterater.FoundFile += iterater_FoundFile;
            iterater.Start();
            Console.WriteLine("Generate Doc Done.");
        }
        private Entity CurrentEntity = null;
        void iterater_FoundFile(FileInfo file)
        {
            if (Path.GetExtension(file.FullName).ToLower() != ".js") return;
            string content = File.ReadAllText(file.FullName);
            var matchCommentList = Regex.Matches(content, @"/\*\*.*?\*/\s*?\n.*?\n", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            foreach (Match matchComment in matchCommentList)
            {
                var commentValue = matchComment.Value;
                if (Entity.IsEntity(commentValue))
                {
                    if (this.CurrentEntity != null)
                        this.EntityList.Add(CurrentEntity);
                    this.CurrentEntity = new Entity(commentValue);
                }
                else
                {
                    if (this.CurrentEntity != null)
                        this.CurrentEntity = new Entity(string.Format("/**{0}\n@module {0}*/", Path.GetFileNameWithoutExtension(file.Name)));
                    this.CurrentEntity.Members.Add(new Member(commentValue));
                }
            }
        }


    }
}
