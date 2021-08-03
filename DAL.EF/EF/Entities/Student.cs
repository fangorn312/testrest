using System;
using System.Collections.Generic;

#nullable disable

namespace DAL.EF.EF.Entities
{
    public partial class Student
    {
        public int id { get; set; }
        public string surName { get; set; }
        public string firstName { get; set; }
        public string secondName { get; set; }
        public DateTime dob { get; set; }
        public int idSex { get; set; }
        public int? idAcademicPerformance { get; set; }

        public virtual AcademicPerformance idAcademicPerformanceNavigation { get; set; }
        public virtual Sex idSexNavigation { get; set; }
    }
}
