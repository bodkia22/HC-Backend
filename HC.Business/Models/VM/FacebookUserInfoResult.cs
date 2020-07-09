using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace HC.Business.Models.VM
{
    public partial class FacebookUserInfoResult
    {
        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("picture")]
        public FacebookPicture Picture { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public partial class FacebookPicture
    {
        [JsonProperty("data")]
        public FacebookPictureDate Data { get; set; }
    }

    public partial class FacebookPictureDate
    {
        [JsonProperty("height")]
        public long Height { get; set; }

        [JsonProperty("is_silhouette")]
        public bool IsSilhouette { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("width")]
        public long Width { get; set; }
    }
}
