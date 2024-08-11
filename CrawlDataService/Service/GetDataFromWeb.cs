using Common;
using CrawlDataService.Common;
using HtmlAgilityPack;
using Repository.Model;
using System.Net;
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
                var webClient = new WebClient();
                var html = webClient.CreateWebClient().DownloadString(pathSearch);
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
                GetNovelPath(pageNovel++, hrefLink?.Replace(";", "&"));

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
                WriteFileTxt.WriteFileXLSX(pathSave, $"Report_{DateTime.Now.ToString("ddMMyyyyHHmmss")}", dicNovel);
            }
            catch (Exception ex)
            {
                logger.Error($"while in multithread, msg: {ex}");
            }
        }
        


        public async Task GetDataNovel(string pathNovel, string pathSave)
        {
            if(string.IsNullOrEmpty(pathNovel)) return;
            try
            {
                int chapterNumber = 1;
                pathNovel = PropertyExtension.CheckPathWeb(pathNovel);
                var webClient = new WebClient();
                var html = webClient.CreateWebClient().DownloadString(pathNovel);
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
                var pathFolder = WriteFileTxt.CreateFolder(pathSave, nameNovel.RemoveDiacriticsAndSpaces());

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
                logger.Info($"End crawl data novel {nameNovel}");
            }
            catch (Exception ex)
            {
                logger.Error($"Error while crawl data novel {pathNovel}, msg: {ex}");
                chapterLog.Info(PropertyExtension.FormatErrorChapter(true, pathNovel, 0, null));
            }
        }
        
        public async Task GetContentChapter(string nameNovel, int chaper, string pathSave, string? pathChapter)
        {
            if (string.IsNullOrEmpty(pathChapter)) return;
            pathChapter = PropertyExtension.CheckPathWeb(pathChapter);
            if (chaper % 50 == 0) Thread.Sleep(30000);
            try
            {
                logger.Info($"Start crawl novel:{nameNovel}, chapter: {chaper}");
                var webClient = new WebClient();
                var html = webClient.CreateWebClient().DownloadString(pathChapter);
                HtmlDocument htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);
                var titleChapter = htmlDoc.DocumentNode.Descendants("title").FirstOrDefault()?.InnerText.Split("-")[1];
                var contentDoc = htmlDoc.GetHtmlNode("div", "class", "content-body-wrapper");
                var contentNodes = contentDoc?.GetHtmlNode("div", "id", "bookContentBody");
                var content = GetContentFromHTML(contentNodes);
                WriteFileTxt.WriteFile(pathSave, titleChapter.RemoveDiacriticsAndSpaces(), content);
                var indexBox = htmlDoc.GetHtmlNode("span", "class", "index-box");
                var tagA = indexBox?.GetHtmlNode("a", "id", "btnNextChapter");
                var nextChapterPath = $"{pathWeb}{tagA?.Attributes["href"].Value}";
                Thread.Sleep(10000);
                await GetContentChapter(nameNovel, chaper += 1, pathSave, nextChapterPath);
            }
            catch (Exception ex)
            {
                logger.Error($"Error while crawl data novel {nameNovel}, chapter: {chaper}, msg: {ex}");
                chapterLog.Info(PropertyExtension.FormatErrorChapter(false, null, chaper, pathChapter));
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
