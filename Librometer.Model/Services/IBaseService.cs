using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Librometer.Model.Services
{
    /// <summary>
    /// Interface de base pour les services.
    /// </summary>
    /// <typeparam name="TModelBase"></typeparam>
    /// <typeparam name="TCriteria"></typeparam>
    public interface IBaseService<TModelBase, /* RGE: non supporté in*/ TCriteria>
        where TModelBase : BaseModel
        where TCriteria : BaseCriteria<TCriteria>, new()
    {
        bool Create(TModelBase toCreate);
        bool Update(TModelBase toUpdate);
        bool Delete(TModelBase toDelete);
        TModelBase GetById(int id);
        List<TModelBase> GetByCriteria(TCriteria criteria);
        int GetLastCreatedId();
        bool GetAsync(TCriteria criteria, Action<AsyncResponse, List<TModelBase>> callback);
        void ApplyChanges();
        void UndoChanges();
    }

    /// <summary>
    /// Interface de base pour les services n'ayant pas de critères de 
    /// recherche spécifiques.
    /// </summary>
    /// <typeparam name="TModelBase"></typeparam>
    public interface IServiceBase<TModelBase>
        : IBaseService<TModelBase, BaseCriteria>
        where TModelBase : BaseModel { }
}
