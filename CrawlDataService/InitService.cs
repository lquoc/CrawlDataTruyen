using Amazon.Polly;
using Amazon;
using Aspose.Cells.Charts;
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
            service.AddScoped<GetDataFromWebService>();
            service.AddSingleton<IAmazonPolly>(sp =>
            {
                var config = new AmazonPollyConfig
                {
                    RegionEndpoint = RegionEndpoint.APSoutheast1,// Chọn vùng phù hợp
                };
                var credentials = new BasicAWSCredentials("AKIA2AUOPJZ42AFK3N5E", "6cjlKlxCjpZlDf5X7Vis+lOMBrE+qN6d1LrpwH9H");
                return new AmazonPollyClient(credentials, config);
            });
            service.AddScoped<ChangeTextToVoice>();
            return service;
        }

    }
}
