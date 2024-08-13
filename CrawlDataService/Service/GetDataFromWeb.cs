using Common;
using Common.MultiThread;
using CrawlDataService.Common;
using HtmlAgilityPack;
using OpenQA.Selenium;
using Repository.Model;
using System.Text;

namespace CrawlDataService.Service
{
    public class GetDataFromWebService : BaseSerivce
    {
        int pageNovel = 1;
        public List<string> listPathNovel = new();
        public void GetNovelPath(int pageNumber, string pathSearch)
        {
            if (string.IsNullOrEmpty(pathSearch)) return;
            logger.Info($"Start crawl path novel page:{pageNumber}");
            try
            {
                pathSearch = PropertyExtension.CheckPathWeb(pathSearch);
                var html = pathSearch.DownloadStringWebClient();
                HtmlDocument htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);
                var bookListNode = htmlDoc.GetListHtmlNode("div", "class", "book-list");
                var infoCol = bookListNode?.GetListHtmlNode("div", "class", "info-col");
                var pathHrefs = infoCol?.GetListHtmlNode("a", "class", "tooltipped");
                var pathNovels = pathHrefs?.Select(e => e.Attributes["href"].Value).ToList();
                if (pathNovels is null || pathNovels.Count == 0) return;
                listPathNovel.AddRange(pathNovels);
                var pageNode = htmlDoc.GetHtmlNode("ul", "class", "pagination");
                var effectNodes = pageNode?.GetListHtmlNode("li", "class", "waves-effect");
                var hrefLink = effectNodes?.Where(e => e.GetListHtmlNode("i", "class", "fa fa-angle-right").Any()).Select(e => e.Descendants("a").FirstOrDefault().Attributes["href"].Value).FirstOrDefault();
                logger.Info($"End crawl path novel page:{pageNumber}");
            }
            catch (Exception ex)
            {
                logger.Error($"Error while crawl path novel, msg: {ex}");
            }
        }

        public async Task StartCrawlData(int numberBatch, string pathSave, string pathSearch)
        {
            try
            {
                GetNovelPath(1, pathSearch);
                HashSet<string> novelCrawled = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                novelCrawled = ReadFile.ReadFileTxt();
                listPathNovel = listPathNovel.Where(x => !novelCrawled.Contains(x)).ToList();
                MultiThreadHelper.MultiThread(listPathNovel, numberBatch, pathSave, GetDataNovel);
                WriteFile.WriteFileXLSX(pathSave, $"Report_{DateTime.Now.ToString("ddMMyyyyHHmmss")}.xlsx", dicNovel);
            }
            catch (Exception ex)
            {
                logger.Error($"Error while in multithread, msg: {ex}");
            }
        }


        public async Task StartReCrawData(int numberBatch, List<ChapterErrorLog> novelError, List<ChapterErrorLog> chapterError, string pathSave)
        {
            if (novelError?.Count > 0)
            {
                try
                {
                    var currentTaskCount = novelError.Count / numberBatch + (novelError.Count % numberBatch > 0 ? 1 : 0);
                    var batchNumber = currentTaskCount > RuntimeContext.MaxThraed ? (novelError.Count / RuntimeContext.MaxThraed + (novelError.Count % RuntimeContext.MaxThraed > 0 ? 1 : 0)) : numberBatch;
                    var tasks = new List<Task>();
                    foreach (var batch in novelError.Chunk(batchNumber))
                    {
                        var task = Task.Factory.StartNew(() =>
                        {
                            foreach (var item in batch)
                            {
                                GetDataNovel(item.PathNovel, pathSave);
                            }
                        });
                        tasks.Add(task);
                    }
                    Task.WaitAll(tasks.ToArray());
                    WriteFile.WriteFileXLSX(pathSave, $"Report_{DateTime.Now.ToString("ddMMyyyyHHmmss")}.xlsx", dicNovel);
                }
                catch (Exception ex)
                {
                    logger.Error($"Error while in setup multithread to ReCrawl Novel, msg: {ex}");
                }
            }
            if (chapterError?.Count > 0)
            {
                logger.Info("Start ReCrawl Chapter Error");
                try
                {
                    var currentTaskCount = chapterError.Count / numberBatch + (chapterError.Count % numberBatch > 0 ? 1 : 0);
                    var batchNumber = currentTaskCount > RuntimeContext.MaxThraed ? (chapterError.Count / RuntimeContext.MaxThraed + (chapterError.Count % RuntimeContext.MaxThraed > 0 ? 1 : 0)) : numberBatch;
                    var tasks = new List<Task>();
                    foreach (var batch in chapterError.Chunk(batchNumber))
                    {
                        var task = Task.Factory.StartNew(() =>
                        {
                            foreach (var item in batch)
                            {
                                //IWebDriver webDriver = NewWebClient.GetInstantWebDriver();
                                GetContentChapter("", item.ChapterNumber, item.PathChapterLocal, item.PathChapter);
                            }
                        });
                        tasks.Add(task);
                    }
                    Task.WaitAll(tasks.ToArray());
                    //WriteFile.WriteFileXLSX(pathSave, $"Report_{DateTime.Now.ToString("ddMMyyyyHHmmss")}", dicNovel);
                }
                catch (Exception ex)
                {
                    logger.Error($"Error while in setup multithread to ReCrawl chapter, msg: {ex}");

                }
            }

        }


