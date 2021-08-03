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
    public class TestAcademicPerformanceController
    {
        private AcademicPerformanceController Controller;

        [SetUp]
        public void Setup()
        {
            var mock = new Mock<IComplexProvider>();

            // Подменяем сервис заглушкой
            mock.Setup(ls => ls.AcademicPerformance).Returns(new StubAcademicPerformanceService());
            mock.Setup(ls => ls.Set<AcademicPerformanceDto>()).Returns(new StubAcademicPerformanceService());
            mock.Setup(ls => ls.Set<AcademicPerformanceDto, int>()).Returns(new StubAcademicPerformanceService());

            Controller = new AcademicPerformanceController(mock.Object);
        }

        #region Load List
        [Test]
        public void GetAll()
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

            var resultList = (IEnumerable<AcademicPerformanceDto>)Controller.Get();

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
            Assert.AreEqual($"{nameof(AcademicPerformanceDto)} with id = {id} not found", result.Value.ToString());
        }

        [Test]
        public void GetOne_VeryBad()
        {
            var neededVeryBad = new AcademicPerformanceDto()
            {
                id = 1,
                code = "verybad",
                name = "Фиаско",
                description = ""
            };

            var resultVeryBad = (AcademicPerformanceDto)Controller.Get(neededVeryBad.id);

            Assert.AreEqual(neededVeryBad, resultVeryBad);
        }

        [Test]
        public void GetOne_Bad()
        {
            var neededBad = new AcademicPerformanceDto()
            {
                id = 2,
                code = "bad",
                name = "Неудовлетворительно",
                description = ""
            };

            var resultBad = Controller.Get(neededBad.id);

            Assert.AreEqual(neededBad, resultBad);
        }
        #endregion

        #region Add
        [Test]
        public void Add_WithId()
        {
            var newAcademicPerformance = new AcademicPerformanceDto()
            {
                id = 1,
                code = "verybad",
                name = "Фиаско",
                description = ""
            };

            var result = (ObjectResult)Controller.Post(newAcademicPerformance);
            
            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.AreEqual($"This operation is invalid for provided {nameof(AcademicPerformanceDto)}", result.Value.ToString());
        }

        [Test]
        public void Add_Existing()
        {
            var newAcademicPerformance = new AcademicPerformanceDto()
            {
                id = 0,
                code = "verybad",
                name = "Фиаско",
                description = ""
            };
            
            var result = (ObjectResult)Controller.Post(newAcademicPerformance);

            Assert.AreEqual(StatusCodes.Status409Conflict, result.StatusCode);
            Assert.AreEqual($"{nameof(AcademicPerformanceDto)} with same fields are already exists", result.Value.ToString());
        }

        [Test]
        public void Add_New()
        {
            var neededId = 6;
            var newAcademicPerformance = new AcademicPerformanceDto()
            {
                id = 0,
                name = "Неплохо",
                code = "notbad",
                description = ""
            };

            var result = (AcademicPerformanceDto)Controller.Post(newAcademicPerformance);

            newAcademicPerformance.id = neededId;
            Assert.AreEqual(newAcademicPerformance, result);
        }
        #endregion

        #region Update
        [Test]
        public void Update_Unknown()
        {
            var updateUnknownAcademicPerformance = new AcademicPerformanceDto()
            {
                id = 10,
                name = "Неизвестно",
                code = "unknown",
                description = "Оценка не установлена"
            };

            var result = (ObjectResult)Controller.Put(updateUnknownAcademicPerformance);

            Assert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
            Assert.AreEqual($"{nameof(AcademicPerformanceDto)} with id = {updateUnknownAcademicPerformance.id} not found", result.Value.ToString());
        }


        [Test]
        public void Update_VeryBad()
        {
            var updateFemaleAcademicPerformance = new AcademicPerformanceDto()
            {
                id = 1,
                code = "verybad",
                name = "Очень плохо",
                description = "ужасно"
            };

            var result = (AcademicPerformanceDto)Controller.Put(updateFemaleAcademicPerformance);

            Assert.AreEqual(updateFemaleAcademicPerformance, result);
        }
        #endregion

        #region Remove
        [Test]
        public void RemoveById_Unknown()
        {
            var id = 10;

            var result = (ObjectResult)Controller.Delete(id);

            Assert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
            Assert.AreEqual($"{nameof(AcademicPerformanceDto)} with id = {id} not found", result.Value.ToString());
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
