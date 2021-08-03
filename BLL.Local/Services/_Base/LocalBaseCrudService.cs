using System;
using System.Collections.Generic;
using System.Text;
using BLL.Interface;
using BLL.Interface.Exception;
using DAL.Interface;

// Основной базовый класс для всех сервисов. Если требуются какие-то особые функции, можно переопределить
namespace BLL.Local.Services.Base
{
    public class LocalBaseCrudService<Dto, KeyType> : ICrudService<Dto, KeyType>
        where Dto : IBaseDto, IEntityWithId<KeyType>, new()
    {
        protected IUnitOfWork db { get; private set; }

        #region Ctor
        protected LocalBaseCrudService(IUnitOfWork uow)
        {
            SetUnitOfWork(uow);
        }
        
        // На случай кастомного конструктора в дочернем классе для доступа к координатору
        protected void SetUnitOfWork(IUnitOfWork uow)
        {
            db = uow;
        }
        #endregion

        #region Load Data
        // Получает всю коллекцию. 
        public virtual IEnumerable<Dto> Items()
        {
            // db.Set - выдаёт нужный репозиторий через координатор
            return db.Set<Dto>().Items();
        }

        // Получение одного элемента
        public virtual Dto GetOneById(KeyType id)
        {
            // db.Set - выдаёт нужный репозиторий через координатор
            var item = db.Set<Dto, KeyType>().GetOneById(id);
            if(item == null)
            {
                throw MakeNullReferenceWithId(id);
            }
            return item;
        }
        #endregion

        #region CUD
        // Может использоваться для каких-то дополнительных действий.
        // Н-р логирование на конкретных сервисах
        protected virtual void BeforeAdd(Dto item) { }
        // Может использоваться для каких-то кастомных валидаций
        protected virtual string ValidateAdd(Dto item) { return string.Empty; }
        // Отображает необходимость проверки уникальности записи перед добавлением
        // Если для какого-то сервиса нужна - переопределяется в сервисе на true
        protected virtual bool UseUniqueValidation => false;

        // Добавление новой записи
        public virtual Dto Add(Dto item)
        {
            if (!item.IsNew())
            {
                throw MakeInvalidOperationException(item.id);
            }
            if (UseUniqueValidation)
            {
                var hasSameItem = db.Set<Dto>().HasSameItem(item);
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
            BeforeAdd(item);
            return db.Set<Dto>().Add(item);
        }

        // Как и при добавлении может использоваться для каких-то дополнительных действий
        protected virtual void BeforeUpdate(Dto item) { }
        protected virtual string ValidateUpdate(Dto item) { return string.Empty; }
        // Обновление записи
        public virtual Dto Update(Dto item)
        {
            var itemToUpdate = db.Set<Dto, KeyType>().GetOneById(item.id);
            if (itemToUpdate == null)
            {
                throw MakeNullReferenceWithItem(item);
            }
            if (UseUniqueValidation)
            {
                var hasSameItem = db.Set<Dto>().HasSameItem(item);
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
            BeforeUpdate(item);
            return db.Set<Dto>().Update(item);
        }

        // Как и при добавлении может использоваться для каких-то дополнительных действий
        protected virtual void BeforeDelete(KeyType id) { }
        // Удаление записи
        public virtual void RemoveById(KeyType id)
        {
            var itemToRemove = db.Set<Dto, KeyType>().GetOneById(id);
            if (itemToRemove == null)
            {
                throw MakeNullReferenceWithId(id);
            }
            BeforeDelete(id);
            db.Set<Dto, KeyType>().RemoveById(id);
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
