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
using SmlTestTask.Tests.Integration;
using DAL.EF.EF.Entities;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;

namespace SmlTestTask.Tests.Integration
{
    [TestFixture]
    public class TestAcademicPerformanceController: BaseCotrollerIntegrationTest
    {
        private const string ControllerPath = "AcademicPerformance";
        protected override void InitTestData()
        {
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
        }

        #region Load List
        [Test]
        public async Task GetAll()
        {
            var score1 = new AcademicPerformanceDto()
            {
                id = 1,
                code = "verybad",
                name = "Фиаско",
                description = ""
            };
            var score2 = new AcademicPerformanceDto()
            {
                id = 2,
                code = "bad",
                name = "Неудовлетворительно",
                description = ""
            };
            var score3 = new AcademicPerformanceDto()
            {
                id = 3,
                code = "satisfying",
                name = "Удовлетворительно",
                description = ""
            };
            var score4 = new AcademicPerformanceDto()
            {
                id = 4,
                code = "good",
                name = "Хорошо",
                description = ""
            };
            var score5 = new AcademicPerformanceDto()
            {
                id = 5,
                code = "excellent",
                name = "Отлично",
                description = ""
            };
            var neededList = new List<AcademicPerformanceDto>() { score1, score2, score3, score4, score5 };

            var response = await client.GetAsync($"/{ControllerPath}");
            Assert.AreEqual(StatusCodes.Status200OK, (int)response.StatusCode);

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var resultList = JsonConvert.DeserializeObject<List<AcademicPerformanceDto>>(jsonResponse);

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
            Assert.AreEqual($"{nameof(AcademicPerformanceDto)} with id = {id} not found", stringResponse);
        }

        [Test]
        public async Task GetOne_VeryBad()
        {
            var neededVeryBad = new AcademicPerformanceDto()
            {
                id = 1,
                code = "verybad",
                name = "Фиаско",
                description = ""
            };

            var response = await client.GetAsync($"/{ControllerPath}/{neededVeryBad.id}");
            Assert.AreEqual(StatusCodes.Status200OK, (int)response.StatusCode);

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var resultVeryBad = JsonConvert.DeserializeObject<AcademicPerformanceDto>(jsonResponse);

            Assert.AreEqual(neededVeryBad, resultVeryBad);
        }

        [Test]
        public async Task GetOne_Bad()
        {
            var neededBad = new AcademicPerformanceDto()
            {
                id = 2,
                code = "bad",
                name = "Неудовлетворительно",
                description = ""
            };

            var response = await client.GetAsync($"/{ControllerPath}/{neededBad.id}");
            Assert.AreEqual(StatusCodes.Status200OK, (int)response.StatusCode);

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var resultBad = JsonConvert.DeserializeObject<AcademicPerformanceDto>(jsonResponse);

            Assert.AreEqual(neededBad, resultBad);
        }
        #endregion

        #region Add
        [Test]
        public async Task Add_WithId()
        {
            var newAcademicPerformance = new AcademicPerformanceDto()
            {
                id = 1,
                code = "verybad",
                name = "Фиаско",
                description = ""
            };

            var json = JsonConvert.SerializeObject(newAcademicPerformance);

            var response = await client.PostAsync($"/{ControllerPath}", new StringContent(json, Encoding.UTF8, "application/json"));
            Assert.AreEqual(StatusCodes.Status400BadRequest, (int)response.StatusCode);

            var stringResponse = await response.Content.ReadAsStringAsync();
            Assert.AreEqual($"This operation is invalid for provided {nameof(AcademicPerformanceDto)}", stringResponse);

        }

        [Test]
        public async Task Add_Existing()
        {
            var newAcademicPerformance = new AcademicPerformanceDto()
            {
                id = 0,
                code = "verybad",
                name = "Фиаско",
                description = ""
            };

            var json = JsonConvert.SerializeObject(newAcademicPerformance);

            var response = await client.PostAsync($"/{ControllerPath}", new StringContent(json, Encoding.UTF8, "application/json"));
            Assert.AreEqual(StatusCodes.Status409Conflict, (int)response.StatusCode);

            var stringResponse = await response.Content.ReadAsStringAsync();
            Assert.AreEqual($"{nameof(AcademicPerformanceDto)} with same fields are already exists", stringResponse);
        }

        [Test]
        public async Task Add_New()
        {
            var neededId = 6;
            var newAcademicPerformance = new AcademicPerformanceDto()
            {
                id = 0,
                name = "Неплохо",
                code = "notbad",
                description = ""
            };

            var json = JsonConvert.SerializeObject(newAcademicPerformance);
            newAcademicPerformance.id = neededId;

            var response = await client.PostAsync($"/{ControllerPath}", new StringContent(json, Encoding.UTF8, "application/json"));
            Assert.AreEqual(StatusCodes.Status200OK, (int)response.StatusCode);

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<AcademicPerformanceDto>(jsonResponse);

            Assert.AreEqual(newAcademicPerformance, result);

        }
        #endregion

        #region Update
        [Test]
        public async Task Update_Unknown()
        {
            var updateUnknownAcademicPerformance = new AcademicPerformanceDto()
            {
                id = 10,
                name = "Неизвестно",
                code = "unknown",
                description = "Оценка не установлена"
            };

            var json = JsonConvert.SerializeObject(updateUnknownAcademicPerformance);

            var response = await client.PutAsync($"/{ControllerPath}", new StringContent(json, Encoding.UTF8, "application/json"));
            Assert.AreEqual(StatusCodes.Status404NotFound, (int)response.StatusCode);

            var stringResponse = await response.Content.ReadAsStringAsync();
            Assert.AreEqual($"{nameof(AcademicPerformanceDto)} with id = {updateUnknownAcademicPerformance.id} not found", stringResponse);            
        }


        [Test]
        public async Task Update_VeryBad()
        {
            var updateFemaleAcademicPerformance = new AcademicPerformanceDto()
            {
                id = 1,
                code = "verybad",
                name = "Очень плохо",
                description = "ужасно"
            };


            var json = JsonConvert.SerializeObject(updateFemaleAcademicPerformance);

            var response = await client.PutAsync($"/{ControllerPath}", new StringContent(json, Encoding.UTF8, "application/json"));
            Assert.AreEqual(StatusCodes.Status200OK, (int)response.StatusCode);

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<AcademicPerformanceDto>(jsonResponse);

            Assert.AreEqual(updateFemaleAcademicPerformance, result);
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
            Assert.AreEqual($"{nameof(AcademicPerformanceDto)} with id = {id} not found", stringResponse);
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
