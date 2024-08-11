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

    }
}
