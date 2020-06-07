using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookmarks.DataModel.Filtering
{
    public sealed class BookmarkFilter
    {
        private readonly Func<Bookmark, bool> _titleFilter;
        private readonly Func<Bookmark,bool> _tagFilter;

        private BookmarkFilter(string filter) 
        {
            (_titleFilter, _tagFilter) = PrepareFilter(filter);
        }

        public static BookmarkFilter Create(string filter)
        {
            return new BookmarkFilter(filter);
        }

        public bool FilterOnSearchString(Bookmark bookmark)
        {
            return _titleFilter(bookmark) &&
                       _tagFilter(bookmark);
        }

        private (Func<Bookmark, bool> titleFilter, Func<Bookmark, bool> tagFilter) PrepareFilter(string filter)
        {
            Func<Bookmark, bool> titleFilter;
            Func<Bookmark, bool> tagFilter;

            if (string.IsNullOrEmpty(filter))
            {
                titleFilter = _ => true;
                tagFilter = _ => true;
            }
            else
            {
                var (filterTokens, tagTokens) = Tokenize(filter);

                if (filterTokens.Length > 1)
                {
                    throw new ArgumentOutOfRangeException("More than one title search criteria not supported!");
                }
                if (filterTokens.Any())
                {
                    titleFilter = bm => bm.Title.Contains(filterTokens[0], StringComparison.OrdinalIgnoreCase);
                }
                else
                {
                    titleFilter = _ => true;
                }

                if (tagTokens.Any())
                {
                    tagFilter = bm => AnyMatch(tagTokens, bm.Tags);
                }
                else
                {
                    tagFilter = _ => true;
                }
            }
            return (titleFilter, tagFilter);
        }

        private bool AnyMatch(IEnumerable<string> expectedTags, string[] existingTags)
        {
            foreach (var token in expectedTags)
            {
                if (!existingTags.Any(tag => string.Compare(tag, token, StringComparison.OrdinalIgnoreCase) == 0))
                {
                    return false;
                }
            }
            return true;
        }

        internal static (string[] titleFilter, string[] tagFilter) Tokenize(string filter)
        {
            var tokens = filter.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var titleTokens = new List<string>();
            var tagTokens = new List<string>();
            foreach (var token in tokens)
            {
                if (token[0] == '#')
                {
                    tagTokens.Add(token[1..]);
                }
                else
                {
                    titleTokens.Add(token);
                }
            }
            return (titleTokens.ToArray(), tagTokens.ToArray());
        }
    }
}
