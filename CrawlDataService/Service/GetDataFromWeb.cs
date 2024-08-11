using Common;
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
                IWebDriver webDrive = NewWebClient.GetInstantWebDriver();
                var html = webDrive.GetWebBySelenium(pathSearch);
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
                //GetNovelPath(pageNovel++, hrefLink?.Replace(";", "&"));
                webDrive.Quit();
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
                var currentTaskCount = listPathNovel.Count / numberBatch + (listPathNovel.Count % numberBatch > 0 ? 1 : 0);
                var batchNumber = currentTaskCount > RuntimeContext.MaxThraed ? (listPathNovel.Count / RuntimeContext.MaxThraed + (listPathNovel.Count % RuntimeContext.MaxThraed > 0 ? 1 : 0)) : numberBatch;
                var tasks = new List<Task>();
                foreach (var batch in listPathNovel.Chunk(batchNumber))
                {
                    var task = Task.Factory.StartNew(() =>
                    {
                        foreach (var item in batch)
                        {
                            GetDataNovel(item, pathSave);
                        }
                    });
                    tasks.Add(task);
                }
                Task.WaitAll(tasks.ToArray());
                WriteFile.WriteFileXLSX(pathSave, $"Report_{DateTime.Now.ToString("ddMMyyyyHHmmss")}", dicNovel);
            }
            catch (Exception ex)
            {
                logger.Error($"Error while in multithread, msg: {ex}");
            }
        }


        public async Task StartReCrawData(int numberBatch, List<ChapterErrorLog> novelError, List<ChapterErrorLog> chapterError, string pathSave)
        {

            logger.Info("Start ReCrawl Novel Error");
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
                WriteFile.WriteFileXLSX(pathSave, $"Report_{DateTime.Now.ToString("ddMMyyyyHHmmss")}", dicNovel);
            }
            catch (Exception ex)
            {
                logger.Error($"Error while in setup multithread to ReCrawl Novel, msg: {ex}");
            }

            logger.Info("Start ReCrawl Novel Error");
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
                            IWebDriver webDriver = NewWebClient.GetInstantWebDriver();
                            GetContentChapter("", item.ChapterNumber, item.PathNovel, pathSave, webDriver);
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


        public async Task GetDataNovel(string pathNovel, string pathSave)
        {
            if (string.IsNullOrEmpty(pathNovel)) return;
            try
            {
                int chapterNumber = 1;
                pathNovel = PropertyExtension.CheckPathWeb(pathNovel);
                IWebDriver webDriver = NewWebClient.GetInstantWebDriver();
                var html = webDriver.GetWebBySelenium(pathNovel);
                HtmlDocument htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);
                var nameNovel = htmlDoc.GetHtmlNode("title")?.InnerText;

                var nodeDesc = htmlDoc.GetHtmlNode("div", "class", "book-desc");
                var listGener = nodeDesc?.Descendants("p").FirstOrDefault()?.Descendants("a").Select(e => e.InnerText);
                var gener = listGener.JoinGender();

                var convertInfo = htmlDoc.GetHtmlNode("div", "class", "cover-info");
                var infos = convertInfo?.Descendants("p").Select(e => e.InnerText.Split(":"));
                var author = infos?.Where(e => e[0].Trim().Equals("Tác giả")).FirstOrDefault()?[1];

                var descriptionDoc = nodeDesc?.GetHtmlNode("div", "class", "book-desc-detail");
                var description = descriptionDoc?.Descendants("p").FirstOrDefault()?.InnerText;

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
                await GetContentChapter(nameNovel, chapterNumber, pathFolder, hrefValue, webDriver);

                WriteFile.WriteFileTxt(nameNovel);
                logger.Info($"End crawl data novel {nameNovel}");
                webDriver.Quit();

            }
            catch (Exception ex)
            {
                logger.Error($"Error while crawl data novel {pathNovel}, msg: {ex}");
                chapterLog.Info(PropertyExtension.FormatErrorChapter(true, pathNovel, 0, null, null));
            }
        }

        public async Task GetContentChapter(string nameNovel, int chaper, string pathSave, string? pathChapter, IWebDriver webDriver)
        {
            if (string.IsNullOrEmpty(pathChapter)) return;
            pathChapter = PropertyExtension.CheckPathWeb(pathChapter);
            //if (chaper % 50 == 0) Thread.Sleep(25000);
            try
            {
                logger.Info($"Start crawl novel:{nameNovel}, chapter: {chaper}");
                HtmlDocument htmlDoc = new HtmlDocument();
                string titleChapter = "";
                do
                {
                    var html = webDriver.GetWebBySelenium(pathChapter);
                    htmlDoc.LoadHtml(html);
                    var titleChapters = htmlDoc.DocumentNode.Descendants("title").FirstOrDefault()?.InnerText.Split("-");
                    titleChapter = titleChapters?.Count() > 1 ? titleChapters[1] : titleChapters?.Count() > 0 ? titleChapters[0] : $"Chuong {chaper}";
                    if (titleChapter.Equals("503 Service Temporarily Unavailable", StringComparison.OrdinalIgnoreCase)) 
                    {
                        Thread.Sleep(5000);
                        logger.Warn($"Sleep 5s, {titleChapter}");
                    } 
                } while (titleChapter.Equals("503 Service Temporarily Unavailable", StringComparison.OrdinalIgnoreCase));
                var contentDoc = htmlDoc.GetHtmlNode("div", "class", "content-body-wrapper");
                var contentNodes = contentDoc?.GetHtmlNode("div", "id", "bookContentBody");
                var content = GetContentFromHTML(contentNodes);
                WriteFile.WriteFileTxt(pathSave, titleChapter.RemoveDiacriticsAndSpaces(), content);
                var indexBox = htmlDoc.GetHtmlNode("span", "class", "index-box");
                var tagA = indexBox?.GetHtmlNode("a", "id", "btnNextChapter");
                var nextChapterPath = $"{pathWeb}{tagA?.Attributes["href"].Value}";
                //Thread.Sleep(3000 * RuntimeContext.MaxThraed);
                await GetContentChapter(nameNovel, chaper += 1, pathSave, nextChapterPath, webDriver);
                webDriver.Quit();
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
