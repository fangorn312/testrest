using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.Interface;
using BLL.Interface.Dto;
using DAL.EF.EF.Entities;

namespace DAL.EF.Dto
{
    // Конвертация между Dto и сущностью EF туда и обратно
    public class LocalStudentDto : StudentDto, ILocalDto<Student, StudentDto>
    {
        public StudentDto ConvertToDtoInner(Student _Student)
        {
            var dto = new LocalStudentDto()
            {
                id = _Student.id,
                surName = _Student.surName,
                firstName = _Student.firstName,
                secondName = _Student.secondName,
                dob = _Student.dob,

                idSex = _Student.idSex,
                idSexNavName = _Student.idSexNavigation?.name,
                idSexNavCode = _Student.idSexNavigation?.code,
                idAcademicPerformance = _Student.idAcademicPerformance,
                idAcademicPerformanceNavName = _Student.idAcademicPerformanceNavigation?.name,
                idAcademicPerformanceNavCode = _Student.idAcademicPerformanceNavigation?.code
            };
            return dto;
        }

        public Student ConvertFromDtoInner(StudentDto dto)
        {
            var _Student = new Student()
            {
                id = dto.id,
                surName = dto.surName,
                firstName = dto.firstName,
                secondName = dto.secondName,
                dob = dto.dob,
                idSex = dto.idSex,
                idAcademicPerformance = dto.idAcademicPerformance
            };


            return _Student;
        }
    }
}
