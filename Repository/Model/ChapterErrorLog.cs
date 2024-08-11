namespace Repository.Model
{
    public class ChapterErrorLog
    {
        public bool IsNovelError { get; set; }
        public string PathNovel { get; set; }
        public int ChapterNumber { get; set; }
        public string PathChapter {  get; set; }
        public string PathChapterLocal { get; set; }
    }
}
