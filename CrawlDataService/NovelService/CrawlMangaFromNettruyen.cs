using Common;
using CrawlDataService.Service;
using HtmlAgilityPack;
using Repository.Model;

namespace CrawlDataService
{
    public class CrawlMangaFromNettruyen : CrawlNovelSerivce
    {
        public CrawlMangaFromNettruyen(IServiceProvider service)
        {
            IsManga = true;
        }

        public override List<string>? GetAllLinksChapter(string pathNovel)
        {
            if (string.IsNullOrEmpty(pathNovel) || !RuntimeContext.IsStart) return null;
            try
            {
                logger.Info($"Start crawl all links chapter");

                string html = pathNovel.DownloadStringWebClient();
                HtmlDocument htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);

                var tagArticle = htmlDoc.GetHtmlNode("article", "id", "item-detail");
                var tagDivListChapter = tagArticle?.GetHtmlNode("div", "id", "nt_listchapter");
                var tagNav = tagDivListChapter?.GetHtmlNode("nav");
                var tagUl = tagNav?.GetHtmlNode("ul", "id", "asc");
                var tagLiList = tagUl?.GetListHtmlNode("li");
                var listHref = tagLiList?.Select(e => e.GetHtmlNode("a")?.Attributes["href"].Value).ToList();
                logger.Info($"End crawl all links chapter");
                return listHref;
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
            if (string.IsNullOrEmpty(pathChapter) || !RuntimeContext.IsStart) return null;
            try
            {
                logger.Info($"Start crawl novel:{novel.Name}, chapter: {pathChapter}");
                HtmlDocument htmlDoc = new HtmlDocument();
                var html = pathChapter.DownloadStringWebClient();
                htmlDoc.LoadHtml(html);

                var tagDivCenter = htmlDoc.GetHtmlNode("div", "id", "ctl00_divCenter");

                //get title of chapter
                var titleChapter = tagDivCenter?.GetHtmlNode("h1", "class", "txt-primary")?.InnerText.Split("-")?[1].Trim();

                //get links image
                var tagDivReadingDetail = tagDivCenter?.GetHtmlNode("div", "class", "reading-detail");
                var tagDivPageChapters = tagDivReadingDetail?.GetListHtmlNode("div", "class", "page-chapter");
                var tagImages = tagDivPageChapters.GetListHtmlNode("img");
                var listLinkImage = tagImages.Select(e => e.Attributes["data-src"].Value).ToList();

                //create folder chapter
                var pathFolder = WriteFile.CreateFolder(novel.PathLocal.Trim(), titleChapter.RemoveDiacriticsAndSpaces());
                foreach (var image in listLinkImage)
                {
                    image.DownloadImageFakeInfoWeb(pathFolder, Constant.PathNovelWeb);
                }
                logger.Info($"End crawl novel:{novel.Name}, chapter: {pathChapter}");
                return new ChapterInfo
                {
                    ContentChapter = string.Empty,
                    TitleChapter = titleChapter
                };
            }
            catch (Exception ex)
            {
                logger.Error($"Error while crawl data novel {novel.Name}, chapter: {pathChapter}, msg: {ex}");
                chapterLog.Info(PropertyExtension.FormatErrorChapter(false, novel.Name, null, pathChapter, novel.PathLocal));
            }
            return null;
        }

