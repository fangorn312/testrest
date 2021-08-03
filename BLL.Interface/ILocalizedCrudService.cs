using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace BLL.Interface
{
    public interface ILocalizedCrudService<T> : ICrudService<T> where T: IBaseDto
    {
       //List<T> LangItems(int? langId);
    }
}
