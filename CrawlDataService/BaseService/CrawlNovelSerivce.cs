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

        public abstract Task StartCrawlData(int numberBatch, string pathSave, string pathSaveVoice, string pathSearch);

        public abstract List<string>? GetLinksNovel(string path);

        public abstract Task<Novel?> StartGetInfoNovel(string pathNovel, string pathSave, string pathSaveVoice);

        public abstract List<string>? GetAllLinksChapter(string pathNovel);

        public abstract Task<ChapterInfo?> GetContentChapter(Novel novel, string? pathChapter);
    }
}
