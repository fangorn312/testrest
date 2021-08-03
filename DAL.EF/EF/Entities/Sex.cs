using System;
using System.Collections.Generic;

#nullable disable

namespace DAL.EF.EF.Entities
{
    public partial class Sex
    {
        public Sex()
        {
            Students = new HashSet<Student>();
        }

        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string code { get; set; }

        public virtual ICollection<Student> Students { get; set; }
    }
}
