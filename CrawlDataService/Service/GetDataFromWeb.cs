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
                //WriteFile.WriteFileXLSX(pathSave, $"Report_{DateTime.Now.ToString("ddMMyyyyHHmmss")}.xlsx", dicNovel);
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
                    MultiThreadHelper.MultiThread(novelError.Select(x => x.PathNovel).ToList(), numberBatch, pathSave, GetDataNovel);
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
                    chapterError.MultiThread(numberBatch, GetContentChapter);
                }
                catch (Exception ex)
                {
                    logger.Error($"Error while in setup multithread to ReCrawl chapter, msg: {ex}");

                }
            }

        }

        public async Task<List<string>> GetAllChapterLink(string novelName, string idNovel, string signKey, int numberFuzzy)
        {
            var startChapter = 0;
            var size = 501;
            var isTrue = true;
            var lastChapter = 0;
            List<string> allChapterPath = new();
            while (isTrue)
            {
                logger.Info($"Start get chapter of novel name:{novelName}, start:{startChapter}, size: {size}");
                
                //create sign value and path index.
                var sign = (signKey + startChapter.ToString() + size.ToString()).FuzzySign(numberFuzzy).SignFunc();
                var linkIndexChapter = $"{pathIndex}{idNovel}&start={startChapter}&size={size}&signKey={signKey}&sign={sign}";
                string html = "";
                try
                {

                    html = linkIndexChapter.DownloadStringWebClient();
                    HtmlDocument htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(html);
                    //get link of chapter
                    var listTagLi = htmlDoc.GetListHtmlNode("li", "class", "chapter-name");
                    var listTagA = listTagLi?.GetListHtmlNode("a", "class", "truncate");
                    var getHrefValue = listTagA?.Select(e => e.Attributes["href"].Value).ToList();
                    
                    //get index last chapter
                    if(lastChapter == 0)
                    {
                        var pageNode = htmlDoc.GetHtmlNode("ul", "class", "pagination");
                        var effectNodes = pageNode?.GetListHtmlNode("li", "class", "waves-effect");
                        effectNodes?.AddRange(pageNode?.GetListHtmlNode("li","class", " waves-effect "));
                        var numberPathOfEffects = effectNodes?.Select(e => e.GetHtmlNode("a", "data-action", "loadBookIndex")?.Attributes["data-start"].Value).ToList();
                        var lastPage = numberPathOfEffects?.Where(e => !string.IsNullOrEmpty(e)).Select(e => int.Parse(e)).ToList().Max();
                        if(lastPage != null)
                            lastChapter = lastPage.Value;
                    }
                    allChapterPath.AddRange(getHrefValue);
                    logger.Info($"End get chapter of novel id:{idNovel}, start:{startChapter}, size: {size}");
                }
                catch (Exception ex)
                {
                    logger.Error($"Error while get path chapter of novel {novelName}");
                }
                
                if(string.IsNullOrEmpty(html) || startChapter == lastChapter)
                {
                    isTrue = false;
                    logger.Info($"End get chapter of novel id:{idNovel}");
                }
                else
                {
                    startChapter += size;
                }
            }
            return allChapterPath;
        }

        public async Task GetDataNovel(string pathNovel, string pathSave)
        {
            if (string.IsNullOrEmpty(pathNovel)) return;
            try
            {
                int chapterNumber = 1;
                pathNovel = PropertyExtension.CheckPathWeb(pathNovel);

                string html = pathNovel.DownloadStringWebClient();
                HtmlDocument htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);

                //get name of novel
                var nameNovel = htmlDoc.GetHtmlNode("title")?.InnerText;

                //get novel id
                var Idbook = htmlDoc.GetHtmlNode("input", "id", "bookId").Attributes["Value"].Value;

                //get signKey
                var signKey = htmlDoc.GetSignInKey();

                //get function fuzzy
                int numberFuzzy = htmlDoc.GetNumberFuzzy();
                var allChapter = await GetAllChapterLink(nameNovel, Idbook, signKey, numberFuzzy);

                //get path img of novel
                var imgNode = htmlDoc.GetHtmlNode("ing", "class", "z-depth-1 materialboxed");
                var imgPath = imgNode?.Attributes["src"].Value;
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

                allChapter.SingleForEach(RuntimeContext.numberBatch, nameNovel, pathFolder, GetContentChapter);

                var novel = new Novel
                {
                    Name = nameNovel,
                    Genre = gener,
                    NumberChapter = allChapter.Count().ToString(),
                    Author = author,
                    Description = description,
                    Path = pathFolder
                };
                if (!dicNovel.ContainsKey(nameNovel))
                {
                    dicNovel.Add(nameNovel, novel);
                }

                WriteFile.WriteFileTxt(novel.GetString());
                logger.Info($"End crawl data novel {nameNovel}");

            }
            catch (Exception ex)
            {
                logger.Error($"Error while crawl data novel {pathNovel}, msg: {ex}");
                chapterLog.Info(PropertyExtension.FormatErrorChapter(true, null, pathNovel, 0, null, null));
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
                return;
            }
            catch (Exception ex)
            {
                logger.Error($"Error while crawl data novel {nameNovel}, chapter: {chaper}, msg: {ex}");
                chapterLog.Info(PropertyExtension.FormatErrorChapter(false, nameNovel, null, chaper, pathChapter, pathSave));
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
