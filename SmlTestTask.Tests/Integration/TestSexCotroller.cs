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

namespace BLL.Local.Controllers2
{
    [TestFixture]
    public class TestSexController
    {
        private SexController Controller;
        private HttpClient client;
        private TestRestContext context;

        [SetUp]
        public void Setup()
        {
            var configuration = new ConfigurationBuilder()
                          .SetBasePath(Path.GetFullPath(@"../../"))
                          .Build();


            var builder = new WebHostBuilder()
                      .UseEnvironment("Test")
                      .UseStartup<Startup>()
                      .UseConfiguration(configuration);

            var server = new TestServer(builder);

            client = server.CreateClient();

            var dbOptions = new DbContextOptionsBuilder<TestRestContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            context = new TestRestContext(dbOptions);
            InitTestData();
        }

        private void InitTestData()
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

            var response = await client.GetAsync($"/Sex");
            Assert.AreEqual(StatusCodes.Status200OK, (int)response.StatusCode);

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var resultList = JsonConvert.DeserializeObject<List<SexDto>>(jsonResponse);

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
            Assert.AreEqual($"{nameof(SexDto)} with id = {id} not found", result.Value.ToString());
        }

        [Test]
        public void GetOne_Female()
        {
            var neededFemaleSex = new SexDto()
            {
                id = 1,
                name = "Женский",
                code = "female",
                description = ""
            };

            var resultFemaleSex = (SexDto)Controller.Get(neededFemaleSex.id);

            Assert.AreEqual(neededFemaleSex, resultFemaleSex);
        }

        [Test]
        public void GetOne_Male()
        {
            var neededMaleSex = new SexDto()
            {
                id = 2,
                name = "Мужской",
                code = "male",
                description = ""
            };

            var resultMaleSex = Controller.Get(neededMaleSex.id);

            Assert.AreEqual(neededMaleSex, resultMaleSex);
        }
        #endregion

        #region Add
        [Test]
        public void Add_WithId()
        {
            var newSex = new SexDto()
            {
                id = 1,
                name = "Женский",
                code = "female",
                description = ""
            };

            var result = (ObjectResult)Controller.Post(newSex);
            
            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.AreEqual($"This operation is invalid for provided {nameof(SexDto)}", result.Value.ToString());
        }

        [Test]
        public void Add_Existing()
        {
            var newSex = new SexDto()
            {
                id = 0,
                name = "Женский",
                code = "female",
                description = ""
            };
            
            var result = (ObjectResult)Controller.Post(newSex);

            Assert.AreEqual(StatusCodes.Status409Conflict, result.StatusCode);
            Assert.AreEqual($"{nameof(SexDto)} with same fields are already exists", result.Value.ToString());
        }

        [Test]
        public void Add_New()
        {
            var neededId = 3;
            var newSex = new SexDto()
            {
                id = 0,
                name = "Не определился",
                code = "undef",
                description = "Только ради фейсбука"
            };

            var result = (SexDto)Controller.Post(newSex);

            newSex.id = neededId;
            Assert.AreEqual(newSex, result);
        }
        #endregion

        #region Update
        [Test]
        public void Update_Unknown()
        {
            var updateUnknownSex = new SexDto()
            {
                id = 10,
                name = "Неизвестно",
                code = "unknown",
                description = "Пол не установлен"
            };

            var result = (ObjectResult)Controller.Put(updateUnknownSex);

            Assert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
            Assert.AreEqual($"{nameof(SexDto)} with id = {updateUnknownSex.id} not found", result.Value.ToString());
        }


        [Test]
        public void Update_Female()
        {
            var updateFemaleSex = new SexDto()
            {
                id = 1,
                name = "Женский 2",
                code = "female",
                description = "Описание женского пола"
            };

            var result = (SexDto)Controller.Put(updateFemaleSex);

            Assert.AreEqual(updateFemaleSex, result);
        }
        #endregion

        #region Remove
        [Test]
        public void RemoveById_Unknown()
        {
            var id = 10;

            var result = (ObjectResult)Controller.Delete(id);

            Assert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
            Assert.AreEqual($"{nameof(SexDto)} with id = {id} not found", result.Value.ToString());
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
