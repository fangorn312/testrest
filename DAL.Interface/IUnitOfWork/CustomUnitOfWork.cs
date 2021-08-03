using System;
using System.Collections.Generic;
using System.Text;
using BLL.Interface;
using DAL.Interface.Repository;

namespace DAL.Interface
{
    public partial interface IUnitOfWork
    {
        IAcademicPerformanceRepository AcademicPerformance { get; }
        ISexRepository Sex { get; }
        IStudentRepository Student { get; }
    }
}