using System.Net;
using System.Text;

namespace CrawlDataService.Common
{
    public static class NewWebClient
    {
        public static WebClient CreateWebClient(this WebClient webClient)
        {
            webClient = new WebClient();
            AddHeaderForWebClient(webClient);
            //ChangeProxy(webClient);
            return webClient;
        }

        public static void ChangeProxy(WebClient webClient)
        {
            WebProxy proxy = new WebProxy
            {
                Address = new Uri("47.74.40.128:7788"),
                BypassProxyOnLocal = false,

                // Nếu proxy cần xác thực
                //Credentials = new NetworkCredential(
                //    userName: "yourUsername",
                //    password: "yourPassword")
            };
            webClient.Proxy = proxy;
        }

        public static void AddHeaderForWebClient(WebClient webClient)
        {
            webClient.Headers["User-Agent"] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/127.0.0.0 Safari/537.36 Edg/127.0.0.0";
            webClient.Headers["Accept"] = "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7";
            webClient.Headers["Accept-Language"] = "en-US,en;q=0.9";
            //webClient.Headers["Accept-Encoding"] = "gzip, deflate, br, zstd";
            webClient.Headers["Upgrade-Insecure-Requests"] = "1";
            webClient.Headers["Sec-Fetch-User"] = "?1";
            webClient.Headers["Sec-Fetch-Site"] = "same-origin";
            webClient.Headers["Sec-Fetch-Mode"] = "navigate";
            webClient.Headers["Sec-Fetch-Dest"] = "document";
            webClient.Headers["Sec-Ch-Ua"] = "\"Not)A;Brand\";v=\"99\", \"Microsoft Edge\";v=\"127\", \"Chromium\";v=\"127\"";
            webClient.Headers["Sec-Ch-Ua-Platform"] = "\"Windows\"";
            webClient.Headers["Priority"] = "u=0, i";
            //webClient.Headers["Cookie"] = "_uidcms=1718323802903879915; __uidac=1838217a9958d22db5151cb95db333af; _ga=GA1.1.33780048.1718323803; __RC=31; __UF=-1; X2Wdo13iWT=1; webgl=6c172e9805e4e7b459a72adb8d6cdd85e07f0ec3; __R=2; __tb=0; _admchkCK=1; jiyakeji_uuid=80289c20-5667-11ef-9580-fb8f19809c3d; bs_onshow=1; __uif=__uid%3A711832380420265138%7C__create%3A1718323804; __gads=ID=0099f5ff8a4306d6:T=1723211570:RT=1723273170:S=ALNI_MZbDGNA-8o1H1FxqM-tkOim1UXxRQ; __gpi=UID=00000ebae268f0ec:T=1723211570:RT=1723273170:S=ALNI_MaYeRQL5yZLN5C6GvGB2Z6VduADTA; __eoi=ID=707ec67eef158a6a:T=1723211570:RT=1723273170:S=AA-AfjaiDkJcHeQChu8TZnwjfeS4; _ga_DSEYM6VX97=GS1.1.1723269805.7.1.1723273171.44.0.0; _ga_YCSZZTM0SE=GS1.1.1723269805.6.1.1723273171.44.0.0; _ga_EGJYZDHJEC=GS1.1.1723269805.5.1.1723273171.0.0.0; _ga_XBF5L66EJ2=GS1.1.1723269805.5.1.1723273171.44.1.1484453438; FCNEC=%5B%5B%22AKsRol9ZEBmCQ4ny4Z_l6q4mv70wHfVB6r3MeHER14X9qw1QRU5n1hIVUJObqbJus5NXmnjrl6d4mbiKPrUuYPewAU8YP91AVAsWdrdNdEtfHXHwZFmaNb4prROhD6ObPBVa8FLkbziU13a78o6hzM_tRVVeXIWBRg%3D%3D%22%5D%5D";
            webClient.Headers["Scheme"] = "https";
            webClient.Encoding = Encoding.UTF8;
        }
    }
}
