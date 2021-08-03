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
    public class TestAcademicPerformanceService
    {
        private LocalAcademicPerformanceService service;

        [SetUp]
        public void Setup()
        {
            var mock = new Mock<IUnitOfWork>();

            // ��������� ������ ����������� �� ��������
            mock.Setup(lr => lr.AcademicPerformance).Returns(new StubAcademicPerformanceRepository());
            mock.Setup(lr => lr.Set<AcademicPerformanceDto>()).Returns(new StubAcademicPerformanceRepository());
            mock.Setup(lr => lr.Set<AcademicPerformanceDto, int>()).Returns(new StubAcademicPerformanceRepository());

            service = new LocalAcademicPerformanceService(mock.Object);
        }

        #region Load List
        [Test]
        public void GetAll()
        {
            var score1 = new AcademicPerformanceDto()
            {
                id = 1,
                code = "verybad",
                name = "������",
                description = ""
            };
            var score2 = new AcademicPerformanceDto()
            {
                id = 2,
                code = "bad",
                name = "�������������������",
                description = ""
            };
            var score3 = new AcademicPerformanceDto()
            {
                id = 3,
                code = "satisfying",
                name = "�����������������",
                description = ""
            };
            var score4 = new AcademicPerformanceDto()
            {
                id = 4,
                code = "good",
                name = "������",
                description = ""
            };
            var score5 = new AcademicPerformanceDto()
            {
                id = 5,
                code = "excellent",
                name = "�������",
                description = ""
            };
            var neededList = new List<AcademicPerformanceDto>() { score1, score2, score3, score4, score5 };

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
        public void GetOne_VeryBad()
        {
            var neededVeryBad = new AcademicPerformanceDto()
            {
                id = 1,
                name = "������",
                code = "verybad",
                description = ""
            };

            var resultVeryBad = service.GetOneById(neededVeryBad.id);

            Assert.AreEqual(neededVeryBad, resultVeryBad);
        }

        [Test]
        public void GetOne_Bad()
        {
            var neededBad = new AcademicPerformanceDto()
            {
                id = 2,
                code = "bad",
                name = "�������������������",
                description = ""
            };

            var resultBad = service.GetOneById(neededBad.id);

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
                name = "������",
                code = "verybad",
                description = ""
            };

            Assert.Throws<InvalidOperationException>(() => service.Add(newAcademicPerformance));
        }

        [Test]
        public void Add_Existing()
        {
            var newAcademicPerformance = new AcademicPerformanceDto()
            {
                id = 0,
                name = "������",
                code = "verybad",
                description = ""
            };

            Assert.Throws<NonUniqueException>(() => service.Add(newAcademicPerformance));
        }

        [Test]
        public void Add_New()
        {
            var neededId = 6;
            var newAcademicPerformance = new AcademicPerformanceDto()
            {
                id = 0,
                name = "�������",
                code = "notbad",
                description = ""
            };

            var result = service.Add(newAcademicPerformance);
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
                name = "����������",
                code = "unknown",
                description = "������ �� �����������"
            };

            Assert.Throws<NullReferenceException>(() => service.Update(updateUnknownAcademicPerformance));
        }


        [Test]
        public void Update_VeryBad()
        {
            var updateVeryBad = new AcademicPerformanceDto()
            {
                id = 1,
                code = "verybad",
                name = "����� �����",
                description = "������"
            };

            var result = service.Update(updateVeryBad);

            Assert.AreEqual(updateVeryBad, result);
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
        public void RemoveById_VeryBad()
        {
            var id = 1;

            Assert.DoesNotThrow(() => service.RemoveById(id));
        }
        #endregion
    }
}
