using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.Interface;
using DAL.EF.EF.Context;
using DAL.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DAL.EF.Repository.Base
{
    // Базовый репозиторий
    // Dto - выходная dto-шка, будет далее использоваться на слое логики
    // LocalDto - конвертация
    // Entity - сущность из EF
    // KeyType - тип данных ключа в dto и entity
    public class EfCrudRepository<Entity, Dto, LocalDto, KeyType> : ICrudRepository<Dto, KeyType>
        where Dto : IBaseDto, IEntityWithId<KeyType>
        where LocalDto : ILocalDto<Entity, Dto>, new()
        where Entity : class, IEntityWithId<KeyType>
    {

        protected EfCrudRepository(TestRestContext context)
        {
            _db = context;
        }
        #region Context
        private TestRestContext _db;

        internal TestRestContext db
        {
            get
            {
                if (_db == null)
                    throw new NullReferenceException();
                return _db;
            }
        }
        #endregion

        #region Load Data
        public virtual List<Dto> Items()
        {
            return TheWholeEntities.OrderBy(x=>x.id).ToList().Select(x => new LocalDto().ConvertToDto(x)).ToList();
        }

        public virtual Dto GetOneById(KeyType id)
        {
            var item = TheWholeEntities.FirstOrDefault(x => x.id.Equals(id));
            return new LocalDto().ConvertToDto(item);
        }

        #endregion

        #region CUD

        public virtual Dto Add(Dto dto)
        {
            var entity = new LocalDto().ConvertFromDto(dto);
            db.Set<Entity>().Add(entity);
            db.SaveChanges();
            db.Entry(entity).State = EntityState.Detached;
            return GetOneById(entity.id);
        }

        public virtual Dto Update(Dto dto)
        {
            var entity = new LocalDto().ConvertFromDto(dto);
            db.Entry(entity).State = EntityState.Modified;
            db.SaveChanges();
            db.Entry(entity).State = EntityState.Detached;
            return GetOneById(dto.id);
        }


        public virtual void RemoveById(KeyType id)
        {
            var entity = db.Set<Entity>().FirstOrDefault(x => x.id.Equals(id));
            db.Set<Entity>().Remove(entity);
            db.SaveChanges();
            db.Entry(entity).State = EntityState.Detached;
        }

        // На случай использования проверки на уникальность без её определения
        // Отвалится на интеграционном тесте с тестовой базой
        public virtual bool HasSameItem(Dto dto)
        {
            throw new NotImplementedException();
        }

        #endregion
        // Читаем данные без вноса в контекст, тк данные меняются в слое логики, а хранение контекста для этого не требуется
        protected virtual IQueryable<Entity> TheWholeEntities
        {
            get
            {
                return db.Set<Entity>().AsQueryable().AsNoTracking();
            }
        }
    }
}
