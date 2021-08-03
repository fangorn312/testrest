using System;
using System.Collections.Generic;
using System.Text;
using BLL.Interface;
using DAL.EF;
namespace DAL.EF
{
    public static class LocalDtoExtension
    {
        public static Dto ConvertToDto<Entity, Dto>(this ILocalDto<Entity, Dto> local, Entity country) where Dto : IBaseDto
        {
            if (country == null)
                return default(Dto);
            return (Dto)local.ConvertToDtoInner(country);
        }

        public static Entity ConvertFromDto<Entity, Dto>(this ILocalDto<Entity, Dto> local, Dto dto) where Dto : IBaseDto
        {
            if (dto == null)
                return default(Entity);
            return local.ConvertFromDtoInner(dto);
        }
    }
}
