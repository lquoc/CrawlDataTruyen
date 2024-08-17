using Common.ErrorNovel;
using Migration.Common;

namespace CrawlDataService.Service
{
    public abstract class CrawlNovelSerivce
    {
        public MyLogger logger = MyLogger.GetInstance();
        public ChapterErrorLogger chapterLog = ChapterErrorLogger.GetInstance();

        public virtual async Task StartCrawlData(int numberBatch, string pathSave, string pathSaveVoice, string pathSearch)
        {
        }
        public virtual async Task StartCrawlOnlyOneNovel(int numberBatch, string pathSave, string pathSaveVoice, string pathNovel)
        {

        }

    }
}
