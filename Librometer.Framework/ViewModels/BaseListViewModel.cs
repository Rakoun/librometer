using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace Librometer.Framework
{
    public abstract class BaseListViewModel<T> : BaseViewModel
        where T : class
    {
        #region Méthodes abstraites
        /// <summary>
        /// Méthode ABSTRAITE fournissant les différents items.
        /// </summary>
        /// <returns>Les items chargés</returns>
        protected abstract ObservableCollection<T> LoadItems();
        /// <summary>
        /// Méthode appelée lorsque l'item courant change.
        /// À surcharger pour insérer un traitement particulier.
        /// </summary>
        protected virtual void CurrentElementChanged() { }
        protected bool _isReloadingNecessary = true;
        #endregion // Méthodes abstraites

        private bool _isBeingProcessed;
        public bool IsBeingProcessed
        {
            get { return _isBeingProcessed; }
            set
            {
                if (_isBeingProcessed == value)
                    return;
                _isBeingProcessed = value;
                RaisePropertyChanged<bool>(() => IsBeingProcessed);
            }
        }



        #region Items
        private ObservableCollection<T> _items = null;

        /// <summary>
        /// La liste d'items du ViewModel
        /// </summary>
        public ObservableCollection<T> Items
        {
            get
            {
                //Si les items ne sont pas chargés, on laisse le soin 
                //aux classes filles de fournir les données.
                if (_items == null || _isReloadingNecessary)
                {
                    _items = LoadItems();
                    _isReloadingNecessary = false;
                }
                return _items;
            }
            set
            {
                if (_items != value)
                {
                    _items = value;
                    RaisePropertyChanged<ObservableCollection<T>>(() => Items);
                }
            }
        }
        #endregion //Items property

        /// <summary>
        /// Retourne la vue par défaut sur la liste d'items
        /// </summary>
        protected ICollectionView DefaultViewOnList
        {
            get
            { 
                //return CollectionViewSource.GetDefaultView(Items);ne fonctionne pas avec une library de type WP7
                CollectionViewSource source = new CollectionViewSource();
                source.Source = Items;
                return source.View;
            }
        }

        #region ElementCourant
        /// <summary>
        /// Le LivreViewModel courant dans la collection de livres.
        /// </summary>
        public T CurrentElement
        {
            get { return (Items != null) ? DefaultViewOnList.CurrentItem as T : null; }
            set
            {
                DefaultViewOnList.MoveCurrentTo(value);
                RaisePropertyChanged<T>(() => CurrentElement);
                CurrentElementChanged();

            }
        }
        #endregion //ElementCourant


        #region SelectedItems
        private ObservableCollection<T> _selectedItems = new ObservableCollection<T>();

        /// <summary>
        /// Les différents éléments selectionnés
        /// </summary>
        public ObservableCollection<T> SelectedItems
        {
            get { return _selectedItems; }
            set
            {
                if (_selectedItems != value)
                {
                    _selectedItems = value;
                    RaisePropertyChanged<ObservableCollection<T>>(() => SelectedItems);
                }
            }
        }
        #endregion //SelectedItems property

        public void Refresh()
        {
            _isReloadingNecessary = true;
            RaisePropertyChanged<ObservableCollection<T>>(() => Items);
        }
    }
}
