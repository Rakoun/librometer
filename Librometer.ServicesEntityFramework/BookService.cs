using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Librometer.Model;
using Librometer.Model.Services;
using Librometer.ServicesSQLCE.Mappers;
using Librometer.ServicesSQLCE.datas;

namespace Librometer.ServicesSQLCE
{
    public partial class BookService
        : BaseService<BookModel, Book, BookCriteria>
        , IBookService
    {
        #region Méthodes spécifiques à IBookService
        //TODO: si nécessaire
        #endregion


        #region Méthodes surchargée de ServiceBase
        protected override Book MapToEntity(BookModel model)
        {
            var global = model.ToBook();
            return global;
        }

        protected override BookModel MapToModel(Book entity)
        {
            var book = entity.ToBookModel();

            return book;
        }

        protected override IQueryable<Book> GetConcrete(BookCriteria criteria)
        {
            Thread.Sleep(3000);

            IQueryable<Book> books =
                books = from Book book in Context.Books
                        select book;

            if (!string.IsNullOrEmpty(criteria.Title))
                books = books.Where(b => b.Title.Contains(criteria.Title));

            return books;
        }

        protected override void DeepCopy(Book oldEntity, Book newEntity)
        {
            oldEntity.Id = newEntity.Id;
            oldEntity.Cover = newEntity.Cover;
            oldEntity.Editor = newEntity.Editor;
            oldEntity.IdAuthor = newEntity.IdAuthor;
            oldEntity.IdCategory = newEntity.IdCategory;
            oldEntity.ISBN = newEntity.ISBN;
            oldEntity.Rate = newEntity.Rate;
            oldEntity.Title = newEntity.Title;
        }

        #endregion // Méthodes surchargée de ServiceBase
    }
}
