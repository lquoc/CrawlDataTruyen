﻿using Common;
using CrawlDataService.Service;
using Microsoft.Extensions.DependencyInjection;
using Repository.Model;
using static Repository.Enum.ListEnum;

namespace CrawlDataService.Common
{
    public class ManagerService
    {
        readonly ChangeTextToVoice changeTextToVoiceService;
        readonly MP4Service mp4Service;

        public ManagerService(IServiceProvider service)
        {
            changeTextToVoiceService = service.GetRequiredService<ChangeTextToVoice>();
            mp4Service = service.GetRequiredService<MP4Service>();
        }

        // get the service based on the service name and the Enum PageWeb. 
        public CrawlNovelSerivce? CreateInstantService()
        {
            try
            {
                var listServiceCrawl = InitService.GetTypesService().ToList();
                var serviceNeedCreateInstant = listServiceCrawl.Where(e => e.Name.Contains(RuntimeContext.EnumWeb.ToString())).FirstOrDefault();
                if (serviceNeedCreateInstant != null)
                {
                    return RuntimeContext._serviceProvider.GetRequiredService(serviceNeedCreateInstant) as CrawlNovelSerivce;
                }
                return null;
            }
            catch (Exception ex)
            {
                RuntimeContext.logger.Error($"Error while create instant service, msg: {ex}");
            }
            return null;
        }

        public async Task StartNovelService(Action<bool> action)
        {
            var crawlNovelService = CreateInstantService();
            var getChangeTextToVoice = RuntimeContext._serviceProvider.GetRequiredService<ChangeTextToVoice>();
            var mp4Service = RuntimeContext._serviceProvider.GetService<MP4Service>();
            if (crawlNovelService != null && RuntimeContext.IsStart && RuntimeContext.TypeCrawl == TypeCrawl.MultiNovel)
            {
                await StartCrawlMultiNovel(crawlNovelService, 1, RuntimeContext.PathSaveLocal, RuntimeContext.PathSaveFileMp3, RuntimeContext.PathCrawl);
            }
            else if (crawlNovelService != null && RuntimeContext.IsStart && RuntimeContext.TypeCrawl == TypeCrawl.OneNovel)
            {
                await StartCrawlSingleNovel(crawlNovelService, 1, RuntimeContext.PathSaveLocal, RuntimeContext.PathSaveFileMp3, RuntimeContext.PathCrawl);
            }
            RuntimeContext.IsStart = false;
            action(true);
        }

        public async Task StartCrawlMultiNovel(CrawlNovelSerivce service, int numberBatch, string pathSaveLocel, string PathSaveFileMp3OrMp4, string pathSearchNovel)
        {
            var listNovelPath = service.GetLinksNovel(pathSearchNovel);
            if (listNovelPath is null || listNovelPath.Count() == 0) return;
            var currentTaskCount = listNovelPath.Count / numberBatch + (listNovelPath.Count % numberBatch > 0 ? 1 : 0);
            var batchNumber = currentTaskCount > RuntimeContext.MaxThread ? (listNovelPath.Count / RuntimeContext.MaxThread + (listNovelPath.Count % RuntimeContext.MaxThread > 0 ? 1 : 0)) : numberBatch;
            var tasks = new List<Task>();
            foreach (var batch in listNovelPath.Chunk(batchNumber))
            {
                var task = Task.Run(async () =>
                {
                    foreach (var novelPath in batch)
                    {
                        if (RuntimeContext.IsStart)
                        {
                            var novel = await service.StartGetInfoNovel(novelPath, pathSaveLocel, PathSaveFileMp3OrMp4);
                            var listAllChapter = service.GetAllLinksChapter(novelPath);
                            if (novel != null && listAllChapter != null)
                            {
                                foreach (var item in listAllChapter)
                                {
                                    var chapterInfo = await service.GetContentChapter(novel, item);
                                    WriteFileTextAndMp3OrMp4(novel, chapterInfo);
                                }
                            }
                            RuntimeContext.logger.Info($"End crawl data novel: {novelPath}");
                        }
                    }
                });
                tasks.Add(task);
            }
            await Task.WhenAll(tasks.ToArray());
            RuntimeContext.logger.Info($"End crawl multi thread novel");
        }

        public async Task StartCrawlSingleNovel(CrawlNovelSerivce service, int numberBatch, string pathSaveLocel, string PathSaveFileMp3OrMp4, string pathNovel)
        {
            var novel = await service.StartGetInfoNovel(pathNovel, pathSaveLocel, PathSaveFileMp3OrMp4);
            var listAllChapter = service.GetAllLinksChapter(pathNovel);
            if (novel != null && listAllChapter != null)
            {
                if (listAllChapter is null || listAllChapter.Count() == 0) return;
                var currentTaskCount = listAllChapter.Count / numberBatch + (listAllChapter.Count % numberBatch > 0 ? 1 : 0);
                var batchNumber = currentTaskCount > RuntimeContext.MaxThread ? (listAllChapter.Count / RuntimeContext.MaxThread + (listAllChapter.Count % RuntimeContext.MaxThread > 0 ? 1 : 0)) : numberBatch;
                var tasks = new List<Task>();
                foreach (var batch in listAllChapter.Chunk(batchNumber))
                {
                    var task = Task.Factory.StartNew(async () =>
                    {
                        foreach (var chapterPath in batch)
                        {
                            if (RuntimeContext.IsStart)
                            {
                                var chapterInfo = await service.GetContentChapter(novel, chapterPath);
                                if (!service.IsManga)
                                {
                                    WriteFileTextAndMp3OrMp4(novel, chapterInfo);
                                }
                            }
                        }
                    });
                    tasks.Add(task);
                }
                await Task.WhenAll(tasks.ToArray());
                RuntimeContext.logger.Info($"End crawl data novel: {pathNovel}");
            }
        }

        public async void WriteFileTextAndMp3OrMp4(Novel novel, ChapterInfo? chapterInfo)
        {
            WriteFile.WriteFileTxt(novel.PathLocal.Trim(), chapterInfo?.TitleChapter.RemoveDiacriticsAndSpaces(), chapterInfo?.ContentChapter);
            if (RuntimeContext.IsChangeTextIntoVoice && RuntimeContext.TypeFile == Repository.Enum.ListEnum.TypeFile.MP3)
            {
                await changeTextToVoiceService.RequestCreateSpeechGoogleCloudy(chapterInfo?.ContentChapter, novel.VoiceOrMP4Path, chapterInfo?.TitleChapter.RemoveDiacriticsAndSpaces());
            }
            else if (RuntimeContext.IsChangeTextIntoVoice && RuntimeContext.TypeFile == Repository.Enum.ListEnum.TypeFile.MP4)
            {
                await mp4Service.CreateVideoFromMp3AndImages(chapterInfo?.ContentChapter, novel.ImgPathLocal, novel.VoiceOrMP4Path, chapterInfo?.TitleChapter.RemoveDiacriticsAndSpaces());
            }
        }
    }
}
