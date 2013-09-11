using System.Collections.Generic;

namespace WebBuilder.Doc
{
    public class Member
    {
        public string Name { get; set; }
        public string Descript { get; set; }
        public string Define { get; set; }
        public List<Paramter> Paramters { get; set; }
        public Return Return { get; set; }
        public Member(string comment)
        {
            this.Paramters = new List<Paramter>();
        }
    }
}
