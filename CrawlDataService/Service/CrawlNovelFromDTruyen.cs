﻿using Common;
using Common.MultiThread;
using CrawlDataService.Service;
using HtmlAgilityPack;
using Microsoft.Extensions.DependencyInjection;
using Repository.Model;
using System.Text;
using System.Text.RegularExpressions;

namespace CrawlDataService
{
    public class CrawlNovelFromDTruyen : CrawlNovelSerivce
    {

        public override async Task StartCrawlData(int numberBatch, string pathSave, string pathSaveVoice, string pathSearch)
        {
            RuntimeContext.logger.Info("CrawlNovelFromDTruyen");
            var listPathNovel = GetLinksNovel(pathSearch);
            //check novel is dowloaded
            HashSet<string> novelCrawled = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            novelCrawled = ReadFile.ReadFileTxt();
            listPathNovel = listPathNovel.Where(x => !novelCrawled.Contains(x)).ToList();

            //start thread
            MultiThreadHelper.MultiThread(listPathNovel, numberBatch, pathSave, pathSaveVoice, StartGetInfoNovelAndChapter);

        }


        public async Task StartGetInfoNovelAndChapter(string pathNovel, string pathSave, string pathSaveVoice)
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
                var nameNovel = htmlDoc.GetHtmlNode("title")?.InnerText.Split("-")[0];

                //get tag novel detail
                var novelDetail = htmlDoc.GetHtmlNode("div", "id", "story-detail");

                //get author novel
                var getTagPAuthor = novelDetail?.GetHtmlNode("p", "class", "author");
                var author = getTagPAuthor?.GetHtmlNode("a", "itemprop", "author")?.InnerText;

                //get gener novel
                var getTagPGener = novelDetail?.GetHtmlNode("p", "class", "story_categories");
                var gener = getTagPGener?.GetListHtmlNode("a", "itemprop", "genre")?.Select(e => e.InnerText).JoinGender();

                //get status novel
                var getTagDivInfo = novelDetail?.GetHtmlNode("div", "class", "infos");
                var getListTagP = getTagDivInfo?.GetListHtmlNode("p");
                var status = getListTagP?.Where(e => e.Descendants("i").FirstOrDefault(i => i.Attributes.Contains("class") && i.Attributes["class"].Value.Contains("fa-star")) != null)?.FirstOrDefault()?.InnerText;

                //get img novel
                var getTagDivImg = novelDetail?.GetHtmlNode("div", "class", "thumb");
                var imgPath = getTagDivImg?.GetHtmlNode("img", "itemprop", "image")?.Attributes["src"].Value;

                //get discription novel
                var description = novelDetail?.GetHtmlNode("div", "class", "description")?.InnerText;



                var allChapter = GetAllLinkChapterInNovel(pathNovel);

                var pathFolder = WriteFile.CreateFolder(pathSave, nameNovel.RemoveDiacriticsAndSpaces());
                var pathFolderVoice = WriteFile.CreateFolder(pathSaveVoice, nameNovel.RemoveDiacriticsAndSpaces());

