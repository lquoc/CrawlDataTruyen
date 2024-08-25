using Amazon.Runtime;
using Common;
using CrawlDataService.Service;
using Microsoft.ClearScript.JavaScript;
using Microsoft.Extensions.DependencyInjection;
using static Repository.Enum.ListEnum;

namespace CrawlDataService.Common
{
    public class ManagerService
    {

        public ManagerService(IServiceProvider service)
        {

        }
        // get the service based on the service name and the Enum PageWeb. 
        public CrawlNovelSerivce? CreateInstantService()
        {
            try
            {
                var listServiceCrawl = InitService.GetTypesService().ToList();
                var serviceNeedCreateInstant = listServiceCrawl.Where(e => e.Name.Contains(RuntimeContext.EnumWeb.ToString())).FirstOrDefault();
                if(serviceNeedCreateInstant != null)
                {
                    return RuntimeContext._serviceProvider.GetRequiredService(serviceNeedCreateInstant) as CrawlNovelSerivce;
                }
                return null;
            }
            catch (Exception ex)
            {
                RuntimeContext.logger.Error($"Error while create instant service, msg: {ex}");
            }
            return null;
        }

        public async Task StartService()
        {
            var crawlNovelService = CreateInstantService();
            var getChangeTextToVoice = RuntimeContext._serviceProvider.GetRequiredService<ChangeTextToVoice>();
            var mp4Service = RuntimeContext._serviceProvider.GetService<MP4Service>();
            if (RuntimeContext.IsStart && RuntimeContext.TypeCrawl == TypeCrawl.MultiNovel)
            {
                await crawlNovelService?.StartCrawlData(1,RuntimeContext.PathSaveLocal, RuntimeContext.PathSaveFileMp3, RuntimeContext.PathCrawl);
            }
            else if (RuntimeContext.IsStart && RuntimeContext.TypeCrawl == TypeCrawl.OneNovel)
            {
                await crawlNovelService?.StartGetInfoNovelAndChapter(RuntimeContext.PathCrawl, RuntimeContext.PathSaveLocal, RuntimeContext.PathSaveFileMp3);
            }
        }
    }
}
