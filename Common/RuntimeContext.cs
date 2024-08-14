using Common.ErrorNovel;
using Migration.Common;

namespace Common
{
    public static class RuntimeContext
    {
        public static MyLogger logger = MyLogger.GetInstance();
        public static ChapterErrorLogger chapterLog = ChapterErrorLogger.GetInstance();
        public static IServiceProvider _serviceProvider;
        public static int MaxThraed = 3;
        public static string PathChapterError = Path.Combine(AppContext.BaseDirectory, "errorchapterlogs","chapterError.log");
        public static string NameFileNovelList = "novelList.txt";
        public static string PathSaveLocal = "";
        public static int numberBatch = 1000;
    }
}
