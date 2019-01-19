using Lazztech.Events.Dto.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Lazztech.Cloud.ClientFacade.Tests.IntegrationTests
{
    //[Category("Integration Tests")]
    //public class StartupResolveDIServicesTests
    //    : IClassFixture<WebApplicationFactory<Startup>>
    //{
    //    private readonly WebApplicationFactory<Startup> _factory;

    //    public StartupResolveDIServicesTests(WebApplicationFactory<Startup> factory)
    //    {
    //        _factory = factory;
    //    }

    //    [Fact]
    //    public void ResolveISmsService()
    //    {
    //        //Arrange
    //        //var client = _factory.CreateDefaultClient();
    //        //var server = (ISmsService)_factory.Server.Host.Services.GetService(typeof(ISmsService));

    //        var testServer = new TestServer(new WebHostBuilder()
    //.UseStartup<Startup>()
    //.UseEnvironment("Development"));

    //        var myService = testServer.Host.Services.GetRequiredService<ISmsService>();
    //        myService.SendSms("4254434290", "This is a test");

    //        //Act

    //        //Assert
    //    }
    //}
}
