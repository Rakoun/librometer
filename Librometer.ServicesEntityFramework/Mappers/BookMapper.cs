using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Librometer.Model;
//using Librometer.ModelGlobal;
using Librometer.ServicesSQLCE.datas;

namespace Librometer.ServicesSQLCE.Mappers
{
    public static class BookMapper
    {
        public static BookModel ToBookModel(this Book global)
        {
            BookModel book = new BookModel
            {
                Cover = global.Cover,
                Editor = global.Editor,
                IdAuthor = global.IdAuthor,
                Id = global.Id,
                IdCategory = global.IdCategory,
                ISBN = global.ISBN,
                Rate = global.Rate,
                Title = global.Title
            };

            return book;
        }

        public static Book ToBook(this BookModel client)
        {
            Book global = new Book
            {
                Cover = client.Cover,
                Editor = client.Editor,
                IdAuthor = client.IdAuthor,
                Id = client.Id,
                IdCategory = client.IdCategory,
                ISBN = client.ISBN,
                Rate = client.Rate,
                Title = client.Title
            };

            return global;
        }
    }
}
