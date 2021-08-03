using BLL.Interface.Dto;
using BLL.Interface.Interface;
using DAL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.Interface.Exception;

namespace BLL.Local.Services
{
    public class StubSexService : StubBaseIntService<SexDto>, ISexService
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
        protected override bool UseUniqueValidation() { return true; }

        protected override bool HasSameItem(SexDto dto)
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
