using Common;
using CrawlDataService.Service;
using HtmlAgilityPack;
using Repository.Model;

namespace CrawlDataService;

public class CrawlDataFromTruyenQQ : CrawlNovelSerivce
{
    public override List<string>? GetAllLinksChapter(string pathNovel)
    {
        if (string.IsNullOrEmpty(pathNovel) || !RuntimeContext.IsStart) return null;
        try
        {
            logger.Info($"Start crawl all links chapter");

            string html = pathNovel.DownloadStringWebClient();
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var tagDivBookDetail = htmlDoc.GetHtmlNode("div", "class", "book_detail");
            var tagDivListChapter = tagDivBookDetail?.GetHtmlNode("div", "class", "list_chapter");
            var tagDivWorksChapter = tagDivListChapter?.GetHtmlNode("div","class", "works-chapter-list");
            var tagDivChapterItem = tagDivWorksChapter?.GetListHtmlNode("div", "class", "works-chapter-item");
            var tagA = tagDivChapterItem?.GetListHtmlNode("a", "target", "_self");
            var listHref = tagA?.Select(e => e.Attributes["href"].Value).ToList();
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

            var tagDivContent = htmlDoc.GetHtmlNode("div", "class", "content background-black");
            var tagDivMiddle = tagDivContent?.GetHtmlNode("div", "class", "div_middle");
            var tagDivMainContent = tagDivMiddle?.GetHtmlNode("div", "class", "main_content");
            var tagDivChapterContent = tagDivMainContent?.GetHtmlNode("div", "id", "chapter_content");

            //get title of chapter
            var titleChapter = tagDivChapterContent?.GetHtmlNode("h1", "class", "detail-title")?.InnerText.Split("-")?[1].Trim();

            //get links image
            var tagDivPageChapters = tagDivMainContent?.GetListHtmlNode("div", "class", "page-chapter");
            var tagImages = tagDivPageChapters.GetListHtmlNode("img");
            var listLinkImage = tagImages?.Select(e => e.Attributes["src"].Value).ToList();

            //create folder chapter
            var pathFolder = WriteFile.CreateFolder(novel?.PathLocal.Trim(), titleChapter.RemoveDiacriticsAndSpaces());
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

    public override List<string>? GetLinksNovel(string path)
    {
        throw new NotImplementedException();
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
            string html = pathNovel.DownloadStringWebClient();
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var tagDivBookDetail = htmlDoc.GetHtmlNode("div", "class", "book_detail");

            //get name of novel
            var tagLiName = tagDivBookDetail?.GetListHtmlNode("li", "itemprop", "itemListElement");
            var nameNovel = tagLiName?.Count >=2 ? tagLiName[1]?.GetHtmlNode("span", "itemprop", "name")?.InnerText : "";

            //get tag novel detail
            var tagDivBookInfo = htmlDoc.GetHtmlNode("div", "class", "book_info");
            var tagDivBookOther = tagDivBookInfo?.GetHtmlNode("div", "class", "book_other");
            var tagUl = tagDivBookInfo?.GetHtmlNode("ul", "class", "list-info");

            //get author novel
            var getTagAuthor = tagUl?.GetHtmlNode("li", "class", "author row");
            var author = getTagAuthor?.GetListHtmlNode("p")?[1].InnerText;

            //get gener novel
            var tagUlGener = tagDivBookOther?.GetHtmlNode("ul", "class", "list01");
            var tagLiGener = tagUlGener?.GetListHtmlNode("li");
            var gener = tagLiGener?.GetListHtmlNode("a")?.Select(e => e.InnerText).JoinGender();

            //get status novel
            var getTagStatus = tagUl?.GetHtmlNode("li", "class", "status row");
            var status = getTagStatus?.GetListHtmlNode("p")?[1].InnerText;

            //get img novel
            var tagDivBookAvatar = tagDivBookInfo?.GetHtmlNode("div", "class", "book_avatar");
            var imgPath = tagDivBookAvatar?.GetHtmlNode("img", "itemprop", "image")?.Attributes["src"].Value;

            //get discription novel
            var getTagDivDescription = tagDivBookOther?.GetHtmlNode("div", "class", "story-detail-info detail-content");
            var description = getTagDivDescription?.GetHtmlNode("p")?.InnerText;

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
