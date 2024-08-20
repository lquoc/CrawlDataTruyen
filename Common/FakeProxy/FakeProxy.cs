using Repository.Proxy;
using System.Net;
using Newtonsoft.Json;
namespace Common.FakeProxy
{
    public static class FakeProxy
    {
        public static (bool, ProxyInfo) GetProxyInfo()
        {
            var isSuccess = true;
            string linkProxy = string.Format($"https://mproxy.vn/capi/{RuntimeContext.ProxyKey}/keys");
            WebClient webClient = new WebClient();
            var response = webClient.DownloadString(linkProxy);
            var apiResponse = JsonConvert.DeserializeObject<ProxyInfo>(response);
            if (!apiResponse?.Message.Equals("Thành công", StringComparison.OrdinalIgnoreCase) ?? true)
            {
                RuntimeContext.logger.Warn("Cannot get info if proxy from api key");
                isSuccess = false;
            }
            return (isSuccess, apiResponse);
        }
    }
}
