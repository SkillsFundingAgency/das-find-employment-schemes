
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SFA.DAS.FindEmploymentSchemes.Web.Services;


namespace SFA.DAS.FindEmploymentSchemes.UnitTests
{
    public static class Program
    {
        public static IServiceProvider GetServices()
        {
            return new HostBuilder()
                .ConfigureServices((hostContext, services) => {
                    services.AddOptions();
                    services.AddSingleton<IFilterService, FilterService>();
                })
                .Build()
                .Services;
        }
    }
}
