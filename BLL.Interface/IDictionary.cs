using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Interface
{
    public interface IDictionary : IEntityWithId<int>
    {
        string Name { get; set; }
        string Description { get; set; }
        string Code { get; set; }
    }
}
