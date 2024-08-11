
using HtmlAgilityPack;
using System.Globalization;
using System.Text;

namespace Common
{
    public static class PropertyExtension
    {
        public static string JoinGender(this IEnumerable<string>? genders)
        {
            if (genders == null || genders.Count() == 0) return string.Empty;
            return string.Join(Constant.Seperation, genders);
        }
        public static HtmlNode? GetHtmlNode(this HtmlDocument htmlDoc, string tagName)
        {
            return htmlDoc.DocumentNode.Descendants(tagName).FirstOrDefault();
        }
        public static HtmlNode? GetHtmlNode(this HtmlDocument htmlDoc, string tagName, string attributeName, string attributeValue)
        {
            return htmlDoc.DocumentNode.Descendants(tagName).FirstOrDefault(e => e.Attributes.Contains(attributeName) && e.Attributes[attributeName].Value.Equals(attributeValue));
        }
        public static HtmlNode? GetHtmlNode(this HtmlNode htmlNote, string tagName, string attributeName, string attributeValue)
        {
            return htmlNote.Descendants(tagName).FirstOrDefault(e => e.Attributes.Contains(attributeName) && e.Attributes[attributeName].Value.Equals(attributeValue));
        }


        public static List<HtmlNode>? GetListHtmlNode(this HtmlDocument htmlDoc, string tagName)
        {
            return htmlDoc.DocumentNode.Descendants(tagName).ToList();
        }

        public static List<HtmlNode>? GetListHtmlNode(this HtmlDocument htmlDoc, string tagName, string attributeName, string attributeValue)
        {
            return htmlDoc.DocumentNode.Descendants(tagName).Where(e => e.Attributes.Contains(attributeName) && e.Attributes[attributeName].Value.Equals(attributeValue)).ToList();
        }
        public static List<HtmlNode>? GetListHtmlNode(this HtmlNode htmlNote, string tagName, string attributeName, string attributeValue)
        {
            return htmlNote.Descendants(tagName).Where(e => e.Attributes.Contains(attributeName) && e.Attributes[attributeName].Value.Equals(attributeValue)).ToList();
        }
        public static List<HtmlNode>? GetListHtmlNode(this List<HtmlNode> htmlNote, string tagName, string attributeName, string attributeValue)
        {
            return htmlNote.SelectMany(e => e.Descendants(tagName).Where(e => e.Attributes.Contains(attributeName) && e.Attributes[attributeName].Value.Equals(attributeValue))).ToList();
        }

        public static string RemoveDiacriticsAndSpaces(this string? name)
        {
            if (string.IsNullOrEmpty(name)) return string.Empty;
            var normalizedString = name.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            string resultWithoutDiacritics = stringBuilder.ToString().Normalize(NormalizationForm.FormC);

            //string resultWithoutSpaces = resultWithoutDiacritics.Replace(" ", "");

            return resultWithoutDiacritics;
        }

        public static string RemoveInvalidPathChars(this string nameFolder)
        {
            char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
            foreach (char invalidChar in invalidFileNameChars)
            {
                nameFolder = nameFolder.Replace(invalidChar.ToString(), "");
            }
            return nameFolder;
        }

        public static string CheckPathWeb(string path)
        {
            if (!path.Contains(Constant.PathWeb))
            {
                return path = $"{Constant.PathWeb}{path}";
            }          
            return path;
        }

        public static string FormatErrorChapter(bool isNovelError, string? pathNovel, int chapterNumber, string? pathChapter, string? pathChapterLocal)
        {
            return $"IsNovelError: {isNovelError}{Constant.Seperation} PathNovel: {pathNovel}{Constant.Seperation} ChapterNumber: {chapterNumber}{Constant.Seperation} PathChapter: {pathChapter} {Constant.Seperation} PathChapterLocal: {pathChapterLocal}";
        }

    }
}
