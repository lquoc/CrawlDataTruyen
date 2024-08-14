using Repository.Model;
using System.Text.RegularExpressions;

namespace Common
{
    public static class ReadFile
    {
        public static HashSet<string> ReadFileTxt()
        {
            var path = Path.Combine(RuntimeContext.PathSaveLocal, RuntimeContext.NameFileNovelList);
            HashSet<string> novelNames = new();
            if (File.Exists(path))
            {
                string[] logLines = File.ReadAllLines(path);
                foreach (string line in logLines)
                {
                    novelNames.Add(line);
                }
            }
            else
            {
                RuntimeContext.logger.Warn("Log file not found.");
            }
            return novelNames;
        }

        public static List<ChapterErrorLog> ReadFileLog(string logFilePath)
        {
            List<ChapterErrorLog> chapterErrors = new();
            if (File.Exists(logFilePath))
            {
                string[] logLines = File.ReadAllLines(logFilePath);
                foreach (string line in logLines)
                {
                    var chapterError = SlipLog(line);
                    if (chapterError != null)
                        chapterErrors.Add(SlipLog(line));
                }
            }
            else
            {
                RuntimeContext.logger.Warn("Log file not found.");
            }
            return chapterErrors;
        }

        private static ChapterErrorLog? SlipLog(string logLine)
        {
            //logLine = logLine.Replace(" ", "");
            string messagePattern = "\"Message\": \"(.*?)\"";
            var messageMatch = Regex.Match(logLine, messagePattern);
            if (messageMatch.Success)
            {
                string messageContent = messageMatch.Groups[1].Value;
                var chapterErrorLog = new ChapterErrorLog();

                var pairs = messageContent.Split(new[] { Constant.Seperation+" " }, StringSplitOptions.None);

                foreach (var pair in pairs)
                {
                    var keyValue = pair.Split(new[] { ": " }, StringSplitOptions.None);
                    if (keyValue.Length == 2)
                    {
                        string key = keyValue[0];
                        string value = keyValue[1];
                        switch (key)
                        {
                            case "IsNovelError":
                                chapterErrorLog.IsNovelError = bool.Parse(value);
                                break;
                            case "NovelName":
                                chapterErrorLog.NovelName = value;
                                break;
                            case "PathNovel":
                                chapterErrorLog.PathNovel = value;
                                break;
                            case "ChapterNumber":
                                chapterErrorLog.ChapterNumber = int.Parse(value);
                                break;
                            case "PathChapter":
                                chapterErrorLog.PathChapter = value;
                                break;
                            case "PathChapterLocal":
                                chapterErrorLog.PathChapterLocal = value;
                                break;
                        }
                    }
                }
                return chapterErrorLog;
            }
            return null;
        }

    }
}
