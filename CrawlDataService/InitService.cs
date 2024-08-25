using Amazon;
using Amazon.Polly;
using Amazon.Runtime;
using Common;
using CrawlDataService.Common;
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
            AddServiceCrawl(service);
            IServiceProvider serviceProvider = service.BuildServiceProvider();
            RuntimeContext._serviceProvider = serviceProvider;
        }
        private static void AddServiceCrawl(IServiceCollection services)
        {
            var types = typeof(CrawlNovelFromWiki).Assembly.GetTypes().ToList();
            var typesServiceCrawl = types.Where(e => e.IsSubclassOf(typeof(CrawlNovelSerivce)));
            foreach (var type in typesServiceCrawl)
            {
                services.AddScoped(type);
                var interfaces = type.GetInterfaces();
                foreach (var i in interfaces)
                {
                    services.AddScoped(i, type);
                }
            }
        }


        private static IServiceCollection AddServiceCore(ServiceCollection service)
        {
            service.AddSingleton<IAmazonPolly>(sp =>
            {
                var config = new AmazonPollyConfig
                {
                    RegionEndpoint = RegionEndpoint.APSoutheast1,// Chọn vùng phù hợp
                };
                var credentials = new BasicAWSCredentials("Key Api", "Secret key");
                return new AmazonPollyClient(credentials, config);
            });
            service.AddScoped<ChangeTextToVoice>();
            service.AddScoped<MP4Service>();
            service.AddScoped<ManagerService>();
            return service;
        }

    }
}
