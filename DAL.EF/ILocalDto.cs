using System;
using System.Collections.Generic;
using System.Text;
using BLL.Interface;

namespace DAL.EF
{
    public interface ILocalDto<Entity, Dto> where Dto: IBaseDto
    {
        Entity ConvertFromDtoInner(Dto dto);
        Dto ConvertToDtoInner(Entity country);
    }
}
