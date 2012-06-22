using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Librometer.Framework;
using Librometer.Model;
using Librometer.Model.Services;
using Librometer.Framework.IOC;
using Librometer.Adapters;
using System.Windows;
using Rakouncom.WP.IsolatedStorage;

namespace Librometer.ViewModels
{
    public class BookListViewModel : BaseListViewModel<BookViewModel>
    {
        private IDialogService _windowServices;
        private IBookService _bookService;
        private INavigationServiceFacade _navigationServiceFacade;

        public BookListViewModel() { }
        public BookListViewModel(INavigationServiceFacade navigationServiceFacade)
        {
            this._navigationServiceFacade = navigationServiceFacade;
        }


        #region Méthodes d'initialisation

        protected override void ServicesInitialization()
        {
            _windowServices
                = ServiceLocator.Instance.Retrieve<IDialogService>();
            _bookService = ServiceLocator.Instance.Retrieve<IBookService>();
        }

        protected override void CommandsInitialization()
        {
            LaunchSearchCommand = new ProxyCommand<BookListViewModel>(
                (param) => LaunchSearch(),
                /*_ => !string.IsNullOrEmpty(this.SearchText)*/null,
            this);

            OpenAddBookCommand = new ProxyCommand<BookListViewModel>((_) =>
            {
                var viewModel = new BookViewModel(
                            new BookModel(),
                            "Créer un livre",
                            this._navigationServiceFacade, false);//TODO:mettre la chaîne "Créer un lvire" dans une ressource
                viewModel.Book.BeginEdit();
                _windowServices
                            .OpenSaveOrCancelWindow<BookViewModel>(
                                    viewModel, this._navigationServiceFacade, (bookVM) =>
                                    {
                                        bookVM.Book.EndEdit();

                                        var srvBook = ServiceLocator.Instance.Retrieve<IBookService>();

                                        // on évalue l'identifiant du prochain livre
                                        string nextBookId = (srvBook.GetLastCreatedId() + 1).ToString().PadLeft(4, '0');

                                        // création du chemin d'accès au fichier image qui contiendra
                                        // la photo de la couverture du livre
                                        string coverPath = "Librometer/images/cover" + nextBookId + ".jpg";

                                        // on sauvegarde le nouveau livre
                                        bookVM.Book.IdCategory = bookVM.SelectedCategory.Id;
                                        bookVM.Book.IdAuthor = bookVM.SelectedAuthor.Id;
                                        bookVM.Book.Cover = coverPath;
                                        bool ok = srvBook.Create(bookVM.Book);

                                        // on crée le nouveau Bookmark
                                        var srvBookmark = ServiceLocator.Instance.Retrieve<IBookmarkService>();
                                        BookmarkModel bookmark = new BookmarkModel()
                                        {
                                            CreationDate = DateTime.Now.ToShortDateString(),
                                            IdBook = 0,
                                            ThumbImage = coverPath,
                                            Name = bookVM.Book.Title,
                                            ReaderPage = 0
                                        };
                                        srvBookmark.Create(bookmark);

                                        srvBook.ApplyChanges();
                                        srvBookmark.ApplyChanges();

                                        if (IsolatedStorageHelper.FileExist("Librometer/images/draft/cover.jpg") == true)
                                        {
                                            IsolatedStorageHelper.CopyFile("Librometer/images/draft/cover.jpg", coverPath);
                                            // on supprime l'image contenu dans le répertoire draft
                                            IsolatedStorageHelper.DeleteFile("Librometer/images/draft/cover.jpg");
                                        }


                                        return ok;

                                    }, null);
            });

            DeleteBookCommand = new ProxyCommand<BookListViewModel>((_) =>
                {
                    if (_windowServices.AskConfirmation(
                                "Suppresion des livres",
                                "Voulez-vous supprimer votre sélection") == true)
                    {
                        //this.SearchText = this.SelectedItems.Count.ToString();
                        foreach(BookViewModel book in this.SelectedItems)
                        {
                            _bookService.Delete(book.Book);
                        }
                        _bookService.ApplyChanges();

                        this.IsBtnNewVisible = true;
                        this.IsBtnChoiceVisible = true;
                        this.IsBtnDeleteVisible = false;
                        //this.IsBtnDeleteEnabled = false;TODO: quand on saura intercepter la sélection d'un item
                        this.IsLstBoxInChooseState = false;
                    }
                });

            EditBookViewCommand = new ProxyCommand<BookListViewModel>((_) =>
                {
                });

            ChoiceBookCommand = new ProxyCommand<BookListViewModel>((_) =>
                {
                    this.IsBtnNewVisible = false;
                    this.IsBtnChoiceVisible = false;
                    this.IsBtnDeleteVisible = true;
                    this.IsLstBoxInChooseState = true;
                });

            TapCommand = new ProxyCommand<BookListViewModel>((param) =>
                {
                    if (this.IsBtnDeleteVisible == false)
                    {
                        BookViewModel selectedItem = param as BookViewModel;
                        int test = selectedItem.GetHashCode();
                        if (selectedItem != null)
                        {
                            selectedItem.Book.BeginEdit();
                            _windowServices
                                        .OpenSaveOrCancelWindow(
                                                selectedItem,
                                                this._navigationServiceFacade,
                                // Méthodes appelée si sauvegarde
                                                (bookVM) =>
                                                {
                                                    bookVM.Book.EndEdit();
                                                    var srv = ServiceLocator.Instance.Retrieve<IBookService>();

                                                    // création du chemin d'accès au fichier image qui contiendra
                                                    // la photo de la couverture du livre
                                                    string coverPath = "Librometer/images/cover" + 
                                                                bookVM.Book.Id.ToString().PadLeft(4, '0') + ".jpg";

                                                    // on met à jour le livre
                                                    bookVM.Book.IdCategory = bookVM.SelectedCategory.Id;
                                                    bookVM.Book.IdAuthor = bookVM.SelectedAuthor.Id;
                                                    bookVM.Book.Cover = coverPath;
                                                    bool ok = srv.Update(bookVM.Book);


                                                    srv.ApplyChanges();

                                                    if (bookVM.IsNewPhoto == true)
                                                    {
                                                        if (IsolatedStorageHelper.FileExist("Librometer/images/draft/cover.jpg") == true)
                                                        {
                                                            IsolatedStorageHelper.CopyFile("Librometer/images/draft/cover.jpg", coverPath);
                                                            // on supprime l'image contenu dans le répertoire draft
                                                            IsolatedStorageHelper.DeleteFile("Librometer/images/draft/cover.jpg");
                                                        }
                                                    }

                                                    return ok;
                                                },
                                // Si annulation on annule les changements
                                                (_) =>
                                                {
                                                    selectedItem.Book.CancelEdit(); return true;
                                                });
                        }
                    }
                });
        }

