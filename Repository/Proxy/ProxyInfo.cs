using Newtonsoft.Json;

namespace Repository.Proxy
{
    public class ExtraData
    {
        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }
    }

    public class Data
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("key_code")]
        public string KeyCode { get; set; }

        [JsonProperty("key_type")]
        public int KeyType { get; set; }

        [JsonProperty("created_time")]
        public DateTime CreatedTime { get; set; }

        [JsonProperty("allow_ip")]
        public string AllowIp { get; set; }

        [JsonProperty("expired_time")]
        public DateTime ExpiredTime { get; set; }

        [JsonProperty("updated_time")]
        public DateTime UpdatedTime { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("server_port")]
        public int ServerPort { get; set; }

        [JsonProperty("last_use_time")]
        public object LastUseTime { get; set; }

        [JsonProperty("bandwidth_limit")]
        public long BandwidthLimit { get; set; }

        [JsonProperty("total_download")]
        public long TotalDownload { get; set; }

        [JsonProperty("total_upload")]
        public long TotalUpload { get; set; }

        [JsonProperty("priority_telco")]
        public object PriorityTelco { get; set; }

        [JsonProperty("priority_loc")]
        public object PriorityLoc { get; set; }

        [JsonProperty("reset_loop")]
        public int ResetLoop { get; set; }

        [JsonProperty("auto_extend")]
        public int AutoExtend { get; set; }

        [JsonProperty("family")]
        public string Family { get; set; }

        [JsonProperty("extra_data")]
        public ExtraData ExtraData { get; set; }

        [JsonProperty("user")]
        public string User { get; set; }

        [JsonProperty("proxy")]
        public string Proxy { get; set; }

        [JsonProperty("finished")]
        public bool Finished { get; set; }
    }

    public class ProxyInfo
    {
        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("data")]
        public List<Data> Data { get; set; }
    }
}
