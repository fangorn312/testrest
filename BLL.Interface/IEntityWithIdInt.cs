using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Interface
{
    public interface IEntityWithId<KeyType>
    {
        KeyType id { get; set; }
    }
}
