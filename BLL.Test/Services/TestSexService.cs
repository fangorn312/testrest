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
    public class TestSexService
    {
        private LocalSexService service;
        
        [SetUp]
        public void Setup()
        {
            var mock = new Mock<IUnitOfWork>();

            // Подменяем вызовы репозитория на заглушки
            mock.Setup(lr => lr.Sex).Returns(new StubSexRepository());
            mock.Setup(lr => lr.Set<SexDto>()).Returns(new StubSexRepository());
            mock.Setup(lr => lr.Set<SexDto, int>()).Returns(new StubSexRepository());

            service = new LocalSexService(mock.Object);
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
        public void GetOne_Female()
        {
            var neededFemaleSex = new SexDto()
            {
                id = 1,
                name = "Женский",
                code = "female",
                description = ""
            };

            var resultFemaleSex = service.GetOneById(neededFemaleSex.id);

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

            var resultMaleSex = service.GetOneById(neededMaleSex.id);

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

            Assert.Throws<InvalidOperationException>(() => service.Add(newSex));
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

            Assert.Throws<NonUniqueException>(() => service.Add(newSex));
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

            var result = service.Add(newSex);

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

            Assert.Throws<NullReferenceException>(() => service.Update(updateUnknownSex));
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

            var result = service.Update(updateFemaleSex);

            Assert.AreEqual(updateFemaleSex, result);
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
        public void RemoveById_Female()
        {
            var id = 1;

            Assert.DoesNotThrow(() => service.RemoveById(id));
        }
        #endregion
    }
}
