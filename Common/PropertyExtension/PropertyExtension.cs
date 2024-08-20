using CrawlDataService.Common;
using HtmlAgilityPack;
using Jint;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
namespace Common
{
    public static class PropertyExtension
    {
        static HashSet<string> IsFullString = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "Hoàn Thành"
        };
        public static string JoinGender(this IEnumerable<string>? genders)
        {
            if (genders == null || genders.Count() == 0) return string.Empty;
            return string.Join(Constant.Seperation, genders);
        }
        public static bool GetStatus(this string? status)
        {
            if (string.IsNullOrEmpty(status)) return false;
            if (IsFullString.Contains(status))
            {
                return true;
            }
            return false;
        }


        //support html 
        public static HtmlNode? GetHtmlNode(this HtmlDocument htmlDoc, string tagName)
        {
            return htmlDoc.DocumentNode.Descendants(tagName).FirstOrDefault();
        }
        public static HtmlNode? GetHtmlNode(this HtmlNode htmlDoc, string tagName)
        {
            return htmlDoc.Descendants(tagName).FirstOrDefault();
        }
        public static List<HtmlNode>? GetListHtmlNode(this HtmlNode htmlDoc, string tagName)
        {
            return htmlDoc.Descendants(tagName).ToList();
        }
        public static HtmlNode? GetHtmlNode(this HtmlDocument htmlDoc, string tagName, string attributeName, string attributeValue)
        {
            return htmlDoc.DocumentNode.Descendants(tagName).FirstOrDefault(e => e.Attributes.Contains(attributeName) && e.Attributes[attributeName].Value.Contains(attributeValue));
        }
        public static HtmlNode? GetHtmlNode(this HtmlNode htmlNote, string tagName, string attributeName, string attributeValue)
        {
            return htmlNote.Descendants(tagName).FirstOrDefault(e => e.Attributes.Contains(attributeName) && e.Attributes[attributeName].Value.Contains(attributeValue));
        }


        public static List<HtmlNode>? GetListHtmlNode(this HtmlDocument htmlDoc, string tagName)
        {
            return htmlDoc.DocumentNode.Descendants(tagName).ToList();
        }

