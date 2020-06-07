using Bookmarks.DataModel.Filtering;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Bookmarks.DataModel;
using System.Linq;

namespace BookmarksTests.DataModel.Filtering
{

    internal class BookmarkFilterTokenizerTests
    {
        Bookmark _bookmark = new Bookmark()
        {
            Title = "The quick Brown Fox",
        };

        [Test]
        public void Title_is_extracted()
        {
            var (title, tags) = BookmarkFilter.Tokenize(" test");

            Assert.IsTrue(title.Contains("test"));
            CollectionAssert.IsEmpty(tags);
        }

        [Test]
        public void Tag_extracted()
        {
            var (title, tags) = BookmarkFilter.Tokenize(" #test");

            Assert.IsTrue(tags.Contains("test"));
            CollectionAssert.IsEmpty(title);
        }

        [Test]
        public void Mixed()
        {
            var (title, tags) = BookmarkFilter.Tokenize(" #test token");

            Assert.IsTrue(tags.Contains("test"));
            Assert.IsTrue(title.Contains("token"));

            Assert.AreEqual(1, title.Length);
            Assert.AreEqual(1, tags.Length);
        }
    }
}
