using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Librometer.Model;
//using Librometer.ModelGlobal;
using Librometer.ServicesSQLCE.datas;

namespace Librometer.ServicesSQLCE.Mappers
{
    public static class BookmarkMapper
    {
        public static BookmarkModel ToBookmarkModel(this Bookmark global)
        {
            BookmarkModel bookmark = new BookmarkModel
            {
                Id = global.Id,
                Name = global.Name,
                ReaderPage = global.ReaderPage,
                ThumbImage = global.ThumbImage,
                CreationDate = global.CreationDate
            };

            return bookmark;
        }

        public static Bookmark ToBookmark(this BookmarkModel client)
        {
            Bookmark global = new Bookmark
            {
                Id = client.Id,
                Name = client.Name,
                ReaderPage = client.ReaderPage,
                ThumbImage = client.ThumbImage,
                CreationDate =client.CreationDate
            };

            return global;
        }
    }
}