        public async Task GetDataNovel(string pathNovel, string pathSave)
        {
            if (string.IsNullOrEmpty(pathNovel)) return;
            try
            {
                int chapterNumber = 1;
                pathNovel = PropertyExtension.CheckPathWeb(pathNovel);

                //string html = pathNovel.DownloadStringWebClient();
                IWebDriver webDriver = NewWebClient.GetInstantWebDriver();
                var html = webDriver.GetWebBySelenium(pathNovel);
                HtmlDocument htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);

                //get name of novel
                var nameNovel = htmlDoc.GetHtmlNode("title")?.InnerText;

                //get gener of novel
                var nodeDesc = htmlDoc.GetHtmlNode("div", "class", "book-desc");
                var listGener = nodeDesc?.Descendants("p").FirstOrDefault()?.Descendants("a").Select(e => e.InnerText);
                var gener = listGener.JoinGender();

                //get author of novel
                var convertInfo = htmlDoc.GetHtmlNode("div", "class", "cover-info");
                var infos = convertInfo?.Descendants("p").Select(e => e.InnerText.Split(":"));
                var author = infos?.Where(e => e[0].Trim().Equals("Tác giả")).FirstOrDefault()?[1];

                //get description of novel
                var descriptionDoc = nodeDesc?.GetHtmlNode("div", "class", "book-desc-detail");
                var description = descriptionDoc?.Descendants("p").FirstOrDefault()?.InnerText;

                //get link first chapter
                var controlBtnNode = htmlDoc.GetHtmlNode("div", "class", "control-btns");
                var tagA = controlBtnNode?.GetListHtmlNode("a", "class", "btn waves-effect waves-light orange-btn");
                var hrefValue = tagA?.FirstOrDefault(e => e.InnerText.Equals("Đọc"))?.Attributes["href"].Value;

                var pathFolder = WriteFile.CreateFolder(pathSave, nameNovel.RemoveDiacriticsAndSpaces());

                var novel = new Novel
                {
                    Name = nameNovel,
                    Genre = gener,
                    NumberChapter = "",
                    Author = author,
                    Description = description,
                    Path = pathFolder
                };
                if (!dicNovel.ContainsKey(nameNovel))
                {
                    dicNovel.Add(nameNovel, novel);
                }
                await GetContentChapter(nameNovel, chapterNumber, pathFolder, hrefValue);

                WriteFile.WriteFileTxt(nameNovel);
                logger.Info($"End crawl data novel {nameNovel}");

            }
            catch (Exception ex)
            {
                logger.Error($"Error while crawl data novel {pathNovel}, msg: {ex}");
                chapterLog.Info(PropertyExtension.FormatErrorChapter(true, pathNovel, 0, null, null));
            }
        }

        public async Task GetContentChapter(string nameNovel, int chaper, string pathSave, string? pathChapter)
        {
            if (string.IsNullOrEmpty(pathChapter)) return;
            pathChapter = PropertyExtension.CheckPathWeb(pathChapter);
            //if (chaper % 50 == 0) Thread.Sleep(25000);
            string[] titleChapters = new string[10];
            string titleChapter;
            try
            {
                logger.Info($"Start crawl novel:{nameNovel}, chapter: {chaper}");
                HtmlDocument htmlDoc = new HtmlDocument();
                var html = pathChapter.DownloadStringWebClient();

                //get title of chapter
                htmlDoc.LoadHtml(html);
                titleChapters = htmlDoc.DocumentNode.Descendants("title").FirstOrDefault()?.InnerText.Split("-");
                titleChapter = titleChapters?.Count() > 1 ? titleChapters[1] : titleChapters?.Count() > 0 ? titleChapters[0] : $"Chuong {chaper}";

                //get content of chapter 
                var contentDoc = htmlDoc.GetHtmlNode("div", "class", "content-body-wrapper");
                var contentNodes = contentDoc?.GetHtmlNode("div", "id", "bookContentBody");
                var content = GetContentFromHTML(contentNodes);
                WriteFile.WriteFileTxt(pathSave, titleChapter.RemoveDiacriticsAndSpaces(), content);

                //get link in  btnNext Chapter
                var indexBox = htmlDoc.GetHtmlNode("span", "class", "index-box");
                var tagA = indexBox?.GetHtmlNode("a", "id", "btnNextChapter");
                var nextChapterPath = $"{pathWeb}{tagA?.Attributes["href"].Value}";

                await GetContentChapter(nameNovel, chaper += 1, pathSave, nextChapterPath);
                //webDriver.Quit();
                return;
            }
            catch (Exception ex)
            {
                logger.Error($"Error while crawl data novel {nameNovel}, chapter: {chaper}, msg: {ex}");
                chapterLog.Info(PropertyExtension.FormatErrorChapter(false, null, chaper, pathChapter, pathSave));
                return;
            }
        }

        private string GetContentFromHTML(HtmlNode? htmlNode)
        {
            if (htmlNode == null) return string.Empty;
            StringBuilder stringBuilder = new();
            var contentNodes = htmlNode.Descendants("p");
            foreach (var child in contentNodes)
            {
                stringBuilder.AppendLine(child.InnerText);
            }
            return stringBuilder.ToString();
        }

    }
}
