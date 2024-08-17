using Amazon.Polly;
using Amazon;
using Common;
using CrawlDataService.Service;
using Microsoft.Extensions.DependencyInjection;
using Amazon.Runtime;

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
            service.AddScoped<CrawlNovelFromWiki>();
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
            service.AddScoped<CrawlNovelFromDTruyen>();
            return service;
        }

    }
}
