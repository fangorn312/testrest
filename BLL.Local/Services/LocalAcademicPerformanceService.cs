using System;
using System.Collections.Generic;
using System.Linq;
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

        #region CUD
        // Используем проверку на уникальность в этом сервисе
        protected override bool UseUniqueValidation => true;

        protected override string ValidateAdd(AcademicPerformanceDto item)
        {
            return ValidateCommonAddUpdate(item);
        }

        protected override string ValidateUpdate(AcademicPerformanceDto item)
        {
            return ValidateCommonAddUpdate(item);
        }

        private string ValidateCommonAddUpdate(AcademicPerformanceDto item)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(item.name))
                errors.Add((errors.Count + 1) + ". Name cannot be null or empty");
            if (item.code?.All(char.IsWhiteSpace) == true || item.code?.Length == 0)
                errors.Add((errors.Count + 1) + ". Code name cannot be empty");

            return string.Join("\n", errors);
        }
        #endregion
    }
}
