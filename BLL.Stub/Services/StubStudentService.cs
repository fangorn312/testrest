using BLL.Interface.Dto;
using BLL.Interface.Interface;
using DAL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.Interface.Exception;

namespace BLL.Local.Services
{
    public class StubStudentService : StubBaseIntService<StudentDto>, IStudentService
    {
        #region Init test Data
        protected override void InitData()
        {
            var studentMale = new StudentDto()
            {
                id = 1,
                surName = "������",
                firstName = "����",
                secondName = "��������",
                dob = new DateTime(2000, 1, 1),
                idSex = 2,
                idSexNavCode = "male",
                idSexNavName = "�������",
                idAcademicPerformance = 1,
                idAcademicPerformanceNavCode = "verybad",
                idAcademicPerformanceNavName = "������"
            };
            var studentFemale = new StudentDto()
            {
                id = 2,
                surName = "������������",
                firstName = "����������",
                secondName = "�������������",
                dob = new DateTime(2002, 2, 2),
                idSex = 1,
                idSexNavCode = "female",
                idSexNavName = "�������",
                idAcademicPerformance = 2,
                idAcademicPerformanceNavCode = "bad",
                idAcademicPerformanceNavName = "�������������������"
            };
            TheWholeEntities = new List<StudentDto>() { studentMale, studentFemale };
        }
        #endregion

        #region CUD
        protected override bool UseUniqueValidation() { return true; }

        protected override bool HasSameItem(StudentDto dto)
        {
            return TheWholeEntities.Any(x =>
                x.surName.ToLower() == dto.surName.ToLower()
                && x.firstName.ToLower() == dto.firstName.ToLower()
                && x.secondName.ToLower() == dto.secondName.ToLower()
                && x.dob == dto.dob
            );
        }
        #endregion

    }
}
