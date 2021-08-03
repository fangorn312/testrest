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
    // «аглушка реализует CRUD-методы поверх списка
    // Dto - основна€ используема€ Dto
    // KeyType - ключ этой Dto, может быть любого типа данных
    public class StubBaseService<Dto, KeyType> 
        where Dto : IEntityWithId<KeyType>, IBaseDto, ICloneable
    {
        protected List<Dto> TheWholeEntities;

        #region Ctor
        public StubBaseService()
        {
            InitData();
        }

        protected virtual void InitData() { throw new NotImplementedException(); }
        #endregion

        #region Load Data
        public Dto GetOneById(KeyType id)
        {
            var item = TheWholeEntities
                .FirstOrDefault(x => x.id.Equals(id));

            if (item == null)
            {
                throw MakeNullReferenceWithId(id);
            }

            return item;
        }

        public IEnumerable<Dto> Items()
        {
            return TheWholeEntities;
        }
        #endregion

        #region CUD

        protected virtual string ValidateAdd(Dto item) { return string.Empty; }
        protected virtual bool UseUniqueValidation() { return false; }

        public virtual Dto Add(Dto item)
        {
            if (!item.IsNew())
            {
                throw MakeInvalidOperationException(item.id);
            }
            if (UseUniqueValidation())
            {
                var hasSameItem = HasSameItem(item);
                if (hasSameItem)
                {
                    throw MakeNonUniqueException();
                }
            }
            var validationMessage = ValidateAdd(item);
            if (!string.IsNullOrWhiteSpace(validationMessage))
            {
                throw MakeValidationException(validationMessage);
            }

            var newItem = (Dto)item.Clone();


            var id = GetNextKey();
            newItem.id = id;

            TheWholeEntities.Add(newItem);
            return newItem;
        }

        protected virtual KeyType GetNextKey()
        {
            throw new NotImplementedException();
        }

        protected virtual string ValidateUpdate(Dto item) { return string.Empty; }
        public Dto Update(Dto item)
        {
            var itemToUpdate = TheWholeEntities.FirstOrDefault(x=>x.id.Equals(item.id));
            if (itemToUpdate == null)
            {
                throw MakeNullReferenceWithItem(item);
            }
            if (UseUniqueValidation())
            {
                var hasSameItem = HasSameItem(item);
                if (hasSameItem)
                {
                    throw MakeNonUniqueException();
                }
            }
            var validationMessage = ValidateUpdate(item);
            if (!string.IsNullOrWhiteSpace(validationMessage))
            {
                throw MakeValidationException(validationMessage);
            }

            var index = TheWholeEntities.IndexOf(itemToUpdate);

            var newItem = (Dto)item.Clone();

            TheWholeEntities[index] = newItem;
            return item;
        }

        public void RemoveById(KeyType id)
        {
            var itemToRemove = TheWholeEntities.FirstOrDefault(x=>x.id.Equals(id));
            if (itemToRemove == null)
            {
                throw MakeNullReferenceWithId(id);
            }
            TheWholeEntities.Remove(itemToRemove);
        }

        protected virtual bool HasSameItem(Dto dto)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Exceptions 
        private NullReferenceException MakeNullReferenceWithItem(Dto item)
        {
            var id = item.id;
            return MakeNullReferenceWithId(id);
        }
        private NullReferenceException MakeNullReferenceWithId(KeyType id)
        {
            var message = MakeMessageNullReferenceWithId(id);
            return new NullReferenceException(message);
        }

        private string MakeMessageNullReferenceWithId(KeyType id)
        {
            return $"{typeof(Dto).Name} with id = {id} not found";
        }

        private InvalidOperationException MakeInvalidOperationException(KeyType id)
        {
            var message = $"This operation is invalid for provided {typeof(Dto).Name}";
            return new InvalidOperationException(message);
        }

        protected CustomValidationException MakeValidationException(string message)
        {
            var msg = $"Validation error for {typeof(Dto).Name}. {message}";
            return new CustomValidationException(msg);
        }

        protected NonUniqueException MakeNonUniqueException()
        {
            var msg = $"{typeof(Dto).Name} with same fields are already exists";
            return new NonUniqueException(msg);
        }
        #endregion

    }
}