        public static List<HtmlNode>? GetListHtmlNode(this HtmlDocument htmlDoc, string tagName, string attributeName, string attributeValue)
        {
            return htmlDoc.DocumentNode.Descendants(tagName).Where(e => e.Attributes.Contains(attributeName) && e.Attributes[attributeName].Value.Contains(attributeValue)).ToList();
        }
        public static List<HtmlNode>? GetListHtmlNode(this HtmlNode htmlNote, string tagName, string attributeName, string attributeValue)
        {
            return htmlNote.Descendants(tagName).Where(e => e.Attributes.Contains(attributeName) && e.Attributes[attributeName].Value.Contains(attributeValue)).ToList();
        }
        public static List<HtmlNode>? GetListHtmlNode(this List<HtmlNode> htmlNote, string tagName, string attributeName, string attributeValue)
        {
            return htmlNote.SelectMany(e => e.Descendants(tagName).Where(e => e.Attributes.Contains(attributeName) && e.Attributes[attributeName].Value.Contains(attributeValue))).ToList();
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

        public static string FormatErrorChapter(bool isNovelError, string novelName, string? pathNovel, int chapterNumber, string? pathChapter, string? pathChapterLocal)
        {
            return $"IsNovelError: {isNovelError}{Constant.Seperation} NovelName: {novelName}{Constant.Seperation} PathNovel: {pathNovel}{Constant.Seperation} ChapterNumber: {chapterNumber}{Constant.Seperation} PathChapter: {pathChapter} {Constant.Seperation} PathChapterLocal: {pathChapterLocal}";
        }


        public static string DownloadStringWeb(this string path)
        {
            WebClient webClient = new WebClient();
            return webClient.CreateWebClient().DownloadString(path);
        }

        public static string DownloadStringWebClient(this string path)
        {
            var html = "";
            int i = 0;
            do
            {
                try
                {
                    Thread.Sleep(1000);
                    i++;
                    if (i == 10)
                    {
                        RuntimeContext.logger.Error($"Cannot found paht: {path}");
                        break;
                    }
                    WebClient webClient = new WebClient();
                    html = webClient.CreateWebClient().DownloadString(path);
                }
                catch (Exception ex)
                {
                    RuntimeContext.logger.Warn($"Sleep {2 * RuntimeContext.MaxThread}s, msg: {ex.Message}");
                    Thread.Sleep(2000 * RuntimeContext.MaxThread);
                }
            } while (string.IsNullOrEmpty(html));
            return html;
        }

        public static string DownloadImgage(this string imgUrl, string outPath)
        {
            if (!Directory.Exists(outPath))
            {
                Directory.CreateDirectory(outPath.Replace("\\\\", "\\"));
            }
            var pathCombine = Path.Combine(outPath.Trim(), Path.GetFileName(imgUrl).RemoveInvalidPathChars());
            try
            {
                WebClient webClient = new WebClient();
                webClient.CreateWebClient().DownloadFile(imgUrl, pathCombine);
                RuntimeContext.logger.Info($"Image downloaded and saved to {outPath}");
            }
            catch (Exception ex)
            {
                RuntimeContext.logger.Error($"An error occurred: {ex.Message}");
            }
            return pathCombine;
        }
        public static Image ResizeImageToEvenDimensions(this string inputPath)
        {
            using (var image = Image.FromFile(inputPath))
            {
                // Lấy kích thước hiện tại của ảnh
                int width = image.Width;
                int height = image.Height;

                // Đảm bảo kích thước chia hết cho 2
                int newWidth = (width % 2 == 0) ? width : width + 1;
                int newHeight = (height % 2 == 0) ? height : height + 1;

                // Tạo một Bitmap mới với kích thước đã điều chỉnh
                var resizedImage = new Bitmap(newWidth, newHeight);

                using (var graphics = Graphics.FromImage(resizedImage))
                {
                    // Vẽ hình ảnh gốc vào hình ảnh mới với kích thước đã điều chỉnh
                    graphics.DrawImage(image, 0, 0, newWidth, newHeight);
                }
                return resizedImage;
            }
        }
        public static void ResizeImageToEvenDimensions(Image img, string outputPath)
        {
            using (var image = img)
            {
                int width = image.Width;
                int height = image.Height;
                int newWidth = (width % 2 == 0) ? width : width + 1;
                int newHeight = (height % 2 == 0) ? height : height + 1;
                using (var resizedImage = new Bitmap(image, newWidth, newHeight))
                {
                    resizedImage.Save(outputPath);
                }
            }
        }


        public static string GetSignInKey(this HtmlDocument htmlDoc)
        {
            var scriptNodes = htmlDoc.DocumentNode.SelectNodes("//script");
            string signKey = null;
            foreach (var scriptNode in scriptNodes)
            {
                var scriptContent = scriptNode.InnerText;
                var keyIndex = scriptContent.IndexOf("signKey = \"");
                if (keyIndex != -1)
                {
                    // Find the start and end of the signKey value
                    var start = keyIndex + "signKey = \"".Length;
                    var end = scriptContent.IndexOf("\"", start);
                    signKey = scriptContent.Substring(start, end - start);
                    break;
                }
            }
            return signKey;
        }
        public static string FuzzySign(this string text, int numberFuzzy)
        {
            string result = text.Substring(numberFuzzy) + text.Substring(0, numberFuzzy);
            return result;
        }

        public static string SignFunc(this string text)
        {
            var engine = new Engine();
            string script = @"
            var signFunc = function a(W) {
                function V(d, c) {
                    return d >>> c | d << 32 - c
                }
                for (var U, T, S = Math.pow, R = S(2, 32), Q = 'length', P = '', O = [], N = 8 * W[Q], M = a.h = a.h || [], L = a.k = a.k || [], K = L[Q], J = {}, I = 2; 64 > K; I++) {
                    if (!J[I]) {
                        for (U = 0; 313 > U; U += I) {
                            J[U] = I
                        }
                        M[K] = S(I, 0.5) * R | 0,
                        L[K++] = S(I, 1 / 3) * R | 0
                    }
                }
                for (W += '\x80'; W[Q] % 64 - 56; ) {
                    W += '\x00'
                }
                for (U = 0; U < W[Q]; U++) {
                    if (T = W.charCodeAt(U),
                    T >> 8) {
                        return
                    }
                    O[U >> 2] |= T << (3 - U) % 4 * 8
                }
                for (O[O[Q]] = N / R | 0,
                O[O[Q]] = N,
                T = 0; T < O[Q]; ) {
                    var H = O.slice(T, T += 16)
                      , G = M;
                    for (M = M.slice(0, 8),
                    U = 0; 64 > U; U++) {
                        var F = H[U - 15]
                          , E = H[U - 2]
                          , D = M[0]
                          , C = M[4]
                          , B = M[7] + (V(C, 6) ^ V(C, 11) ^ V(C, 25)) + (C & M[5] ^ ~C & M[6]) + L[U] + (H[U] = 16 > U ? H[U] : H[U - 16] + (V(F, 7) ^ V(F, 18) ^ F >>> 3) + H[U - 7] + (V(E, 17) ^ V(E, 19) ^ E >>> 10) | 0)
                          , A = (V(D, 2) ^ V(D, 13) ^ V(D, 22)) + (D & M[1] ^ D & M[2] ^ M[1] & M[2]);
                        M = [B + A | 0].concat(M),
                        M[4] = M[4] + B | 0
                    }
                    for (U = 0; 8 > U; U++) {
                        M[U] = M[U] + G[U] | 0
                    }
                }
                for (U = 0; 8 > U; U++) {
                    for (T = 3; T + 1; T--) {
                        var z = M[U] >> 8 * T & 255;
                        P += (16 > z ? 0 : '') + z.toString(16)
                    }
                }
                return P
            };";
            engine.Execute(script);
            var result = engine.Invoke("signFunc", text).AsString();
            return result;

        }


        public static int GetNumberFuzzy(this HtmlDocument htmlDoc)
        {
            var scriptNodes = htmlDoc.DocumentNode.SelectNodes("//script");
            foreach (var scriptNode in scriptNodes)
            {
                string scriptContent = scriptNode.InnerHtml;
                var match = Regex.Match(scriptContent, @"fuzzySign\s*\(text\)\s*{\s*return\s+text\.substring\((\d+)\)");
                if (match.Success)
                {
                    string value = match.Groups[1].Value;
                    if (int.TryParse(value, out var numberFuzzy))
                        return numberFuzzy;
                }
            }
            return 0;
        }
    }
}