                allChapter.SingleForEach(RuntimeContext.numberBatch, nameNovel, pathFolder, pathFolderVoice, GetContentChapter);
                var novel = new Novel
                {
                    Name = nameNovel,
                    Genre = gener,
                    IsFull = status.GetStatus(),
                    NumberChapter = allChapter.Count().ToString(),
                    Author = author,
                    Description = description,
                    ImgPath = imgPath,
                    Path = pathFolder
                };
                WriteFile.WriteFileTxt(novel.GetString());
                logger.Info($"End crawl data novel {nameNovel}");
            }
            catch (Exception ex)
            {
                logger.Error($"Error while crawl data novel {pathNovel}, msg: {ex}");
                chapterLog.Info(PropertyExtension.FormatErrorChapter(true, null, pathNovel, 0, null, null));
            }
        }
        public List<string> GetAllLinkChapterInNovel(string pathNovel)
        {
            var links = new List<string>();
            if (string.IsNullOrEmpty(pathNovel)) return links;
            logger.Info($"Start crawl path chapter in novel: {pathNovel}");
            int i = 0;
            while (true)
            {
                i++;
                try
                {
                    logger.Info($"Start crawl path chapter in page: {i}");
                    var newPathSearch = pathNovel.Replace("//1", $"") + $"/{i}";
                    newPathSearch = PropertyExtension.CheckPathWeb(newPathSearch);
                    var html = newPathSearch.DownloadStringWebClient();
                    HtmlDocument htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(html);

                    //get link chapters in this page
                    var tagDivChapters = htmlDoc.GetHtmlNode("div", "id", "chapters");
                    var tagUlChapter = tagDivChapters?.GetHtmlNode("ul", "class", "chapters");
                    var listHrefValue = tagUlChapter?.GetListHtmlNode("a")?.Select(e => e.Attributes["href"].Value).ToList();
                    if (listHrefValue == null || listHrefValue?.Count == 0)
                    {
                        break;
                    }
                    links.AddRange(listHrefValue);
                    logger.Info($"End crawl novel in page: {i}");
                }
                catch (Exception ex)
                {
                    logger.Error($"Error while crawl path novel, msg: {ex}");
                }
            }
            return links;
        }

        public List<string> GetLinksNovel(string pathSearch)
        {
            var links = new List<string>();
            if (string.IsNullOrEmpty(pathSearch)) return links;
            logger.Info($"Start crawl path novel page: {pathSearch}");
            int i = 0;
            while (true)
            {
                i++;
                try
                {
                    logger.Info($"Start crawl novel in page: {i}");
                    var newPathSearch = pathSearch.Replace("//1", $"") + $"/{i}";
                    newPathSearch = PropertyExtension.CheckPathWeb(newPathSearch);
                    var html = newPathSearch.DownloadStringWebClient();
                    HtmlDocument htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(html);

                    //get all href novels of page
                    var listStoriesNode = htmlDoc.GetHtmlNode("div", "class", "list-stories");
                    var listTagLiNovelList = listStoriesNode?.GetListHtmlNode("li", "class", "story-list");
                    var listTagDivInfo = listTagLiNovelList?.GetListHtmlNode("div", "class", "info");
                    var listTagA = listTagDivInfo?.GetListHtmlNode("a", "itemprop", "url");
                    var listHrefValue = listTagA?.Select(e => e.Attributes["Href"].Value).ToList();
                    if (listHrefValue == null || listHrefValue?.Count == 0)
                    {
                        break;
                    }
                    links.AddRange(listHrefValue);
                    logger.Info($"End crawl novel in page: {i}");
                }
                catch (Exception ex)
                {
                    logger.Error($"Error while crawl path novel, msg: {ex}");
                }
            }
            return links;
        }

        public async Task GetContentChapter(string nameNovel, int chaper, string pathSave, string? pathChapter, string pathFolderVoice)
        {
            if (string.IsNullOrEmpty(pathChapter)) return;
            pathChapter = PropertyExtension.CheckPathWeb(pathChapter);
            try
            {
                logger.Info($"Start crawl novel:{nameNovel}, chapter: {chaper}");
                HtmlDocument htmlDoc = new HtmlDocument();
                var html = pathChapter.DownloadStringWebClient();
                htmlDoc.LoadHtml(html);

                //get tag div chapter-page
                var tagDivChapterPage = htmlDoc.GetHtmlNode("div", "id", "chapter-page");

                //get title of chapter
                var titleChapter = htmlDoc.DocumentNode.Descendants("title").FirstOrDefault()?.InnerText.Split("-")[1];

                //get tag div chapter
                var tagDivChapter = tagDivChapterPage?.GetHtmlNode("div", "id", "chapter");

                //get content of chapter
                var tagDivChapterContent = tagDivChapter?.GetHtmlNode("div", "id", "chapter-content");
                var contentHtml = tagDivChapterContent.InnerText;
                var content = GetContentFromHTML(tagDivChapterContent);
                WriteFile.WriteFileTxt(pathSave, titleChapter.RemoveDiacriticsAndSpaces(), content);
                if (RuntimeContext.IsChangeTextIntoVoice)
                {
                    var changeTextToVoiceSerivce = RuntimeContext._serviceProvider.GetRequiredService<ChangeTextToVoice>();
                    await changeTextToVoiceSerivce.RequestCreateSpeechGoogleCloudy(content, pathFolderVoice, titleChapter.RemoveDiacriticsAndSpaces(), chaper);
                }
                return;
            }
            catch (Exception ex)
            {
                logger.Error($"Error while crawl data novel {nameNovel}, chapter: {chaper}, msg: {ex}");
                chapterLog.Info(PropertyExtension.FormatErrorChapter(false, nameNovel, null, chaper, pathChapter, pathSave));
                return;
            }
        }

        private string GetContentFromHTML(HtmlNode? contentDoc)
        {
            if (contentDoc?.ChildNodes is null) return string.Empty;
            StringBuilder extractedText = new StringBuilder();
            foreach (var node in contentDoc?.ChildNodes)
            {
                if (node.Name == "br")
                {
                    extractedText.AppendLine();
                }
                else
                {
                    extractedText.Append(node.InnerText);
                }
            }
            string output = Regex.Replace(extractedText.ToString(), @"&[a-z]+;", match =>
            {
                return match.Value switch
                {
                    "&igrave;" => "ì",
                    "&ocirc;" => "ô",
                    "&agrave;" => "à",
                    "&eacute;" => "é",
                    "&ecirc;" => "ê",
                    "&aacute;" => "á",
                    "&uacute;" => "ú",
                    "&uuml;" => "ü",
                    "&otilde;" => "õ",
                    "&amp;" => "&",
                    "&lt;" => "<",
                    "&gt;" => ">",
                    "&quot;" => "\"",
                    "&apos;" => "'",
                    "<br/>" => "\n",
                    _ => match.Value
                };
            });
            return output;
        }

        public override async Task StartCrawlOnlyOneNovel(int numberBatch, string pathSave, string pathSaveVoice, string pathNovel)
        {

        }
    }
}
