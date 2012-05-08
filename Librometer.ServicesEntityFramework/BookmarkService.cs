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
    public partial class BookmarkService
        : BaseService<BookmarkModel, Bookmark, BookmarkCriteria>
        , IBookmarkService
    {
        #region Méthodes spécifiques à IBookService
        //TODO: si nécessaire
        #endregion


        #region Méthodes surchargée de ServiceBase
        protected override Bookmark MapToEntity(BookmarkModel model)
        {
            var global = model.ToBookmark();
            return global;
        }

        protected override BookmarkModel MapToModel(Bookmark entity)
        {
            var bookmark = entity.ToBookmarkModel();

            return bookmark;
        }

        protected override IQueryable<Bookmark> GetConcrete(BookmarkCriteria criteria)
        {
            Thread.Sleep(3000);

            IQueryable<Bookmark> bookmarks =
                bookmarks = from Bookmark bookmark in Context.Bookmarks
                            select bookmark;

            if (!string.IsNullOrEmpty(criteria.Name))
                bookmarks = bookmarks.Where(b => b.Name.Contains(criteria.Name));

            return bookmarks;
        }

        protected override void DeepCopy(Bookmark oldEntity, Bookmark newEntity)
        {
            oldEntity.Id = newEntity.Id;
            oldEntity.Name = newEntity.Name;
            oldEntity.Notes = newEntity.Notes;
            oldEntity.ReaderPage = newEntity.ReaderPage;
            oldEntity.ThumbImage = newEntity.ThumbImage;
            oldEntity.CreationDate = newEntity.CreationDate;
        }

        #endregion // Méthodes surchargée de ServiceBase

        public bool GetListAsync(IEnumerable<int> idBookmarks,
                Action<AsyncResponse, List<BookmarkModel>> callback)
        {
            return ExecuteAsync<IEnumerable<int>, List<BookmarkModel>>(idBookmarks,
                GetList, callback, SynchronisationContext);
        }

        private List<BookmarkModel> GetList(IEnumerable<int> idBookmarks)
        {
            return Context.Bookmarks
                     .Where(b => idBookmarks.Contains(b.Id)).ToList()
                     .Select(b => b.ToBookmarkModel()).ToList();
        }
    }
}

