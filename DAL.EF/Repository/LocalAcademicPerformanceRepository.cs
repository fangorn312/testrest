using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using BLL.Interface.Dto;
using BLL.Interface.Interface;
using DAL.EF.Dto;
using DAL.EF.EF.Context;
using DAL.EF.EF.Entities;
using DAL.EF.Repository.Base;
using DAL.Interface.Repository;
using Microsoft.EntityFrameworkCore;

namespace DAL.EF.Repository
{
    // Переопределены только методы, нужные 
    public class LocalAcademicPerformanceRepository :
        EfCrudRepository<AcademicPerformance, AcademicPerformanceDto, LocalAcademicPerformanceDto, int>,
        IAcademicPerformanceRepository
    {
        public LocalAcademicPerformanceRepository(TestRestContext context) : base(context) { }
        // Переопределяем проверку на уникальность
        public override bool HasSameItem(AcademicPerformanceDto dto)
        {
            return TheWholeEntities.Any(x =>
                   x.code.ToLower() == dto.code.ToLower()
                && x.description.ToLower() == dto.description.ToLower()
                && x.name.ToLower() == dto.name.ToLower()
                && x.id != dto.id
            );
        }
    }
}
