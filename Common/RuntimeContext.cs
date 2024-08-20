using Common.ErrorNovel;
using Migration.Common;
using Repository.Enum;
using System.Net.NetworkInformation;
using static Repository.Enum.ListEnum;

namespace Common
{
    public static class RuntimeContext
    {
        //common
        public static MyLogger logger = MyLogger.GetInstance();
        public static ChapterErrorLogger chapterLog = ChapterErrorLogger.GetInstance();
        public static IServiceProvider _serviceProvider;
        public static int MaxThread = 3;
        public static bool IsStart = false;

        //crawl data novel
        public static ListEnum.EnumPage EnumPage = 0;
        public static string PathCrawl = string.Empty;
        public static string PathChapterError = Path.Combine(AppContext.BaseDirectory, "errorchapterlogs","chapterError.log");
        public static string NameFileNovelList = "novelList.txt";
        public static string PathSaveLocal = string.Empty;
        public static int numberBatch = 1000;
        public static TypeCrawl TypeCrawl = TypeCrawl.MultiNovel;


        //change text to voice
        public static bool IsChangeTextIntoVoice = false;
        public static string PathSaveFileMp3 = string.Empty;
        public static int NumberChapterInFile = 5;

        //create mp4 file
        public static TypeFile TypeFile = TypeFile.MP3;


        //proxy key
        public static string ProxyKey = "7-QgvJwlpjd9GvY_bEG43qEECp-fi_sKDeC6ygwz7Vs";
        public static bool IsChangeProxy = false;
    }
}