        public override List<string>? GetLinksNovel(string pathSearch)
        {
            var links = new List<string>();
            if (string.IsNullOrEmpty(pathSearch) || !RuntimeContext.IsStart) return links;
            logger.Info($"Start crawl path novel page: {pathSearch}");
            int i = 0;
            int endPage = 0;
            while (true)
            {
                i++;
                try
                {
                    logger.Info($"Start crawl novel in page: {i}");
                    if (pathSearch.Contains("?page="))
                    {
                        pathSearch = pathSearch.Substring(0, pathSearch.IndexOf("?page=")) + $"?page={i}";
                    }
                    else
                    {
                        pathSearch = pathSearch + $"?page={i}";
                    }
                    var html = pathSearch.DownloadStringWebClient();
                    HtmlDocument htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(html);

                    //get all href novels of page
                    var tagMain = htmlDoc.GetHtmlNode("main", "class", "main");
                    var tagDivCenter = tagMain?.GetHtmlNode("div", "id", "ctl00_divCenter");
                    var tagDivItems = tagDivCenter?.GetHtmlNode("div", "class", "items");
                    var tagDivItemList = tagDivItems?.GetListHtmlNode("div", "class", "item");
                    var tagDivImageList = tagDivItemList?.GetListHtmlNode("div", "class", "image");
                    var tagAList = tagDivImageList?.GetListHtmlNode("a");
                    var listHrefValue = tagAList?.Select(e => e.Attributes["href"].Value).ToList();
                    links.AddRange(listHrefValue);
                    if (i == 1 && endPage == 0)
                    {
                        var tagDivPagination = tagDivCenter?.GetHtmlNode("div", "id", "ctl00_mainContent_ctl01_divPager");
                        var tagAListPagination = tagDivPagination?.GetListHtmlNode("a");
                        var listInt = tagAListPagination?.Where(e => int.TryParse(e.InnerText, out var number)).Select(e => int.Parse(e.InnerText)).ToList();
                        if (listInt != null && listInt.Any())
                        {
                            endPage = listInt.Max();
                        }
                    }
                    if (i == endPage || i > endPage) break;
                    logger.Info($"End crawl novel in page: {i}");
                }
                catch (Exception ex)
                {
                    logger.Error($"Error while crawl path novel, msg: {ex}");
                }
            }
            return links;
        }

        public override Task StartCrawlData(int numberBatch, string pathSave, string pathSaveVoice, string pathSearch)
        {
            throw new NotImplementedException();
        }

        public override async Task<Novel?> StartGetInfoNovel(string pathNovel, string pathSave, string pathSaveVoice)
        {

            if (string.IsNullOrEmpty(pathNovel) || !RuntimeContext.IsStart) return null;
            try
            {
                //pathNovel = PropertyExtension.CheckPathWeb(pathNovel);
                string html = pathNovel.DownloadStringWebClient();
                HtmlDocument htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);

                var tagArticle = htmlDoc.GetHtmlNode("article", "id", "item-detail");

                //get name of novel
                var nameNovel = tagArticle?.GetHtmlNode("h1", "class", "title-detail")?.InnerText;

                //get tag novel detail
                var detailInfo = tagArticle?.GetHtmlNode("div", "class", "detail-info");
                var tagUl = detailInfo?.GetHtmlNode("ul", "class", "list-info");

                //get author novel
                var getTagAuthor = tagUl?.GetHtmlNode("li", "class", "author");
                var author = getTagAuthor?.GetListHtmlNode("p")?[1].InnerText;

                //get gener novel
                var getTagGener = tagUl?.GetHtmlNode("li", "class", "kind");
                var gener = getTagGener?.GetListHtmlNode("p")?[1].GetListHtmlNode("a")?.Select(e => e.InnerText).JoinGender();

                //get status novel
                var getTagStatus = tagUl?.GetHtmlNode("li", "class", "status");
                var status = getTagStatus?.GetListHtmlNode("p")?[1].InnerText;

                //get img novel
                var getTagDivImg = detailInfo?.GetHtmlNode("div", "class", "col-image");
                var imgPath = getTagDivImg?.GetHtmlNode("img", "class", "image-thumb")?.Attributes["src"].Value;

                //get discription novel
                var getTagDivContent = detailInfo?.GetHtmlNode("div", "class", "detail-content");
                var description = getTagDivContent?.GetHtmlNode("div", "class", "shortened")?.InnerText;

                var pathFolder = WriteFile.CreateFolder(pathSave, nameNovel.RemoveDiacriticsAndSpaces());
                var pathFolderVoice = WriteFile.CreateFolder(pathSaveVoice, nameNovel.RemoveDiacriticsAndSpaces());
                var imgPathLocal = imgPath.DownloadImageFakeInfoWeb(pathFolder, Constant.PathNovelWeb);

                var novel = new Novel
                {
                    Name = nameNovel,
                    Genre = gener,
                    IsFull = status.GetStatus(),
                    Author = author,
                    Description = description,
                    ImgPathLocal = imgPathLocal,
                    VoiceOrMP4Path = pathFolderVoice,
                    PathLocal = pathFolder.Trim()
                };
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
    }
}
