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
    public class LocalStudentRepository :
        EfCrudRepository<Student, StudentDto, LocalStudentDto, int>,
        IStudentRepository
    {


        public override StudentDto GetOneById(int id)
        {
            var item = TheWholeEntities
		.Include(x => x.idSexNavigation)
		.Include(x => x.idAcademicPerformanceNavigation)
                .FirstOrDefault(x => x.id == id);
            return new LocalStudentDto().ConvertToDto(item);
        }

        public override List<StudentDto> Items()
        {
            return TheWholeEntities
		.Include(x => x.idSexNavigation)
		.Include(x => x.idAcademicPerformanceNavigation)
                .Select(x => new LocalStudentDto().ConvertToDto(x)).ToList();
        }
    }
}
