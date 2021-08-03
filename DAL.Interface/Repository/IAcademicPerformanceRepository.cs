using System;
using System.Collections.Generic;
using System.Text;
using BLL.Interface.Dto;

namespace DAL.Interface.Repository
{
    public interface IAcademicPerformanceRepository: ICrudRepository<AcademicPerformanceDto, int>
    {
    }
}
