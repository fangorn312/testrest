using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using BLL.Interface;
using BLL.Local.Services;
using BLL.Local.Services.Base;
using DAL.EF;
using DAL.Interface;

// Основной класс-координатор слоя логики
namespace BLL.Local
{
    public partial class LocalProvider : IComplexProvider
    {
        private Dictionary<Type, object> AllServices;
       
        private IUnitOfWork uow;

        public LocalProvider(IUnitOfWork unitOfWork)
        {
            AllServices = new Dictionary<Type, object>();
            uow = unitOfWork;
        }

        public void UseOneService(Type dtoType)
        {
            UseCustomServices(dtoType);
        }

        // Может использоваться для инициализации всех сервисов
        // Удобно использовать для небольших проектов
        public void UseAllServices()
        {
            throw new NotImplementedException();
        }
        
        // Получение сервиса для операций, где не требуется фильтр по id
        // Например, получение всего списка
        public ICrudService<Dto> Set<Dto>() where Dto : IBaseDto
        {
            var type = typeof(Dto);
            if (!AllServices.ContainsKey(type))
                throw new NotImplementedException();

            return (ICrudService<Dto>)AllServices[type];
        }

        // Получение сервиса для операций, где нужен фильтр по id
        // Например, получение одной записи
        public ICrudService<Dto, KeyType> Set<Dto, KeyType>() where Dto : IBaseDto, IEntityWithId<KeyType>
        {
            var type = typeof(Dto);
            if (!AllServices.ContainsKey(type))
                throw new NotImplementedException();

            return AllServices[type] as ICrudService<Dto, KeyType>;
        }
    }
}
