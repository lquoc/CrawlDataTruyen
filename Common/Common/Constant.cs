namespace Common
{
    public static class Constant
    {
        public static string Seperation = "#;";
        public static string PathWeb
        {
            get
            {
                switch (RuntimeContext.EnumPage)
                {
                    case Repository.Enum.ListEnum.EnumPage.WikiDich:
                        return "https://truyenwikidich.net";
                    case Repository.Enum.ListEnum.EnumPage.DTruyen:
                        return "https://dtruyen.com/";
                    default:
                        return "";
                }
            }
        }
    }
}
