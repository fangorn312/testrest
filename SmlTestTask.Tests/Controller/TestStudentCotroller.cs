using BLL.Interface.Dto;
using BLL.Interface.Exception;
using BLL.Interface.Interface;
using DAL.Interface;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using BLL.Interface;
using BLL.Local.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmlTestTask.Controllers;

namespace BLL.Local.Controllers
{
    [TestFixture]
    public class TestStudentController
    {
        private StudentController Controller;

        [SetUp]
        public void Setup()
        {
            var mock = new Mock<IComplexProvider>();

            // Подменяем сервис заглушкой
            mock.Setup(ls => ls.Student).Returns(new StubStudentService());
            mock.Setup(ls => ls.Set<StudentDto>()).Returns(new StubStudentService());
            mock.Setup(ls => ls.Set<StudentDto, int>()).Returns(new StubStudentService());

            Controller = new StudentController(mock.Object);
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

            var resultList = (IEnumerable<StudentDto>)Controller.Get();

            Assert.IsTrue(neededList.SequenceEqual(resultList));
        }
        #endregion

        #region Load One Item
        [Test]
        public void GetOne_Unknown()
        {
            var id = 10;

            var result = (ObjectResult)Controller.Get(id);

            Assert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
            Assert.AreEqual($"{nameof(StudentDto)} with id = {id} not found", result.Value.ToString());
        }

        [Test]
        public void GetOne_StudentFemale()
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

            var resultFemaleStudent = (StudentDto)Controller.Get(neededFemaleStudent.id);

            Assert.AreEqual(neededFemaleStudent, resultFemaleStudent);
        }

        [Test]
        public void GetOne_Male()
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

            var resultMaleStudent = Controller.Get(neededMaleStudent.id);

            Assert.AreEqual(neededMaleStudent, resultMaleStudent);
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

            var result = (ObjectResult)Controller.Post(newStudent);
            
            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.AreEqual($"This operation is invalid for provided {nameof(StudentDto)}", result.Value.ToString());
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
            
            var result = (ObjectResult)Controller.Post(newStudent);

            Assert.AreEqual(StatusCodes.Status409Conflict, result.StatusCode);
            Assert.AreEqual($"{nameof(StudentDto)} with same fields are already exists", result.Value.ToString());
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

            var result = (StudentDto)Controller.Post(newStudent);

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

            var result = (ObjectResult)Controller.Put(updateUnknownStudent);

            Assert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
            Assert.AreEqual($"{nameof(StudentDto)} with id = {updateUnknownStudent.id} not found", result.Value.ToString());
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

            var result = (StudentDto)Controller.Put(updateMaleStudent);

            Assert.AreEqual(updateMaleStudent, result);
        }
        #endregion

        #region Remove
        [Test]
        public void RemoveById_Unknown()
        {
            var id = 10;

            var result = (ObjectResult)Controller.Delete(id);

            Assert.AreEqual(result.StatusCode, StatusCodes.Status404NotFound);
            Assert.AreEqual($"{nameof(StudentDto)} with id = {id} not found", result.Value.ToString());
        }

        [Test]
        public void RemoveById_Female()
        {
            var id = 1;

            Assert.DoesNotThrow(() => Controller.Delete(id));
        }
        #endregion
    }
}
