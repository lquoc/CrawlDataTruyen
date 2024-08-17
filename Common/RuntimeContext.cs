using Common.ErrorNovel;
using Migration.Common;

namespace Common
{
    public static class RuntimeContext
    {
        //common
        public static MyLogger logger = MyLogger.GetInstance();
        public static ChapterErrorLogger chapterLog = ChapterErrorLogger.GetInstance();
        public static IServiceProvider _serviceProvider;
        public static int MaxThraed = 3;
       //crawl data novel
        public static string PathChapterError = Path.Combine(AppContext.BaseDirectory, "errorchapterlogs","chapterError.log");
        public static string NameFileNovelList = "novelList.txt";
        public static string PathSaveLocal = string.Empty;
        public static int numberBatch = 1000;
        
        //change text to voice
        public static bool IsChangeTextIntoVoice = false;
        public static string PathSaveFileMp3 = string.Empty;
        public static int NumberChapterInFile = 5;

    }
}
