using System;
using System.Collections.Generic;
using System.Text;
using BLL.Interface;

namespace DAL.EF.EF.Entities
{
    // Для использования базового репозитория тербуется IEntityWithId с типом ключа
    public partial class AcademicPerformance : IEntityWithId<int>
    {
    }
}
