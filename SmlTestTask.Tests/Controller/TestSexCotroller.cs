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
    public class TestSexController
    {
        private SexController Controller;

        [SetUp]
        public void Setup()
        {
            var mock = new Mock<IComplexProvider>();

            // Подменяем сервис заглушкой
            mock.Setup(ls => ls.Sex).Returns(new StubSexService());
            mock.Setup(ls => ls.Set<SexDto>()).Returns(new StubSexService());
            mock.Setup(ls => ls.Set<SexDto, int>()).Returns(new StubSexService());

            Controller = new SexController(mock.Object);
        }

        #region Load List
        [Test]
        public void GetAll()
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

            var resultList = (IEnumerable<SexDto>)Controller.Get();

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
