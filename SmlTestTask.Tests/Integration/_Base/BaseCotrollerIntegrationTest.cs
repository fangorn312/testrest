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
    public abstract class BaseCotrollerIntegrationTest
    {
        protected HttpClient client;
        protected TestRestContext context;
        [SetUp]
        public void Setup()
        {
            var testId = Guid.NewGuid().ToString();
            var configuration = new ConfigurationBuilder()
                          .SetBasePath(Path.GetFullPath(@"../../"))
                          .Build();


            var builder = new WebHostBuilder()
                      .UseEnvironment($"Test-id-{testId}")
                      .UseStartup<Startup>()
                      .UseConfiguration(configuration);

            var server = new TestServer(builder);

            client = server.CreateClient();

            var dbOptions = new DbContextOptionsBuilder<TestRestContext>()
                .UseInMemoryDatabase(databaseName: "TestDb" + testId)
                .Options;

            context = new TestRestContext(dbOptions);
            InitTestData();
        }

        protected abstract void InitTestData();
    }
}
