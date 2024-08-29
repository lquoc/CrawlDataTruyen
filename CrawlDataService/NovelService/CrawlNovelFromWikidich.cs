using Common;
using Common.MultiThread;
using HtmlAgilityPack;
using Microsoft.Extensions.DependencyInjection;
using Repository.Model;
using System.Text;

namespace CrawlDataService.Service
{
    public class CrawlNovelFromWikidich : CrawlNovelSerivce
    {

        int pageNovel = 1;
        public string pathIndex;
        readonly ChangeTextToVoice changeTextToVoiceService;
        readonly MP4Service mp4Service;
        public CrawlNovelFromWikidich(IServiceProvider service)
        {
            this.pathIndex = "https://truyenwikidich.net/book/index?bookId=";
            changeTextToVoiceService = service.GetRequiredService<ChangeTextToVoice>();
            mp4Service = service.GetRequiredService<MP4Service>();
        }

        public override List<string>? GetLinksNovel(string pathSearch)
        {
            var links = new List<string>();
            if (string.IsNullOrEmpty(pathSearch)) return links;
            logger.Info($"Start crawl link novel at path:{pathSearch}");
            try
            {
                pathSearch = PropertyExtension.CheckPathWeb(pathSearch);
                var html = pathSearch.DownloadStringWebClient();
                HtmlDocument htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);
                //get href novels of page
                var bookListNode = htmlDoc.GetListHtmlNode("div", "class", "book-list");
                var infoCol = bookListNode?.GetListHtmlNode("div", "class", "info-col");
                var pathHrefs = infoCol?.GetListHtmlNode("a", "class", "tooltipped");
                var pathNovels = pathHrefs?.Select(e => e.Attributes["href"].Value).ToList();
                if (pathNovels is null || pathNovels.Count == 0) return links;
                links.AddRange(pathNovels);
                logger.Info($"End crawl link novel at path:{pathSearch}");
                //get href next page
                //var pageNode = htmlDoc.GetHtmlNode("ul", "class", "pagination");
                //var effectNodes = pageNode?.GetListHtmlNode("li", "class", "waves-effect");
                //var hrefLink = effectNodes?.Where(e => e.GetListHtmlNode("i", "class", "fa fa-angle-right").Any()).Select(e => e.Descendants("a").FirstOrDefault().Attributes["href"].Value).FirstOrDefault();
            }
            catch (Exception ex)
            {
                logger.Error($"Error while crawl path novel, msg: {ex}");
            }
            return links;
        }

        public override async Task StartCrawlData(int numberBatch, string pathSave, string pathSaveVoice, string pathSearch)
        {
            try
            {
                var listPathNovel = GetLinksNovel(pathSearch);
                HashSet<string> novelCrawled = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                novelCrawled = ReadFile.ReadFileTxt();
                listPathNovel = listPathNovel?.Where(x => !novelCrawled.Contains(x)).ToList();
                MultiThreadHelper.MultiThread(listPathNovel, numberBatch, pathSave, pathSaveVoice, StartGetInfoNovel);
                //WriteFile.WriteFileXLSX(pathSave, $"Report_{DateTime.Now.ToString("ddMMyyyyHHmmss")}.xlsx", dicNovel);
            }
            catch (Exception ex)
            {
                logger.Error($"Error while in multithread, msg: {ex}");
            }
        }

        //todo
        public async Task StartReCrawData(int numberBatch, List<ChapterErrorLog> novelError, List<ChapterErrorLog> chapterError, string pathSave, string pathSaveVoice)
        {
            //if (novelError?.Count > 0)
            //{
            //    try
            //    {
            //        MultiThreadHelper.MultiThread(novelError.Select(x => x.PathNovel).ToList(), numberBatch, pathSave, pathSaveVoice, GetDataNovel);
            //    }
            //    catch (Exception ex)
            //    {
            //        logger.Error($"Error while in setup multithread to ReCrawl Novel, msg: {ex}");
            //    }
            //}
            //if (chapterError?.Count > 0)
            //{
            //    logger.Info("Start ReCrawl Chapter Error");
            //    try
            //    {
            //        chapterError.MultiThread(numberBatch, RuntimeContext.PathSaveFileMp3, GetContentChapter);
            //    }
            //    catch (Exception ex)
            //    {
            //        logger.Error($"Error while in setup multithread to ReCrawl chapter, msg: {ex}");

            //    }
            //}

        }

