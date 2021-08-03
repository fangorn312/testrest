using BLL.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Interface
{
    // Репозиторий для доступа без id
    public interface ICrudRepository<Dto>
    {
        List<Dto> Items();
        Dto Add(Dto dto);
        Dto Update(Dto dto);
        bool HasSameItem(Dto dto);
    }
    // Репозиторий для доступа с id
    public interface ICrudRepository<Dto, KeyType>: ICrudRepository<Dto>
        where Dto: IEntityWithId<KeyType>
    {
        Dto GetOneById(KeyType id);
        void RemoveById(KeyType id);

    }
}
