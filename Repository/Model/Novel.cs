
namespace Repository.Model
{
    public class Novel
    {
        public string Name { get; set; }
        public string Genre { get; set; }
        public string NumberChapter { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public string ImgPath { get; set; }
        public string Path { get; set; }


        public string GetString()
        {
            return $"{Name}|{Genre}|{NumberChapter}|{Author}|{Description}|{ImgPath}|{Path}";
        }

    }
}
