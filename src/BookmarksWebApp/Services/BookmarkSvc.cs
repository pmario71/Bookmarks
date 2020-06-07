using Bookmarks.DataModel;
using Bookmarks.DataModel.Filtering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Bookmarks.Services
{
    public class BookmarkSvc
    {
        private static BookmarkFilter _defaultEmptyFilterComponent = BookmarkFilter.Create(string.Empty);
        private Configuration _configuration;
        private readonly HttpClient _httpClient;
        private BookmarkFilter _filterComponent = _defaultEmptyFilterComponent;

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

        public string Filter { get; set; }

        public IEnumerable<Bookmark> FilteredBookmarks
        {
            get
            {
                return Config.Bookmarks.Where(bm => _filterComponent.FilterOnSearchString(bm));
            }
        }

        public void ApplyFilter()
        {
            _filterComponent = BookmarkFilter.Create(this.Filter);
        }

        public void ResetFilter()
        {
            this.Filter = string.Empty;
            _filterComponent = _defaultEmptyFilterComponent;
        }

        public Configuration Config => _configuration;
    }
}