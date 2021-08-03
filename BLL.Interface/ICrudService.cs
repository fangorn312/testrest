using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace BLL.Interface
{
    public interface ICrudService<T> where T: IBaseDto
    {
        IEnumerable<T> Items();
        T Add(T item);
        T Update(T item);
    }

    public interface ICrudService<T, KeyType> : ICrudService<T>
        where T : IBaseDto, IEntityWithId<KeyType>
    {
        T GetOneById(KeyType id);
        void RemoveById(KeyType id);
    }
}
