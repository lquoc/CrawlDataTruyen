using Common;
using CrawlDataService.Service;

namespace CrawlDataService
{
    public class CrawlNovelFromDTruyen : CrawlNovelSerivce
    {

        public override async Task StartCrawlData(int numberBatch, string pathSave, string pathSaveVoice, string pathSearch)
        {
            RuntimeContext.logger.Info("CrawlNovelFromDTruyen");
        }






        public override async Task StartCrawlOnlyOneNovel(int numberBatch, string pathSave, string pathSaveVoice, string pathNovel)
        {

        }
    }
}
