using InterviewAssignment.Infrastructure.InterviewApi;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace InterviewAssignment.Unit.Tests.Infrastructure.InterviewApi
{
    public class InterviewApiServiceTests
    {
        [Fact]
        public void ConfigureInterviewApi_ShouldRegisterHttpClientWithBaseAddress()
        {
            // Arrange
            var services = new ServiceCollection();

            // Mock configuration for InterviewApiOptions
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new[]
                {
                    new KeyValuePair<string, string>("InterviewApi:BaseUrl", "https://api.dummy.com/"),
                    new KeyValuePair<string, string>("InterviewApi:SubmitTask", "submit"),
                    new KeyValuePair<string, string>("InterviewApi:GetTask", "get")
                })
                .Build();

            services.AddSingleton<IConfiguration>(config);

            // Act
            services.ConfigureInterviewApi();
            var serviceProvider = services.BuildServiceProvider();

            // Assert that the InterviewApiOptions are configured correctly
            var options = serviceProvider.GetRequiredService<IOptions<InterviewApiOptions>>().Value;
            Assert.Equal("https://api.dummy.com/", options.BaseUrl);
            Assert.Equal("submit", options.SubmitTask);
            Assert.Equal("get", options.GetTask);

            // Assert that the HttpClient for InterviewApi is registered and has the correct BaseAddress
            var factory = serviceProvider.GetRequiredService<IHttpClientFactory>();
            var client = factory.CreateClient("IInterviewApi");
            Assert.Equal(new Uri("https://api.dummy.com/"), client.BaseAddress);
        }
    }
}

