using Bookmarks.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Bookmarks.Services
{
    public class BookmarkSvc
    {
        private Configuration _configuration;
        private readonly HttpClient _httpClient;

        public BookmarkSvc(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }

        public async Task InitializeAsync()
        {
            if (_configuration == null)
            {
                _configuration = await _httpClient.GetFromJsonAsync<Configuration>("sample-data/bookmarks.json");
            }
        }

        public Configuration Config => _configuration;
    }
}
