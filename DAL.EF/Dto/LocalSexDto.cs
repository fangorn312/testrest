using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.Interface;
using BLL.Interface.Dto;
using DAL.EF.EF.Entities;

namespace DAL.EF.Dto
{
    // Конвертация между Dto и сущностью EF туда и обратно
    public class LocalSexDto : SexDto, ILocalDto<Sex, SexDto>
    {
        public SexDto ConvertToDtoInner(Sex _Sex)
        {
            var dto = new LocalSexDto()
            {
                id = _Sex.id,
                name = _Sex.name,
                description = _Sex.description,
                code = _Sex.code,

            };
            return dto;
        }

        public Sex ConvertFromDtoInner(SexDto dto)
        {
            var _Sex = new Sex()
            {
                id = dto.id,
                name = dto.name,
                description = dto.description,
                code = dto.code
            };


            return _Sex;
        }
    }
}
