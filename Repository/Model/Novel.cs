
namespace Repository.Model
{
    public class Novel
    {
        public string? Name { get; set; }
        public string? Genre { get; set; }
        public string? NumberChapter { get; set; }
        public string? Author { get; set; }
        public string? Description { get; set; }
        public string? PathLocal { get; set; }
        public bool IsFull { get; set; } = false;
        public string? ImgPathLocal { get; set; }
        public string? VoiceOrMP4Path { get; set; }

        public string GetString()
        {
            return $"{Name}|{Genre}|{IsFull}|{NumberChapter}|{Author}|{Description}|{ImgPathLocal}|{PathLocal}|{VoiceOrMP4Path}";
        }

    }
}
