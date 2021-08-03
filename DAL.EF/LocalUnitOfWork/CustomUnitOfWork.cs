using System;
using System.Collections.Generic;
using System.Text;
using BLL.Interface;
using BLL.Interface.Dto;
using DAL.EF.Repository;
using DAL.Interface;
using DAL.Interface.Repository;

namespace DAL.EF
{
    // Связывание вызовов
    public partial class LocalUnitOfWork
    {
        public void CustomUnit()
        {
            Sex = new LocalSexRepository(db);
            Student = new LocalStudentRepository(db);
            AcademicPerformance = new LocalAcademicPerformanceRepository(db);
        }

        public ISexRepository Sex { get; private set; }
        public IStudentRepository Student { get; private set; }
        public IAcademicPerformanceRepository AcademicPerformance { get; private set; }


        public ICrudRepository<Dto> CustomUnit<Dto>() where Dto : IBaseDto
        {
            if (typeof(SexDto) == typeof(Dto)) return (ICrudRepository<Dto>)Sex;
            if (typeof(StudentDto) == typeof(Dto)) return (ICrudRepository<Dto>)Student;
            if (typeof(AcademicPerformanceDto) == typeof(Dto)) return (ICrudRepository<Dto>)AcademicPerformance;

            return null;
        }

        public ICrudRepository<Dto, KeyType> CustomUnit<Dto, KeyType>() where Dto : IBaseDto, IEntityWithId<KeyType>
        {
            if (typeof(SexDto) == typeof(Dto)) return (ICrudRepository<Dto, KeyType>)Sex;
            if (typeof(StudentDto) == typeof(Dto)) return (ICrudRepository<Dto, KeyType>)Student;
            if (typeof(AcademicPerformanceDto) == typeof(Dto)) return (ICrudRepository<Dto, KeyType>)AcademicPerformance;

            return null;
        }
    }
}
