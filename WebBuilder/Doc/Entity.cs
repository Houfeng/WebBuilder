using System.Collections.Generic;

namespace WebBuilder.Doc
{
    public class Entity
    {
        public static bool IsEntity(string comment)
        {
            return comment.Contains("@module") || comment.Contains("@class");
        }

        public static string GetEntityType(string comment)
        {
            if (comment.Contains("@class"))
                return "class";
            else
                return "module";
        }


        public Entity(string comment)
        {
            this.Members = new List<Member>();

        }
        public string Name { get; set; }
        public string Descript { get; set; }
        public List<Member> Members { get; set; }
    }
}
