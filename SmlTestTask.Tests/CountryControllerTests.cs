using BLL.Interface;
using BLL.Interface.Dto;
using BLL.Interface.Interface;
using BLL.Local;
using Moq;
using NUnit.Framework;
using PL.API.Open.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace ReactApplication.Tests
{
    [TestFixture]
    public class CountryControllerTests
    {
        private CountryController controller = null;

        [SetUp]
        public void Setup()
        {
            var mock = new Mock<IComplexProvider>();

            mock.Setup(lp => lp.Country).Returns(new MockCountryService());
            mock.Setup(lp => lp.Set<CountryDto>()).Returns(new MockCountryService());
            mock.Setup(lp => lp.Set<CountryDto, int>()).Returns(new MockCountryService());

            controller = new CountryController(mock.Object);
        }

        [Test]
        public void Add_NewCountry_LessMinLengthName()
        {
            var country = CreateCountry(0, "te", "", "");

            var obj = controller.AddOrUpdate(country);

            Assert.AreEqual(0, obj.result.amount);
            Assert.AreEqual(1, obj.meta.errors.Count);
            Assert.AreEqual("Validation", obj.meta.errors[0].type);
            Assert.AreEqual("Country Name shuold be at least 3 characters", obj.meta.errors[0].message);

        }
        
        [Test]
        public void Add_NewCountry_MinLengthName()
        {
            var country = CreateCountry(0, "test", "", "");

            var obj = controller.AddOrUpdate(country);

            Assert.AreEqual(1, obj.result.amount);
            Assert.AreNotEqual(0, obj.result.items[0].id);

        }


        [Test]
        public void GetOne_WithId_1_Kyrgyzstan()
        {
            var obj = controller.GetOneById(1);

            Assert.AreEqual(1, obj.result.amount);
            Assert.AreEqual(1, obj.result.items[0].id);
            Assert.AreEqual("Кыргызстан", obj.result.items[0].name);
            Assert.AreEqual("", obj.result.items[0].code);
            Assert.AreEqual("", obj.result.items[0].description);
        }

        [Test]
        public void GetOne_WithId_0_Unknown()
        {
            var obj = controller.GetOneById(0);

            Assert.AreEqual(obj.result.amount, 0);
            Assert.AreEqual(obj.meta.code, (int)HttpStatusCode.NotFound);
            Assert.AreEqual(obj.meta.errors.Count, 1);
            Assert.AreEqual(obj.meta.errors[0].type, "HZ, nado podumat");
            Assert.AreEqual(obj.meta.errors[0].message, "Country not found");
        }

        private CountryDto CreateCountry(int id, string name, string code, string description)
        {
            var country = new CountryDto();
            country.id = id;
            country.name = name;
            country.description = code;
            country.code = description;
            return country;
        }
    }

    public class MockCountryService : ICountryService
    {
        private List<CountryDto> countries = new List<CountryDto>()
        {
            new CountryDto()
                {
                    id = 1,
                    name = "Кыргызстан",
                    description = "",
                    code = "",
                },
            new CountryDto()
                {
                    id = 2,
                    name = "Узбекистан",
                    description = "Соседи",
                    code = "",
                },
        };

        public bool IsInited => true;

        public CountryDto AddOrUpdate(CountryDto item)
        {
            countries.Add(item);
            item.id = countries.Count;
            return item;
        }

        public CountryDto GetOneById(int id)
        {
            return countries.FirstOrDefault(x => x.id == id);
        }

        public List<CountryDto> Items()
        {
            return countries;
        }

        public void RemoveById(int id)
        {
            throw new NotImplementedException();
        }

        public void SetSession(string sessionKey)
        {
            throw new NotImplementedException();
        }
    }
}
