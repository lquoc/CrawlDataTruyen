using Common;
using CrawlDataService.Service;
using Microsoft.Extensions.DependencyInjection;

namespace CrawlDataService
{
    public static class InitService
    {
        public static void InitServiceProvider()
        {
            var service = new ServiceCollection();
            AddServiceCore(service);
            IServiceProvider serviceProvider = service.BuildServiceProvider();
            RuntimeContext._serviceProvider = serviceProvider;
        }
        private static IServiceCollection AddServiceCore(ServiceCollection service)
        {
            service.AddScoped<GetDataFromWebService>();
            return service;
        }

    }
}