        protected override System.Collections.ObjectModel.ObservableCollection<BookViewModel> LoadItems()
        {
            IsBeingProcessed = true;
            // Obtention des marques pages et création des ViewModel correspondant
            BookCriteria searchCriteria = BookCriteria.Empty;
            searchCriteria.Title = this.SearchText;

            _bookService.GetAsync(searchCriteria, (bookResponse, books) =>
                {
                    if (bookResponse.HasError)
                    {
                        // Traitement de l'erreur
                        IsBeingProcessed = false;
                        return;
                    }
                    ObservableCollection<BookViewModel> tempList
                        = new ObservableCollection<BookViewModel>();
                    foreach (BookModel bookModel in books)
                    {
                        //TODO: mettre le titre dans une ressource
                        BookViewModel bookViewModel =
                            new BookViewModel(
                                        bookModel, "Livres",
                                        this._navigationServiceFacade, false);
                        tempList.Add(bookViewModel);
                    }

                    Items = tempList;
                    IsBeingProcessed = false;
                });

            return new ObservableCollection<BookViewModel>();
        }

        #endregion //Méthodes d'initialisation

        #region Propriétés

        #region SearchText

        private string _searchText = string.Empty;

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

        #region IsBtnNewVisible

        private bool _isBtnNewVisible = true;

