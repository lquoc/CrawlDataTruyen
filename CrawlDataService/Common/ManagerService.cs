using Common;
using CrawlDataService.Service;
using Microsoft.Extensions.DependencyInjection;
using static Repository.Enum.ListEnum;

namespace CrawlDataService.Common
{
    public class ManagerService
    {

        public ManagerService(IServiceProvider service)
        {

        }

        public async Task StartService()
        {
            CrawlNovelSerivce crawlNovelService;
            if (RuntimeContext.EnumPage == EnumPage.WikiDich)
            {
                crawlNovelService = RuntimeContext._serviceProvider.GetRequiredService<CrawlNovelFromWiki>();
            }
            else
            {
                crawlNovelService = RuntimeContext._serviceProvider.GetRequiredService<CrawlNovelFromDTruyen>();
            }
            var getChangeTextToVoice = RuntimeContext._serviceProvider.GetRequiredService<ChangeTextToVoice>();
            var mp4Service = RuntimeContext._serviceProvider.GetService<MP4Service>();
            //mp4Service.TestCreateMP4();
            if (RuntimeContext.IsStart && RuntimeContext.TypeCrawl == TypeCrawl.MultiNovel)
            {
                await crawlNovelService.StartCrawlData(1,RuntimeContext.PathSaveLocal, RuntimeContext.PathSaveFileMp3, RuntimeContext.PathCrawl);
            }
            else if (RuntimeContext.IsStart && RuntimeContext.TypeCrawl == TypeCrawl.OneNovel)
            {
                await crawlNovelService.StartGetInfoNovelAndChapter(RuntimeContext.PathCrawl, RuntimeContext.PathSaveLocal, RuntimeContext.PathSaveFileMp3);
            }
        }
    }
}
