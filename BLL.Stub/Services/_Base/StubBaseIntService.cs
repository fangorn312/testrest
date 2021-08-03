using BLL.Interface.Dto;
using BLL.Interface.Interface;
using DAL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.Interface.Exception;
using BLL.Interface;

namespace BLL.Local.Services
{
    public class StubBaseIntService<Dto> : StubBaseService<Dto, int>
        where Dto : IEntityWithId<int>, IBaseDto, ICloneable
    {
        #region CUD
        // Заглушка для получения ключа типа int
        protected override int GetNextKey()
        {
            return TheWholeEntities.Select(x => x.id).Max() + 1;
        }
        #endregion
    }
}
