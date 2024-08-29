using Common;
using Common.FakeProxy;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using System.Globalization;
using System.Net;
using System.Text;

namespace CrawlDataService.Common
{
    public static class NewWebClient
    {
        public static WebClient CreateWebClient(this WebClient webClient, bool isChangeProxy = true)
        {
            webClient = new WebClient();
            AddHeaderForWebClient(webClient);
            if (RuntimeContext.IsChangeProxy && isChangeProxy)
            {
                ChangeProxy(webClient);
            }
            return webClient;
        }

        public static IWebDriver GetInstantWebDriver()
        {
            EdgeOptions options = new EdgeOptions();
            options.AddArgument("--headless");
            options.AddArgument("--log-level=3");
            options.AddArgument("--silent");
            return new EdgeDriver(options);
        }

        public static string GetWebBySelenium(this IWebDriver driver, string path)
        {
            driver.Navigate().GoToUrl(path);
            return driver.PageSource;
        }


        public static void ChangeProxy(WebClient webClient)
        {
            try
            {
                var (isSuccess,proxyInfo) = FakeProxy.GetProxyInfo();
                if (isSuccess)
                {
                    WebProxy proxy = new WebProxy($"https://{proxyInfo.Data[0].Proxy}", true)
                    {
                        // Nếu proxy cần xác thực
                        Credentials = new NetworkCredential(
                            userName: proxyInfo.Data[0].User,
                            password: proxyInfo.Data[0].KeyCode)
                    };
                    webClient.Proxy = proxy;
                }
            }
            catch (Exception ex)
            {
                RuntimeContext.logger.Error($"Cannot change proxy, msg {ex}");
            }
        }

        public static void AddHeaderForWebClient(WebClient webClient)
        {
            webClient.Headers["User-Agent"] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/127.0.0.0 Safari/537.36 Edg/127.0.0.0";
            webClient.Headers["Accept"] = "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7";
            webClient.Headers["Accept-Language"] = "en-US,en;q=0.9";
            webClient.Headers["Upgrade-Insecure-Requests"] = "1";
            webClient.Headers["Sec-Fetch-User"] = "?1";
            webClient.Headers["Sec-Fetch-Site"] = "same-origin";
            webClient.Headers["Sec-Fetch-Mode"] = "navigate";
            webClient.Headers["Sec-Fetch-Dest"] = "document";
            webClient.Headers["Sec-Ch-Ua"] = "\"Not)A;Brand\";v=\"99\", \"Microsoft Edge\";v=\"127\", \"Chromium\";v=\"127\"";
            webClient.Headers["Sec-Ch-Ua-Platform"] = "\"Windows\"";
            webClient.Headers["Priority"] = "u=0, i";
            webClient.Headers["Scheme"] = "https";
            //webClient.Headers["Cache-Control"] = "max-age=0";
            //webClient.Headers["If-Modified-Since"] = DateTime.UtcNow.ToString("ddd, dd MMM yyyy HH:mm:ss 'GMT'", CultureInfo.InvariantCulture);
            webClient.Encoding = Encoding.UTF8;
        }
    }
}
