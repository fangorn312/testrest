using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Interface
{
    public interface IBaseDto
    {
        bool IsNew();
    }
    public abstract class DtoInt : IEntityWithId<int>, IBaseDto, ICloneable
    {
        public int id { get; set; }

        public virtual object Clone()
        {
            return MemberwiseClone();
        }

        public bool IsNew()
        {
            return id == 0;
        }
    }
}
