using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace BLL.Interface.Dto
{
    public class SexDto : DtoInt
    {
        public string name { get; set; }
        public string description { get; set; }
        public string code { get; set; }


        public override int GetHashCode() => (id, name, code, description).GetHashCode();
        public override bool Equals(object other) => other is SexDto dto && Equals(dto);

        public bool Equals(SexDto dto)
        {
            return
                   id == dto.id
                && name == dto.name
                && description == dto.description
                && code == dto.code;
        }
    }
}
