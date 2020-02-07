using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace WebBlog.WebApi.Core.JsonModels
{
    public class PostHomeModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("creationTime")]
        public DateTime CreationTime { get; set; }

        [JsonProperty("lastModifiedTime")]
        public DateTime LastModifiedTime { get; set; }

        [JsonProperty("hasImage")]
        public bool HasImage { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("categoryId")]
        public int? CategoryId { get; set; }

        [JsonProperty("postImage")]
        public byte[] PostImage { get; set; }

        [JsonProperty("isConfirmed")]
        public bool IsConfirmed { get; set; }
    }
}