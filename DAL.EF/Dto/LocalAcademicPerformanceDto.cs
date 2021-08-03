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
    public class LocalAcademicPerformanceDto : AcademicPerformanceDto, ILocalDto<AcademicPerformance, AcademicPerformanceDto>
    {
        public AcademicPerformanceDto ConvertToDtoInner(AcademicPerformance _AcademicPerformance)
        {
            var dto = new LocalAcademicPerformanceDto()
            {
                id = _AcademicPerformance.id,
                name = _AcademicPerformance.name,
                description = _AcademicPerformance.description,
                code = _AcademicPerformance.code,

            };
            return dto;
        }

        public AcademicPerformance ConvertFromDtoInner(AcademicPerformanceDto dto)
        {
            var _AcademicPerformance = new AcademicPerformance()
            {
                id = dto.id,
                name = dto.name,
                description = dto.description,
                code = dto.code
            };


            return _AcademicPerformance;
        }
    }
}
