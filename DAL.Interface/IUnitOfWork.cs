using System;
using System.Collections.Generic;
using System.Text;
using BLL.Interface;

namespace DAL.Interface
{
    public partial interface IUnitOfWork
    { 
        ICrudRepository<Dto, KeyType> Set<Dto, KeyType>() where Dto : IBaseDto, IEntityWithId<KeyType>;
        ICrudRepository<Dto> Set<Dto>() where Dto : IBaseDto;
    }
}
