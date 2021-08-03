using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace BLL.Interface.Dto
{
    public class StudentDto : DtoInt
    {
        public string surName { get; set; }
        public string firstName { get; set; }
        public string secondName { get; set; }
        public DateTime dob { get; set; }

        public int? idAcademicPerformance { get; set; }
        public string idAcademicPerformanceNavName { get; set; }
        public string idAcademicPerformanceNavCode { get; set; }

        public int idSex { get; set; }
        public string idSexNavName { get; set; }
        public string idSexNavCode { get; set; }

        public override int GetHashCode() => (id, surName, firstName, secondName, dob, 
            idAcademicPerformance, idAcademicPerformanceNavName, idAcademicPerformanceNavCode,
            idSex, idSexNavName, idSexNavCode).GetHashCode();
        public override bool Equals(object other) => other is StudentDto dto && Equals(dto);

        public bool Equals(StudentDto dto)
        {
            return
                surName.ToLower() == dto.surName.ToLower()
                && firstName.ToLower() == dto.firstName.ToLower()
                && secondName.ToLower() == dto.secondName.ToLower()
                && dob == dto.dob
                && idSex == dto.idSex
                && idAcademicPerformance == dto.idAcademicPerformance;
        }
    }
}
