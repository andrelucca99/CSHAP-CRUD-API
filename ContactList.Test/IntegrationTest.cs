namespace ContactList.Test;

using Moq;
using Xunit;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

// importações das camadas Model, Service e Controller da API
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
            {
                services.Remove(descriptor);
            }
            services.AddSingleton(mockService.Object);
        });
    }).CreateClient();
    }

    [Theory(DisplayName = "Testando a rota /GET Person")]
    [InlineData("/person")]
    public async Task TestGetPersonList(string url)
    {
        // Arrange

        Person[] personMock = new Person[2];
        personMock[0] = new Person { PersonId = 1, PersonName = "Roberta Miranda", PersonEmail = "roberta@email.com", PersonPhone = "5511999999999" };
        personMock[1] = new Person { PersonId = 2, PersonName = "José Roberto", PersonEmail = "ze@email.com", PersonPhone = "5511888888888" };
        mockService.Setup(s => s.getPersonList()).Returns(personMock);

        // Act

        var response = await _clientTest.GetAsync(url);
        var responseString = await response.Content.ReadAsStringAsync();
        Person[] jsonResponse = JsonConvert.DeserializeObject<Person[]>(responseString)!;

        // Assert

        Assert.Equal(System.Net.HttpStatusCode.OK, response?.StatusCode);
        Assert.Equal(2, jsonResponse.Count()!);
        Assert.Equal(personMock[0].PersonId, jsonResponse[0].PersonId);
        Assert.Equal(personMock[0].PersonName, jsonResponse[0].PersonName);
        Assert.Equal(personMock[0].PersonEmail, jsonResponse[0].PersonEmail);
        Assert.Equal(personMock[0].PersonPhone, jsonResponse[0].PersonPhone);
    }
}