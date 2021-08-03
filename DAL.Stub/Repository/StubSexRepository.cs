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
    public class StubSexRepository : StubBaseIdIntRepository<SexDto>, ISexRepository
    {
        #region Init test Data
        protected override void InitData()
        {
            var sexFemale = new SexDto()
            {
                id = 1,
                code = "female",
                name = "Женский",
                description = ""
            };
            var sexMale = new SexDto()
            {
                id = 2,
                code = "male",
                name = "Мужской",
                description = ""
            };
            TheWholeEntities = new List<SexDto>() { sexFemale, sexMale };
        }
        #endregion

        #region CUD
        public override bool HasSameItem(SexDto dto)
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
