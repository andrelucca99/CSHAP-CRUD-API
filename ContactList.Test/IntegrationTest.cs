namespace ContactList.Test;

using Moq;
using Xunit;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

using ContactList.Models;
using ContactList.Services;
using ContactList.Controllers;

public class IntegrationTest : IClassFixture<WebApplicationFactory<Program>>
{
    public HttpClient _clientTest;
    public Mock<IContactService> mockService;

    public IntegrationTest(WebApplicationFactory<Program> factory)
    {
        mockService = new Mock<IContactService>();

        _clientTest = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IContactService));
                if (descriptor != null)
                    services.Remove(descriptor)
                services.AddSingleton(mockService.Object);
            });
        }).CreateClient();
    }
}