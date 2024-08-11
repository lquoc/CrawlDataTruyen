﻿using Common.ErrorNovel;
using Migration.Common;
using Repository.Model;

namespace CrawlDataService.Service
{
    public class BaseSerivce
    {
        public MyLogger logger = MyLogger.GetInstance();
        public ChapterErrorLogger chapterLog = ChapterErrorLogger.GetInstance();
        public string pathWeb = "https://truyenwikidich.net";
        public Dictionary<string, Novel> dicNovel = new Dictionary<string, Novel>();
    }
}
