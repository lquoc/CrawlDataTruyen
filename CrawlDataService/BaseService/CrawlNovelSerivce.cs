using Common.ErrorNovel;
using Microsoft.Extensions.Hosting;
using Migration.Common;
using Repository.Model;

namespace CrawlDataService.Service
{
    public abstract class CrawlNovelSerivce
    {
        public MyLogger logger = MyLogger.GetInstance();
        public ChapterErrorLogger chapterLog = ChapterErrorLogger.GetInstance();

        public virtual async Task StartCrawlData(int numberBatch, string pathSave, string pathSaveVoice, string pathSearch)
        {
        }

        public virtual List<string>? GetLinksNovel(string path)
        {
            return null;
        }

        public virtual async Task<Novel?> StartGetInfoNovel(string pathNovel, string pathSave, string pathSaveVoice)
        {
            return null;
        }

        public virtual List<string>? GetAllLinksChapter(string pathNovel)
        {
            return null;
        }

        public virtual async Task<ChapterInfo?> GetContentChapter(int chaper, Novel novel, string? pathChapter)
        {
            return null;
        }
    }
}
