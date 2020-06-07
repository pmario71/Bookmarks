using Bookmarks.DataModel.Filtering;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Bookmarks.DataModel;

namespace BookmarksTests.DataModel.Filtering
{
    internal class BookmarkFilterTests
    {
        Bookmark _bookmark = new Bookmark()
        {
            Title = "The quick Brown Fox",
        };

        [TestCase(null, ExpectedResult = true, Description = "Support null or empty filter string.")]
        [TestCase("ick", ExpectedResult = true)]
        [TestCase("quick", ExpectedResult = true)]
        [TestCase("Quick", ExpectedResult = true)]
        [TestCase("???", ExpectedResult = false, Description = "Non matching token in title")]
        [TestCase("quickx", ExpectedResult = false, Description = "Non matching token in title")]
        public bool PartialMatch_over_title(string filterString)
        {
            var sut = BookmarkFilter.Create(filterString);

            return sut.FilterOnSearchString(_bookmark);
        }

        [TestCase("#tag")]
        [TestCase("#TAG")]
        [TestCase(" #tag")]
        [TestCase("#tag ")]
        public void FullMatch_over_tags(string filterString)
        {
            var sut = BookmarkFilter.Create(filterString);

            Bookmark _bookmark = new Bookmark()
            {
                Title = "The quick Brown Fox",
                Tags = new[] { "tag", "othertag" }
            };

            Assert.True(sut.FilterOnSearchString(_bookmark));
        }

        [TestCase("#tag #notexisting", ExpectedResult = false)]
        [TestCase("#othertag #tag", ExpectedResult = true)]
        public bool If_more_than_a_single_tag_specified_all_tags_must_match(string filterString)
        {
            var sut = BookmarkFilter.Create(filterString);

            Bookmark _bookmark = new Bookmark()
            {
                Title = "The quick Brown Fox",
                Tags = new[] { "tag", "othertag" }
            };

            return sut.FilterOnSearchString(_bookmark);
        }

        [Test]
        public void Throw_if_more_than_a_single_title_token_is_specified()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => BookmarkFilter.Create("test or die"));
        }
    }
}
