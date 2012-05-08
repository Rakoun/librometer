using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Librometer.Model;
using Librometer.Model.Services;
using Librometer.ServicesSQLCE.datas;

namespace Librometer.ServicesSQLCE
{
    public abstract class BaseService<TBaseModel, TEntity>
        : BaseService<TBaseModel, TEntity, BaseCriteria>
        where TBaseModel : BaseModel
        where TEntity : BaseModel//class
    { }

    public abstract class BaseService<TBaseModel, TEntity, TCriteria>
        : IBaseService<TBaseModel, TCriteria>, IDisposable
        where TBaseModel : BaseModel
        where TEntity : BaseModel//class
        where TCriteria : BaseCriteria<TCriteria>, new()
    {
        #region Propriétés

        private LibroContext _context;
        protected LibroContext Context
        {
            get { return this._context; }
        }
        #endregion //Propriétés

        public BaseService()
        {
            _context = new LibroContext(LibroContext.DBConnectionString);//TODO: il faudra mettre la chaine de connection dans une constante.
            if (!_context.DatabaseExists())
            {
                _context.CreateDatabase();
                /*
                try
                {
                    Author author = new Author()
                    {
                        FirstName = "Patrick",
                        LastName = "Smacchia",
                        DisplayName = "Patrick Smacchia",
                    };
                    _context.Authors.InsertOnSubmit(author);
                    _context.SubmitChanges();

                    Category category = new Category()
                    {
                        Name = "Informatique",
                    };
                    _context.Categories.InsertOnSubmit(category);
                    _context.SubmitChanges();

                    Book book = new Book()
                    {
                        IdAuthor = 1,
                        IdCategory = 1,
                        ISBN = "2-84177-245-4",
                        Rate = 5,
                        Title = "Pratique de .net et C#",
                        Editor = "O'Reilly",
                        Cover = "isostore:/Librometer/images/book_0001.jpg",
                    };
                    _context.Books.InsertOnSubmit(book);
                    _context.SubmitChanges();

                    Bookmark bookmark = new Bookmark()
                    {
                        ReaderPage = 100,
                        ThumbImage = "isostore:/Librometer/images/book_0001.jpg",
                        Name = "Pratique de .Net et C#",
                        CreationDate = DateTime.Now.ToShortDateString(),
                    };
                    _context.Bookmarks.InsertOnSubmit(bookmark);
                    _context.SubmitChanges();
                }
                catch (Exception ex)
                {
                    _context.DeleteDatabase();
                }*/
            }
        }

        #region méthodes abtraites

        protected abstract TEntity MapToEntity(TBaseModel model);
        protected abstract TBaseModel MapToModel(TEntity entity);
        protected abstract IQueryable<TEntity> GetConcrete(TCriteria criteria);
        /// <summary>
        /// Copie les valeur de newEntity dans oldEntity
        /// </summary>
        /// <param name="oldEntity">l'entité à mettre à jour</param>
        /// <param name="newEntity">l'entité contenant les valeurs à mettre à jour</param>
        protected abstract void DeepCopy(TEntity oldEdntity, TEntity newEntity);
        protected virtual List<string> ValidationUpdating(TBaseModel previousValue, TBaseModel newValue)
        { return new List<string>(); }
        protected virtual List<string> ValidationCreating(TBaseModel value)
        { return new List<string>(); }
        protected virtual List<string> ValidationDeleting(TBaseModel toDelete)
        { return new List<string>(); }

        #endregion

        public virtual bool Create(TBaseModel toCreate)
        {
            //Verification de la validite de l'objet
            var validationErrors = ValidationCreating(toCreate);
            if (validationErrors.Count > 0)
            {
                toCreate.ReplaceGolablValidationErrors(validationErrors);
                return false;
            }
            TEntity entityToCreate = MapToEntity(toCreate);
            this._context.GetTable<TEntity>().InsertOnSubmit(entityToCreate);
            toCreate.HasBeenModified = false;
            return true;
        }

        public virtual bool Update(TBaseModel toUpdate)
        {
            //obtention de l'ancienne valeur
            var oldValue = GetEntityById(toUpdate.Id);

            //Verification de la validite de l'objet
            var validationErrors =
                ValidationUpdating(MapToModel(oldValue), toUpdate);
            if (validationErrors.Count > 0)
            {
                toUpdate.ReplaceGolablValidationErrors(validationErrors);
                return false;
            }

            //Mise à jour effective
            TEntity entityToUpdate = MapToEntity(toUpdate);
            DeepCopy(oldValue, entityToUpdate); 
            
            toUpdate.HasBeenModified = false;
            return true;
        }


        public virtual bool Delete(TBaseModel toDelete)
        {
            //Verification de la validite de l'objet
            var validationErrors = ValidationDeleting(toDelete);
            if (validationErrors.Count > 0)
            {
                toDelete.ReplaceGolablValidationErrors(validationErrors);
                return false;
            }
            var oldValue = GetEntityById(toDelete.Id);
            this._context.GetTable<TEntity>().DeleteOnSubmit(oldValue);
            return true;
        }

        public virtual TBaseModel GetById(int id)
        {
            var entity = GetEntityById(id);
            if (entity != null)
            {
                var model = MapToModel(entity);
                return model;
            }
            return null;
        }

        public virtual List<TBaseModel> GetByCriteria(TCriteria criteria)
        {
            IQueryable<TEntity> getConcrete = this.GetConcrete(criteria);
            if (criteria.TailleLimite > 0)
            {
                getConcrete = getConcrete.Take(criteria.TailleLimite);
            }
            var list = getConcrete.ToList().Select(MapToModel);

            return list.ToList();
        }

        protected SynchronizationContext SynchronisationContext
        {
            get { return SynchronizationContext.Current; }
        }

        public virtual bool GetAsync(TCriteria criteria,
           Action<AsyncResponse, List<TBaseModel>> callback)
        {
            //Obtention du thread UI
            var UISyncContext = SynchronizationContext.Current;

            //Définition de l'appel
            WaitCallback waitCallBack =
                param =>
                {
                    AsyncResponse response = new AsyncResponse();
                    List<TBaseModel> retour = default(List<TBaseModel>);
                    try
                    {
                        //On obtient la liste des éléments
                        retour = GetByCriteria((TCriteria)param);
                    }
                    catch (Exception e)
                    {
                        response.HasError = true;
                        response.ErrorMessage = e.Message;
                    }
                    finally
                    {
                        //On execute le callback sur le Thread UI
                        UISyncContext
                            .Post(cParam => callback(response, (List<TBaseModel>)cParam)
                            , retour);
                    }
                };

            //Lancement effectif du traitement
            return ThreadPool.QueueUserWorkItem(waitCallBack, criteria);
        }

        /// <summary>
        /// Méthodes d'aide permettant d'implémenter
        /// facilement un appel asynchrone.
        /// </summary>
        protected bool ExecuteAsync<TParam, TResult>(TParam parameter,
            Func<TParam, TResult> timeExpensiveTreatment,
            Action<AsyncResponse, TResult> callback,
            SynchronizationContext contextCallback)
        {

            //Définition de l'appel
            WaitCallback waitCallBack =
                param =>
                {
                    AsyncResponse response = new AsyncResponse();
                    TResult retour = default(TResult);
                    try
                    {
                        //On obtient la liste des éléments
                        retour = timeExpensiveTreatment((TParam)param);
                    }
                    catch (Exception e)
                    {
                        response.HasError = true;
                        response.ErrorMessage = e.Message;
                    }
                    finally
                    {
                        //On execute le callback sur le Thread UI
                        contextCallback
                            .Post(cParam => callback(response, (TResult)cParam)
                            , retour);
                    }

                };

            //Lancement effectif du traitement
            return ThreadPool.QueueUserWorkItem(waitCallBack, parameter);
        }


        public void ApplyChanges()
        {
            this._context.SubmitChanges();
        }

        public void UndoChanges()
        {
            this._context.Dispose();
            this._context = new LibroContext("chaine de connexion");//TODO: mettre la chaine de connexion dans une variable
        }


        #region Méthode d'aides

        private TEntity GetEntityById(int id)
        {
            var entity = this._context.GetTable<TEntity>().Where(t => t.Id == id).SingleOrDefault();
            return entity;
        }
        #endregion

        public void Dispose()
        {
            if (this._context != null) this._context.Dispose();
        }

    }
}
