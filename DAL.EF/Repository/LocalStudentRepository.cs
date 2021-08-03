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
    public class LocalStudentRepository :
        EfCrudRepository<Student, StudentDto, LocalStudentDto, int>,
        IStudentRepository
    {
        public LocalStudentRepository(TestRestContext context) : base(context) { }
        // Добавляем нужные справочники и тд
        protected override IQueryable<Student> TheWholeEntities
        {
            get
            {
                return db.Set<Student>()
                    .Include(x => x.idSexNavigation)
                    .Include(x => x.idAcademicPerformanceNavigation)
                    .AsQueryable()
                    .AsNoTracking();
            }
        }

        // Переопределяем проверку на уникальность
        public override bool HasSameItem(StudentDto dto)
        {
            return TheWholeEntities.Any(x =>
                   x.surName.ToLower() == dto.surName.ToLower()
                && x.firstName.ToLower() == dto.firstName.ToLower()
                && x.secondName.ToLower() == dto.secondName.ToLower()
                && x.dob == dto.dob
                && x.id != dto.id
            );
        }
    }
}
