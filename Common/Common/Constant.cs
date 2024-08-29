namespace Common
{
    public static class Constant
    {
        public static string Seperation = "#;";
        public static string PathNovelWeb
        {
            get
            {
                switch (RuntimeContext.EnumWeb)
                {
                    case Repository.Enum.ListEnum.NovelWeb.Wikidich:
                        return "https://truyenwikidich.net";
                    case Repository.Enum.ListEnum.NovelWeb.DTruyen:
                        return "https://dtruyen.com/";
                    case Repository.Enum.ListEnum.NovelWeb.Nettruyen:
                        return "https://nettruyenviet.com/";
                    default:
                        return "";
                }
            }
        }
    }
}
