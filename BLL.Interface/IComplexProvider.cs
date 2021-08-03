using System;


namespace BLL.Interface
{
    public partial interface IComplexProvider
    { 
        ICrudService<Dto> Set<Dto>() where Dto : IBaseDto;
        ICrudService<Dto, KeyType> Set<Dto, KeyType>() where Dto : IBaseDto, IEntityWithId<KeyType>;

        void UseAllServices();
        void UseOneService(Type dtoType);

    }
}
