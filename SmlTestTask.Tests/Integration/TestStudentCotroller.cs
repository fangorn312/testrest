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
using DAL.EF.EF.Entities;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;

namespace SmlTestTask.Tests.Integration
{
    [TestFixture]
    public class TestStudentController : BaseCotrollerIntegrationTest
    {
        private const string ControllerPath = "Student";
        protected override void InitTestData() {
            var score1 = new AcademicPerformance()
            {
                id = 1,
                code = "verybad",
                name = "Фиаско",
                description = ""
            };
            var score2 = new AcademicPerformance()
            {
                id = 2,
                code = "bad",
                name = "Неудовлетворительно",
                description = ""
            };
            var score3 = new AcademicPerformance()
            {
                id = 3,
                code = "satisfying",
                name = "Удовлетворительно",
                description = ""
            };
            var score4 = new AcademicPerformance()
            {
                id = 4,
                code = "good",
                name = "Хорошо",
                description = ""
            };
            var score5 = new AcademicPerformance()
            {
                id = 5,
                code = "excellent",
                name = "Отлично",
                description = ""
            };

            context.Add(score1);
            context.Add(score2);
            context.Add(score3);
            context.Add(score4);
            context.Add(score5);
            context.SaveChanges();

            var female = new Sex()
            {
                id = 0,
                name = "Женский",
                code = "female",
                description = ""
            };
            var male = new Sex()
            {
                id = 0,
                name = "Мужской",
                code = "male",
                description = ""
            };
            context.Add(female);
            context.Add(male);
            context.SaveChanges();

            var studentMale = new Student()
            {
                id = 1,
                surName = "Иванов",
                firstName = "Иван",
                secondName = "Иванович",
                dob = new DateTime(2000, 1, 1),
                idSex = 2,
                idAcademicPerformance = 1
            };
            var studentFemale = new Student()
            {
                id = 2,
                surName = "Александрова",
                firstName = "Александра",
                secondName = "Александровна",
                dob = new DateTime(2002, 2, 2),
                idSex = 1,
                idAcademicPerformance = 2
            };
            context.Add(studentMale);
            context.Add(studentFemale);
            context.SaveChanges();
        }

        #region Load List
        [Test]
        public async Task GetAll()
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

            var response = await client.GetAsync($"/{ControllerPath}");
            Assert.AreEqual(StatusCodes.Status200OK, (int)response.StatusCode);

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var resultList = JsonConvert.DeserializeObject<List<StudentDto>>(jsonResponse);

            Assert.IsTrue(neededList.SequenceEqual(resultList));
        }
        #endregion

        #region Load One Item
        [Test]
        public async Task GetOne_Unknown()
        {
            var id = 10;

            var response = await client.GetAsync($"/{ControllerPath}/{id}");
            Assert.AreEqual(StatusCodes.Status404NotFound, (int)response.StatusCode);

            var stringResponse = await response.Content.ReadAsStringAsync();
            Assert.AreEqual($"{nameof(StudentDto)} with id = {id} not found", stringResponse);
        }

        [Test]
        public async Task GetOne_StudentFemale()
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

            var response = await client.GetAsync($"/{ControllerPath}/{neededFemaleStudent.id}");
            Assert.AreEqual(StatusCodes.Status200OK, (int)response.StatusCode);

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var resultFemaleStudent = JsonConvert.DeserializeObject<StudentDto>(jsonResponse);

            Assert.AreEqual(neededFemaleStudent, resultFemaleStudent);
        }

        [Test]
        public async Task GetOne_Male()
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

            var response = await client.GetAsync($"/{ControllerPath}/{neededMaleStudent.id}");
            Assert.AreEqual(StatusCodes.Status200OK, (int)response.StatusCode);

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var resultMaleStudent = JsonConvert.DeserializeObject<StudentDto>(jsonResponse);

            Assert.AreEqual(neededMaleStudent, resultMaleStudent);
        }
        #endregion

        #region Add
        [Test]
        public async Task Add_WithId()
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

            var json = JsonConvert.SerializeObject(newStudent);

            var response = await client.PostAsync($"/{ControllerPath}", new StringContent(json, Encoding.UTF8, "application/json"));
            Assert.AreEqual(StatusCodes.Status400BadRequest, (int)response.StatusCode);

            var stringResponse = await response.Content.ReadAsStringAsync();
            Assert.AreEqual($"This operation is invalid for provided {nameof(StudentDto)}", stringResponse);
        }

        [Test]
        public async Task Add_Existing()
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

            var json = JsonConvert.SerializeObject(newStudent);

            var response = await client.PostAsync($"/{ControllerPath}", new StringContent(json, Encoding.UTF8, "application/json"));
            Assert.AreEqual(StatusCodes.Status409Conflict, (int)response.StatusCode);

            var stringResponse = await response.Content.ReadAsStringAsync();
            Assert.AreEqual($"{nameof(StudentDto)} with same fields are already exists", stringResponse);
        }

        [Test]
        public async Task Add_New()
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

            var json = JsonConvert.SerializeObject(newStudent);
            newStudent.id = neededId;

            var response = await client.PostAsync($"/{ControllerPath}", new StringContent(json, Encoding.UTF8, "application/json"));
            Assert.AreEqual(StatusCodes.Status200OK, (int)response.StatusCode);

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<StudentDto>(jsonResponse);

            Assert.AreEqual(newStudent, result);
        }
        #endregion

        #region Update
        [Test]
        public async Task Update_Unknown()
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

            var json = JsonConvert.SerializeObject(updateUnknownStudent);

            var response = await client.PutAsync($"/{ControllerPath}", new StringContent(json, Encoding.UTF8, "application/json"));
            Assert.AreEqual(StatusCodes.Status404NotFound, (int)response.StatusCode);

            var stringResponse = await response.Content.ReadAsStringAsync();
            Assert.AreEqual($"{nameof(StudentDto)} with id = {updateUnknownStudent.id} not found", stringResponse);
        }


        [Test]
        public async Task Update_MaleStudent()
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

            var json = JsonConvert.SerializeObject(updateMaleStudent);

            var response = await client.PutAsync($"/{ControllerPath}", new StringContent(json, Encoding.UTF8, "application/json"));
            Assert.AreEqual(StatusCodes.Status200OK, (int)response.StatusCode);

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<StudentDto>(jsonResponse);

            Assert.AreEqual(updateMaleStudent, result);
        }
        #endregion

        #region Remove
        [Test]
        public async Task RemoveById_Unknown()
        {
            var id = 10;

            var response = await client.DeleteAsync($"/{ControllerPath}/{id}");
            Assert.AreEqual(StatusCodes.Status404NotFound, (int)response.StatusCode);

            var stringResponse = await response.Content.ReadAsStringAsync();
            Assert.AreEqual($"{nameof(StudentDto)} with id = {id} not found", stringResponse);
        }

        [Test]
        public async Task RemoveById_Female()
        {
            var id = 1;

            var response = await client.DeleteAsync($"/{ControllerPath}/{id}");
            Assert.AreEqual(StatusCodes.Status200OK, (int)response.StatusCode);

            var stringResponse = await response.Content.ReadAsStringAsync();
            Assert.AreEqual("", stringResponse);
        }
        #endregion
    }
}
