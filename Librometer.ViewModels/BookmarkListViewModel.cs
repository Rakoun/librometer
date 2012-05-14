using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Librometer.Framework;
using Librometer.Model;
using Librometer.Model.Services;
using Librometer.Framework.IOC;

namespace Librometer.ViewModels
{
    public class BookmarkListViewModel : BaseListViewModel<BookmarkViewModel>
    {
        private IBookmarkService _bookmarkService;

        public BookmarkListViewModel()
        {
        }

        #region Méthodes d'initialisation

        protected override void ServicesInitialization()
        {
            _bookmarkService = ServiceLocator.Instance.Retrieve<IBookmarkService>();
        }

        protected override void CommandsInitialization()
        {
            LaunchSearchCommand = new ProxyCommand<BookmarkListViewModel>(
                (param) => LaunchSearch(),
                /*_ => !string.IsNullOrEmpty(this.SearchText)*/null,
            this);
        }

        protected override System.Collections.ObjectModel.ObservableCollection<BookmarkViewModel> LoadItems()
        {
            IsBeingProcessed = true;
            // Obtention des marques pages et création des ViewModel correspondant
            BookmarkCriteria searchCriteria = BookmarkCriteria.Empty;
            searchCriteria.Name = this.SearchText;

            _bookmarkService.GetAsync(searchCriteria, (bookmarkResponse, bookmarks) =>
                {
                    if (bookmarkResponse.HasError)
                    {
                        // Traitement de l'erreur
                        IsBeingProcessed = false;
                        return;
                    }
                    ObservableCollection<BookmarkViewModel> tempList
                        = new ObservableCollection<BookmarkViewModel>();
                    foreach (BookmarkModel bookmarkModel in bookmarks)
                    {
                        BookmarkViewModel bookmarkViewModel = new BookmarkViewModel(bookmarkModel, false);
                        tempList.Add(bookmarkViewModel);
                    }

                    Items = tempList;
                    IsBeingProcessed = false;
                });

            return new ObservableCollection<BookmarkViewModel>();
        }

        #endregion //Méthodes d'initialisation

        #region Propriétés

        #region SearchText

        private string _searchText = ".net"/*string.Empty*/;

        /// <summary>
        /// L'intitulé du marque page recherché.
        /// </summary>
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                if (_searchText == value) return;

                _searchText = value;
                RaisePropertyChanged<string>(() => SearchText);
            }
        }

        #endregion //SearchText

        #region OpenAddBookmarkViewCommand

        public ProxyCommand<BookmarkListViewModel> OpenAddBookmarkViewCommand { get; set; }

        #endregion //OpenAddBookmarkViewCommand

        #region DisplayBookmarkSearchViewCommand

        public ProxyCommand<BookmarkListViewModel> DisplayBookmarkSearchViewCommand { get; set; }

        #endregion //DisplayBookmarkSearchViewCommand

        #region LaunchSearchCommand

        private ProxyCommand<BookmarkListViewModel> _launchSearchCommand = null;

        public ProxyCommand<BookmarkListViewModel> LaunchSearchCommand
        {
            get { return _launchSearchCommand; }
            set
            {
                if (_launchSearchCommand == value) return;
                _launchSearchCommand = value;
                RaisePropertyChanged<ProxyCommand<BookmarkListViewModel>>(() => LaunchSearchCommand);
            }
        }


        #endregion //LaunchSearchCommand


        #endregion //Propriétés

        private void LaunchSearch()
        {
            this.Refresh();


        }
    }
}
