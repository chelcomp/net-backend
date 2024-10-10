using Microsoft.Extensions.Options;

namespace InterviewAssignment.Infrastructure.InterviewApi;

public static class InterviewApiService
{
    public static IServiceCollection ConfigureInterviewApi(this IServiceCollection service)
    {
        service.AddOptions<InterviewApiOptions>()
            .BindConfiguration("InterviewApi")
            .ValidateDataAnnotations();

        service.AddHttpClient<IInterviewApi>((provider, client) =>
        {
            var options = provider.GetRequiredService<IOptions<InterviewApiOptions>>();
            client.BaseAddress = new Uri(options.Value.BaseUrl);
        });

        return service;
    }
}