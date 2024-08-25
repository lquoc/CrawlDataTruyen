namespace Common
{
    public static class Constant
    {
        public static string Seperation = "#;";
        public static string PathWeb
        {
            get
            {
                switch (RuntimeContext.EnumWeb)
                {
                    case Repository.Enum.ListEnum.EnumWeb.Wikidich:
                        return "https://truyenwikidich.net";
                    case Repository.Enum.ListEnum.EnumWeb.DTruyen:
                        return "https://dtruyen.com/";
                    default:
                        return "";
                }
            }
        }
    }
}
