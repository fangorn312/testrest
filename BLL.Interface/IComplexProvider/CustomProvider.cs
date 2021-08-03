using System;
using System.Collections.Generic;
using System.Text;
using BLL.Interface.Interface;

namespace BLL.Interface
{
    public partial interface IComplexProvider
    {
        ISexService Sex { get; }
        IStudentService Student { get; }
        IAcademicPerformanceService AcademicPerformance { get; }
    }
}