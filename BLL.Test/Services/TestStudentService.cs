using BLL.Interface.Dto;
using BLL.Interface.Exception;
using BLL.Interface.Interface;
using DAL.EF.Repository;
using DAL.Interface;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Local.Services
{
    [TestFixture]
    public class TestStudentService
    {
        private LocalStudentService service;
        
        [SetUp]
        public void Setup()
        {
            var mock = new Mock<IUnitOfWork>();

            // Подменяем вызовы репозитория на заглушки
            mock.Setup(lr => lr.Student).Returns(new StubStudentRepository());
            mock.Setup(lr => lr.Set<StudentDto>()).Returns(new StubStudentRepository());
            mock.Setup(lr => lr.Set<StudentDto, int>()).Returns(new StubStudentRepository());

            mock.Setup(lr => lr.Sex).Returns(new StubSexRepository());
            mock.Setup(lr => lr.Set<SexDto>()).Returns(new StubSexRepository());
            mock.Setup(lr => lr.Set<SexDto, int>()).Returns(new StubSexRepository());
            
            mock.Setup(lr => lr.AcademicPerformance).Returns(new StubAcademicPerformanceRepository());
            mock.Setup(lr => lr.Set<AcademicPerformanceDto>()).Returns(new StubAcademicPerformanceRepository());
            mock.Setup(lr => lr.Set<AcademicPerformanceDto, int>()).Returns(new StubAcademicPerformanceRepository());

            service = new LocalStudentService(mock.Object);
        }

        #region Load List
        [Test]
        public void GetAll()
        {
            var studentMale = new StudentDto()
            {
                id = 1,
                surName = "Иванов",
                firstName = "Иван",
                secondName = "Иванович",
                dob = new DateTime(2000, 1, 1),
                idSex = 2,
                idSexNavCode = "male",
                idSexNavName = "Мужской",
                idAcademicPerformance = 1,
                idAcademicPerformanceNavCode = "verybad",
                idAcademicPerformanceNavName = "Фиаско"
            };
            var studentFemale = new StudentDto()
            {
                id = 2,
                surName = "Александрова",
                firstName = "Александра",
                secondName = "Александровна",
                dob = new DateTime(2002, 2, 2),
                idSex = 1,
                idSexNavCode = "female",
                idSexNavName = "Женский",
                idAcademicPerformance = 2,
                idAcademicPerformanceNavCode = "bad",
                idAcademicPerformanceNavName = "Неудовлетворительно"
            };
            var neededList = new List<StudentDto>() { studentMale, studentFemale };

            var resultList = service.Items();

            Assert.IsTrue(neededList.SequenceEqual(resultList));
        }
        #endregion

        #region Load One Item
        [Test]
        public void GetOne_Unknown()
        {
            var id = 10;

            Assert.Throws<NullReferenceException>(() => service.GetOneById(id));
        }

        [Test]
        public void GetOne_MaleStudent()
        {
            var neededMaleStudent = new StudentDto()
            {
                id = 1,
                surName = "Иванов",
                firstName = "Иван",
                secondName = "Иванович",
                dob = new DateTime(2000, 1, 1),
                idSex = 2,
                idSexNavCode = "male",
                idSexNavName = "Мужской",
                idAcademicPerformance = 1,
                idAcademicPerformanceNavCode = "verybad",
                idAcademicPerformanceNavName = "Фиаско"
            };

            var resultMaleStudent = service.GetOneById(neededMaleStudent.id);

            Assert.AreEqual(neededMaleStudent, resultMaleStudent);
        }

        [Test]
        public void GetOne_FemaleStudent()
        {
            var neededFemaleStudent = new StudentDto()
            {
                id = 2,
                surName = "Александрова",
                firstName = "Александра",
                secondName = "Александровна",
                dob = new DateTime(2002, 2, 2),
                idSex = 1,
                idSexNavCode = "female",
                idSexNavName = "Женский",
                idAcademicPerformance = 2,
                idAcademicPerformanceNavCode = "bad",
                idAcademicPerformanceNavName = "Неудовлетворительно"
            };

            var resultBad = service.GetOneById(neededFemaleStudent.id);

            Assert.AreEqual(neededFemaleStudent, resultBad);
        }
        #endregion

        #region Add
        [Test]
        public void Add_WithId()
        {
            var newStudent = new StudentDto()
            {
                id = 1,
                surName = "Петров",
                firstName = "Петр",
                secondName = "Петрович",
                dob = new DateTime(2000, 1, 1),
                idSex = 2,
                idSexNavCode = "male",
                idSexNavName = "Мужской",
                idAcademicPerformance = 1,
                idAcademicPerformanceNavCode = "verybad",
                idAcademicPerformanceNavName = "Фиаско"
            };

            Assert.Throws<InvalidOperationException>(() => service.Add(newStudent));
        }

        [Test]
        public void Add_Existing()
        {
            var newStudent = new StudentDto()
            {
                id = 0,
                surName = "Иванов",
                firstName = "Иван",
                secondName = "Иванович",
                dob = new DateTime(2000, 1, 1),
                idSex = 2,
                idSexNavCode = "male",
                idSexNavName = "Мужской",
                idAcademicPerformance = 1,
                idAcademicPerformanceNavCode = "verybad",
                idAcademicPerformanceNavName = "Фиаско"
            };

            Assert.Throws<NonUniqueException>(() => service.Add(newStudent));
        }

        [Test]
        public void Add_New()
        {
            var neededId = 3;
            var newStudent = new StudentDto()
            {
                id = 0,
                surName = "Петров",
                firstName = "Петр",
                secondName = "Петрович",
                dob = new DateTime(2003, 3, 3),
                idSex = 2,
                idSexNavCode = "male",
                idSexNavName = "Мужской",
                idAcademicPerformance = 1,
                idAcademicPerformanceNavCode = "verybad",
                idAcademicPerformanceNavName = "Фиаско"
            };

            var result = service.Add(newStudent);
            newStudent.id = neededId;
            Assert.AreEqual(newStudent, result);
        }
        #endregion

        #region Update
        [Test]
        public void Update_Unknown()
        {
            var updateUnknownStudent = new StudentDto()
            {
                id = 10,
                surName = "Анонимов",
                firstName = "Аноним",
                secondName = "Анонимович",
                dob = new DateTime(2005, 3, 3),
                idSex = 2,
                idSexNavCode = "male",
                idSexNavName = "Мужской",
                idAcademicPerformance = 1,
                idAcademicPerformanceNavCode = "verybad",
                idAcademicPerformanceNavName = "Фиаско"
            };

            Assert.Throws<NullReferenceException>(() => service.Update(updateUnknownStudent));
        }


        [Test]
        public void Update_MaleStudent()
        {
            var updateMaleStudent = new StudentDto()
            {
                id = 1,
                surName = "Иванов",
                firstName = "Иван",
                secondName = "Иванович",
                dob = new DateTime(1999, 5, 5),
                idSex = 2,
                idSexNavCode = "male",
                idSexNavName = "Мужской",
                idAcademicPerformance = 5,
                idAcademicPerformanceNavCode = "excellent",
                idAcademicPerformanceNavName = "Отлично"
            };

            var result = service.Update(updateMaleStudent);

            Assert.AreEqual(updateMaleStudent, result);
        }
        #endregion

        #region Remove
        [Test]
        public void RemoveById_Unknown()
        {
            var id = 10;

            Assert.Throws<NullReferenceException>(() => service.RemoveById(id));
        }

        [Test]
        public void RemoveById_MaleStudent()
        {
            var id = 1;

            Assert.DoesNotThrow(() => service.RemoveById(id));
        }
        #endregion
    }
}
