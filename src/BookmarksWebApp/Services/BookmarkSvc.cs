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

        /// <summary>
        /// current raw filter string
        /// </summary>
        public string Filter { get; set; }

        /// <summary>
        /// filtered list of bookmarks
        /// </summary>
        public IEnumerable<Bookmark> FilteredBookmarks
        {
            get
            {
                return _configuration.Bookmarks.Where(bm => _filterComponent.FilterOnSearchString(bm));
            }
        }

        /// <summary>
        /// applies current filterstring and updates filtered list of bookmarks.
        /// </summary>
        public void ApplyFilter()
        {
            _filterComponent = BookmarkFilter.Create(this.Filter);
        }

        /// <summary>
        /// resets filter to show unfiltered list.
        /// </summary>
        public void ResetFilter()
        {
            this.Filter = string.Empty;
            _filterComponent = _defaultEmptyFilterComponent;
        }

        /// <summary>
        /// returns false as long as data model is not initialized
        /// </summary>
        public bool InitializationPending => _configuration == null;
    }
}