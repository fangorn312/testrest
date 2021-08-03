using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using BLL.Interface.Dto;
using BLL.Interface.Interface;
using DAL.Interface.Repository;
using Microsoft.EntityFrameworkCore;

namespace DAL.EF.Repository
{
    public class StubAcademicPerformanceRepository : 
        StubBaseIdIntRepository<AcademicPerformanceDto>, 
        IAcademicPerformanceRepository
    {
        #region Init test Data
        protected override void InitData()
        {
            var score1 = new AcademicPerformanceDto()
            {
                id = 1,
                code = "verybad",
                name = "Фиаско",
                description = ""
            };
            var score2 = new AcademicPerformanceDto()
            {
                id = 2,
                code = "bad",
                name = "Неудовлетворительно",
                description = ""
            };
            var score3 = new AcademicPerformanceDto()
            {
                id = 3,
                code = "satisfying",
                name = "Удовлетворительно",
                description = ""
            };
            var score4 = new AcademicPerformanceDto()
            {
                id = 4,
                code = "good",
                name = "Хорошо",
                description = ""
            };
            var score5 = new AcademicPerformanceDto()
            {
                id = 5,
                code = "excellent",
                name = "Отлично",
                description = ""
            };
            TheWholeEntities = new List<AcademicPerformanceDto>() { score1, score2, score3, score4, score5 };
        }
        #endregion

        #region CUD
        public override bool HasSameItem(AcademicPerformanceDto dto)
        {
            return TheWholeEntities.Any(x =>
                   x.code.ToLower() == dto.code.ToLower()
                && x.description.ToLower() == dto.description.ToLower()
                && x.name.ToLower() == dto.name.ToLower()
            );
        }

        #endregion
    }
}