        public override List<string>? GetAllLinksChapter(string pathNovel)
        {
            #region info to get allchapter link
            pathNovel = PropertyExtension.CheckPathWeb(pathNovel);
            string html = pathNovel.DownloadStringWebClient();
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            //get name of novel
            var nameNovel = htmlDoc.GetHtmlNode("title")?.InnerText;
            //get novel id
            var Idbook = htmlDoc.GetHtmlNode("input", "id", "bookId")?.Attributes["Value"].Value;
            //get signKey
            var signKey = htmlDoc.GetSignInKey();
            //get function fuzzy
            int numberFuzzy = htmlDoc.GetNumberFuzzy();
            #endregion

            var startChapter = 0;
            var size = 501;
            var isTrue = true;
            var lastChapter = 0;
            List<string> allChapterPath = new();
            while (isTrue)
            {
                logger.Info($"Start get chapter of novel name:{nameNovel}, start:{startChapter}, size: {size}");
                //create sign value and path index.
                var sign = (signKey + startChapter.ToString() + size.ToString()).FuzzySign(numberFuzzy).SignFunc();
                var linkIndexChapter = $"{pathIndex}{Idbook}&start={startChapter}&size={size}&signKey={signKey}&sign={sign}";
                string htmlChapter = "";
                try
                {

                    htmlChapter = linkIndexChapter.DownloadStringWebClient();
                    HtmlDocument htmlChapterDoc = new HtmlDocument();
                    htmlChapterDoc.LoadHtml(htmlChapter);
                    //get link of chapter
                    var listTagLi = htmlChapterDoc.GetListHtmlNode("li", "class", "chapter-name");
                    var listTagA = listTagLi?.GetListHtmlNode("a", "class", "truncate");
                    var getHrefValue = listTagA?.Select(e => e.Attributes["href"].Value).ToList();

                    //get index last chapter
                    if (lastChapter == 0)
                    {
                        var pageNode = htmlChapterDoc.GetHtmlNode("ul", "class", "pagination");
                        var effectNodes = pageNode?.GetListHtmlNode("li", "class", "waves-effect");
                        effectNodes?.AddRange(pageNode?.GetListHtmlNode("li", "class", " waves-effect "));
                        var numberPathOfEffects = effectNodes?.Select(e => e.GetHtmlNode("a", "data-action", "loadBookIndex")?.Attributes["data-start"].Value).ToList();
                        var lastPage = numberPathOfEffects?.Where(e => !string.IsNullOrEmpty(e)).Select(e => int.Parse(e)).ToList().Max();
                        if (lastPage != null)
                            lastChapter = lastPage.Value;
                    }
                    allChapterPath.AddRange(getHrefValue);
                    logger.Info($"End get chapter of novel id:{nameNovel}, start:{startChapter}, size: {size}");
                }
                catch (Exception ex)
                {
                    logger.Error($"Error while get path chapter of novel {nameNovel}");
                }

                if (string.IsNullOrEmpty(html) || startChapter == lastChapter)
                {
                    isTrue = false;
                    logger.Info($"End get chapter of novel id:{Idbook}");
                }
                else
                {
                    startChapter += size;
                }
            }
            return allChapterPath;
        }

        public override async Task<Novel?> StartGetInfoNovel(string pathNovel, string pathSave, string pathSaveVoice)
        {
            if (string.IsNullOrEmpty(pathNovel)) return null;
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
                var Idbook = htmlDoc.GetHtmlNode("input", "id", "bookId")?.Attributes["Value"].Value;

                //get signKey
                var signKey = htmlDoc.GetSignInKey();

                //get function fuzzy
                int numberFuzzy = htmlDoc.GetNumberFuzzy();
                //var allChapter = await GetAllChapterLink(nameNovel, Idbook, signKey, numberFuzzy);

                //get path img of novel
                var imgNode = htmlDoc.GetHtmlNode("img", "class", "z-depth-1 materialboxed");
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
                var pathFolderVoice = WriteFile.CreateFolder(pathSaveVoice, nameNovel.RemoveDiacriticsAndSpaces());

                var imgPathLocal = (Constant.PathNovelWeb + imgPath).DownloadImgage(pathFolder);


                var novel = new Novel
                {
                    Name = nameNovel,
                    Genre = gener,
                    //IsFull = status.GetStatus(),
                    //NumberChapter = allChapter.Count().ToString(),
                    Author = author,
                    Description = description,
                    ImgPathLocal = imgPathLocal,
                    VoiceOrMP4Path = pathFolderVoice,
                    PathLocal = pathFolder
                };

                //if (RuntimeContext.TypeCrawl == Repository.Enum.ListEnum.TypeCrawl.MultiNovel)
                //{
                //    allChapter.SingleForEach(novel, GetContentChapter);
                //}
                //else if (RuntimeContext.TypeCrawl == Repository.Enum.ListEnum.TypeCrawl.OneNovel)
                //{
                //    allChapter.MultiThreadParralle((allChapter.Count / RuntimeContext.MaxThread) + RuntimeContext.MaxThread, novel, GetContentChapter);
                //}

                WriteFile.WriteFileTxt(novel.GetString());
                logger.Info($"End crawl data novel {nameNovel}");
                return novel;
            }
            catch (Exception ex)
            {
                logger.Error($"Error while crawl data novel {pathNovel}, msg: {ex}");
                chapterLog.Info(PropertyExtension.FormatErrorChapter(true, null, pathNovel, null, null));
            }
            return null;
        }

        public override async Task<ChapterInfo?> GetContentChapter(Novel novel, string? pathChapter)
        {

            if (string.IsNullOrEmpty(pathChapter)) return null;
            pathChapter = PropertyExtension.CheckPathWeb(pathChapter);
            //if (chaper % 50 == 0) Thread.Sleep(25000);
            string[] titleChapters = new string[10];
            string titleChapter;
            try
            {
                logger.Info($"Start crawl novel:{novel.Name}, chapter: {pathChapter}");
                HtmlDocument htmlDoc = new HtmlDocument();
                var html = pathChapter.DownloadStringWebClient();
                htmlDoc.LoadHtml(html);

                //get title of chapter
                titleChapter = htmlDoc.DocumentNode.Descendants("title").FirstOrDefault()?.InnerText.FirstSplit("-");

                //get content of chapter 
                var contentDoc = htmlDoc.GetHtmlNode("div", "class", "content-body-wrapper");
                var contentNodes = contentDoc?.GetHtmlNode("div", "id", "bookContentBody");
                var content = GetContentFromHTML(contentNodes);
                return new ChapterInfo
                {
                    TitleChapter = titleChapter,
                    ContentChapter = content
                };
            }
            catch (Exception ex)
            {
                logger.Error($"Error while crawl data novel {novel.Name}, chapter: {titleChapters}, msg: {ex}");
                chapterLog.Info(PropertyExtension.FormatErrorChapter(false, novel.Name, null, pathChapter, novel.PathLocal));
            }
            return null;
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
