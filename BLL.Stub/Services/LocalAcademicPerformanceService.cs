using System;
using System.Collections.Generic;
using System.Text;
using BLL.Interface.Dto;
using BLL.Interface.Interface;
using BLL.Local.Services.Base;
using DAL.Interface;

namespace BLL.Local.Services
{
    public class LocalAcademicPerformanceService : LocalBaseCrudService<AcademicPerformanceDto, int>, IAcademicPerformanceService
    {
        public LocalAcademicPerformanceService(IUnitOfWork uow) : base(uow) { }
    }
}
