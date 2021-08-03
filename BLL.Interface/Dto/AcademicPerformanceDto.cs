using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace BLL.Interface.Dto
{
    public class AcademicPerformanceDto : DtoInt
    {
        public string name { get; set; }
        public string description { get; set; }
        public string code { get; set; }

        public override int GetHashCode() => (id, name, code, description).GetHashCode();
        public override bool Equals(object other) => other is AcademicPerformanceDto dto && Equals(dto);

        public bool Equals(AcademicPerformanceDto dto)
        {
            return
                   id == dto.id
                && name == dto.name
                && description == dto.description
                && code == dto.code;
        }
    }
}
