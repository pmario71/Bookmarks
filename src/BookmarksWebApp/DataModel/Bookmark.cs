using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Bookmarks.DataModel
{
    public class Bookmark
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("tags")]
        public string[] Tags { get; set; }
    }
}
