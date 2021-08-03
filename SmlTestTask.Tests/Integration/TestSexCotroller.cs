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
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using SmlTestTask;
using Microsoft.Extensions.Configuration;
using System.IO;
using SmlTestTask.Controllers;
using System.Net.Http;
using DAL.EF.EF.Context;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DAL.EF.EF.Entities;

namespace SmlTestTask.Tests.Integration
{
    [TestFixture]
    public class TestSexController : BaseCotrollerIntegrationTest
    {
        private const string ControllerPath = "Sex";
        protected override void InitTestData()
        {
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
        }

        #region Load List
        [Test]
        public async Task GetAll()
        {
            var female = new SexDto()
            {
                id = 1,
                name = "Женский",
                code = "female",
                description = ""
            };
            var male = new SexDto()
            {
                id = 2,
                name = "Мужской",
                code = "male",
                description = ""
            };
            var neededList = new List<SexDto>() { female, male };

            var response = await client.GetAsync($"/{ControllerPath}");
            Assert.AreEqual(StatusCodes.Status200OK, (int)response.StatusCode);

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var resultList = JsonConvert.DeserializeObject<List<SexDto>>(jsonResponse);

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
            Assert.AreEqual($"{nameof(SexDto)} with id = {id} not found", stringResponse);
        }

        [Test]
        public async Task GetOne_Female()
        {
            var neededFemaleSex = new SexDto()
            {
                id = 1,
                name = "Женский",
                code = "female",
                description = ""
            };

            var response = await client.GetAsync($"/{ControllerPath}/{neededFemaleSex.id}");
            Assert.AreEqual(StatusCodes.Status200OK, (int)response.StatusCode);

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var resultFemaleSex = JsonConvert.DeserializeObject<SexDto>(jsonResponse);

            Assert.AreEqual(neededFemaleSex, resultFemaleSex);
        }

        [Test]
        public async Task GetOne_Male()
        {
            var neededMaleSex = new SexDto()
            {
                id = 2,
                name = "Мужской",
                code = "male",
                description = ""
            };

            var response = await client.GetAsync($"/{ControllerPath}/{neededMaleSex.id}");
            Assert.AreEqual(StatusCodes.Status200OK, (int)response.StatusCode);

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var resultMaleSex = JsonConvert.DeserializeObject<SexDto>(jsonResponse);

            Assert.AreEqual(neededMaleSex, resultMaleSex);
        }
        #endregion

        #region Add
        [Test]
        public async Task Add_WithId()
        {
            var newSex = new SexDto()
            {
                id = 1,
                name = "Женский",
                code = "female",
                description = ""
            };

            var json = JsonConvert.SerializeObject(newSex);

            var response = await client.PostAsync($"/{ControllerPath}", new StringContent(json, Encoding.UTF8, "application/json"));
            Assert.AreEqual(StatusCodes.Status400BadRequest, (int)response.StatusCode);

            var stringResponse = await response.Content.ReadAsStringAsync();
            Assert.AreEqual($"This operation is invalid for provided {nameof(SexDto)}", stringResponse);
        }

        [Test]
        public async Task Add_Existing()
        {
            var newSex = new SexDto()
            {
                id = 0,
                name = "Женский",
                code = "female",
                description = ""
            };

            var json = JsonConvert.SerializeObject(newSex);

            var response = await client.PostAsync($"/{ControllerPath}", new StringContent(json, Encoding.UTF8, "application/json"));
            Assert.AreEqual(StatusCodes.Status409Conflict, (int)response.StatusCode);

            var stringResponse = await response.Content.ReadAsStringAsync();
            Assert.AreEqual($"{nameof(SexDto)} with same fields are already exists", stringResponse);
        }

        [Test]
        public async Task Add_New()
        {
            var neededId = 3;
            var newSex = new SexDto()
            {
                id = 0,
                name = "Не определился",
                code = "undef",
                description = "Только ради фейсбука"
            };

            var json = JsonConvert.SerializeObject(newSex);
            newSex.id = neededId;

            var response = await client.PostAsync($"/{ControllerPath}", new StringContent(json, Encoding.UTF8, "application/json"));
            Assert.AreEqual(StatusCodes.Status200OK, (int)response.StatusCode);

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var resultSex = JsonConvert.DeserializeObject<SexDto>(jsonResponse);

            Assert.AreEqual(newSex, resultSex);
        }
        #endregion

        #region Update
        [Test]
        public async Task Update_Unknown()
        {
            var updateUnknownSex = new SexDto()
            {
                id = 10,
                name = "Неизвестно",
                code = "unknown",
                description = "Пол не установлен"
            };
            var json = JsonConvert.SerializeObject(updateUnknownSex);

            var response = await client.PutAsync($"/{ControllerPath}", new StringContent(json, Encoding.UTF8, "application/json"));
            Assert.AreEqual(StatusCodes.Status404NotFound, (int)response.StatusCode);

            var stringResponse = await response.Content.ReadAsStringAsync();
            Assert.AreEqual($"{nameof(SexDto)} with id = {updateUnknownSex.id} not found", stringResponse);
        }


        [Test]
        public async Task Update_Female()
        {
            var updateFemaleSex = new SexDto()
            {
                id = 1,
                name = "Женский 2",
                code = "female",
                description = "Описание женского пола"
            };

            var json = JsonConvert.SerializeObject(updateFemaleSex);

            var response = await client.PutAsync($"/{ControllerPath}", new StringContent(json, Encoding.UTF8, "application/json"));
            Assert.AreEqual(StatusCodes.Status200OK, (int)response.StatusCode);

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var resultSex = JsonConvert.DeserializeObject<SexDto>(jsonResponse);

            Assert.AreEqual(updateFemaleSex, resultSex);
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
            Assert.AreEqual($"{nameof(SexDto)} with id = {id} not found", stringResponse);
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
