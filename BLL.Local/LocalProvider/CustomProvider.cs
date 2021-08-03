using System;
using System.Collections.Generic;
using System.Text;
using BLL.Interface;
using BLL.Interface.Dto;
using BLL.Interface.Interface;
using BLL.Local.Services;
using BLL.Local.Services.Base;
using DAL.EF;
using DAL.Interface;

// Инициализация сервсисов для Dto, нужна для доступа через координатор IComplexProvider
namespace BLL.Local
{
    public partial class LocalProvider
    {
        private LocalSexService LocalSexService;
        private LocalStudentService LocalStudentService;
        private LocalAcademicPerformanceService LocalAcademicPerformanceService;

        public void UseCustomServices(Type dtoType)
        {
            if (typeof(SexDto) == dtoType)
            {
                LocalSexService = new LocalSexService(uow);
                AllServices.Add(typeof(SexDto), LocalSexService);
            }
            if (typeof(StudentDto) == dtoType)
            {
                LocalStudentService = new LocalStudentService(uow);
                AllServices.Add(typeof(StudentDto), LocalStudentService);
            }
            if (typeof(AcademicPerformanceDto) == dtoType)
            {
                LocalAcademicPerformanceService = new LocalAcademicPerformanceService(uow);
                AllServices.Add(typeof(AcademicPerformanceDto), LocalAcademicPerformanceService);
            }
        }

        public ISexService Sex { get { return LocalSexService; } }
        public IStudentService Student { get { return LocalStudentService; } }
        public IAcademicPerformanceService AcademicPerformance { get { return LocalAcademicPerformanceService; } }


    }
}
