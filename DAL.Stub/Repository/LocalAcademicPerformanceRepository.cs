using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using BLL.Interface.Dto;
using BLL.Interface.Interface;
using DAL.EF.Dto;
using DAL.EF.EF.Entities;
using DAL.EF.Repository.Base;
using DAL.Interface.Repository;
using Microsoft.EntityFrameworkCore;

namespace DAL.EF.Repository
{
    public class LocalAcademicPerformanceRepository :
        EfCrudRepository<AcademicPerformance, AcademicPerformanceDto, LocalAcademicPerformanceDto, int>,
        IAcademicPerformanceRepository
    {


        public override AcademicPerformanceDto GetOneById(int id)
        {
            var item = TheWholeEntities
                .FirstOrDefault(x => x.id == id);
            return new LocalAcademicPerformanceDto().ConvertToDto(item);
        }

        public override List<AcademicPerformanceDto> Items()
        {
            return TheWholeEntities
                .Select(x => new LocalAcademicPerformanceDto().ConvertToDto(x)).ToList();
        }
    }
}