        public bool IsBtnNewVisible
        {
            get { return _isBtnNewVisible; }
            set
            {
                if (_isBtnNewVisible == value)
                    return;
                _isBtnNewVisible = value;
                RaisePropertyChanged<bool>(() => IsBtnNewVisible);
            }
        }

        #endregion // IsBtnNewVisible

        #region IsBtnChoiceVisible

        private bool _isBtnChoiceVisible = true;

        public bool IsBtnChoiceVisible
        {
            get { return _isBtnChoiceVisible; }
            set
            {
                if (_isBtnChoiceVisible == value)
                    return;
                _isBtnChoiceVisible = value;
                RaisePropertyChanged<bool>(() => IsBtnChoiceVisible);
            }
        }

        #endregion // IsBtnChoiceVisible

        #region IsBtnDeleteVisible

        private bool _isBtnDeleteVisible = false;

        public bool IsBtnDeleteVisible
        {
            get { return _isBtnDeleteVisible; }
            set
            {
                if (_isBtnDeleteVisible == value)
                    return;
                _isBtnDeleteVisible = value;
                RaisePropertyChanged<bool>(() => IsBtnDeleteVisible);
            }
        }

        #endregion // IsBtnNewVisible

        #region IsBtnDeleteEnabled

        private bool _isBtnDeleteEnabled = true;

        public bool IsBtnDeleteEnabled
        {
            get { return _isBtnDeleteEnabled; }
            set
            {
                if (_isBtnDeleteEnabled == value)
                    return;
                _isBtnDeleteEnabled = value;
                RaisePropertyChanged<bool>(() => IsBtnDeleteEnabled);
            }
        }

        #endregion // IsBtnNewVisible

        #region IsLstBoxInChooseState

        private bool _isLstBoxInChooseState = false;

        public bool IsLstBoxInChooseState
        {
            get { return _isLstBoxInChooseState; }
            set
            {
                if (_isLstBoxInChooseState == value) return;
                _isLstBoxInChooseState = value;
                RaisePropertyChanged<bool>(() => IsLstBoxInChooseState);
            }
        }

        #endregion // IsLstBoxInChooseState

        #region OpenAddBookCommand

        public ProxyCommand<BookListViewModel> OpenAddBookCommand { get; set; }

        #endregion //OpenAddBookCommand

        #region EditBookViewCommand

        public ProxyCommand<BookListViewModel> EditBookViewCommand { get; set; }

        #endregion //EditBookViewCommand

        #region LaunchSearchCommand

        private ProxyCommand<BookListViewModel> _launchSearchCommand = null;

        public ProxyCommand<BookListViewModel> LaunchSearchCommand
        {
            get { return _launchSearchCommand; }
            set
            {
                if (_launchSearchCommand == value) return;
                _launchSearchCommand = value;
                RaisePropertyChanged<ProxyCommand<BookListViewModel>>(() => LaunchSearchCommand);
            }
        }

        #endregion //LaunchSearchCommand

        #region DeleteBookCommand

        public ProxyCommand<BookListViewModel> DeleteBookCommand { get; set; }

        #endregion //DeleteBookCommand

        #endregion //Propriétés

        #region ChoiceBookCommand

        public ProxyCommand<BookListViewModel> ChoiceBookCommand { get; set; }

        #endregion //ChoiceBookCommand

        #region TapCommand

        public ProxyCommand<BookListViewModel> TapCommand { get; set; }

        #endregion // TapCommand

        private void LaunchSearch()
        {
            this.Refresh();
        }
    }
}
