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
    public partial class AuthorService
        : BaseService<AuthorModel, Author, AuthorCriteria>
        , IAuthorService
    {

        #region Méthodes spécifiques à IAuthorService
        //TODO: si nécessaire
        #endregion


        #region Méthodes surchargée de ServiceBase
        protected override Author MapToEntity(AuthorModel model)
        {
            var global = model.ToAuthor();
            return global;
        }

        protected override AuthorModel MapToModel(Author entity)
        {
            var author = entity.ToAuthorModel();

            return author;
        }

        protected override IQueryable<Author> GetConcrete(AuthorCriteria criteria)
        {
            Thread.Sleep(3000);

            IQueryable<Author> authors =
                authors = from Author author in Context.Authors
                         select author;

            if (!string.IsNullOrEmpty(criteria.LastName))
                authors = authors.Where(a => a.LastName.Contains(criteria.LastName));

            return authors;
        }

        protected override void DeepCopy(Author oldEntity, Author newEntity)
        {
            oldEntity.DisplayName = newEntity.DisplayName;
            oldEntity.FirstName = newEntity.FirstName;
            oldEntity.Id = newEntity.Id;
            oldEntity.LastName = newEntity.LastName;
            oldEntity.Books = newEntity.Books;
        }
        #endregion // Méthodes surchargée de ServiceBase
    }
}
