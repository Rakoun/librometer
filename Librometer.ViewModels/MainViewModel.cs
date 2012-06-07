using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Librometer.Framework;
using Librometer.Framework.IOC;
using Librometer.Model;
using Librometer.Model.Services;
using Librometer.Adapters;
using Rakouncom.WP.IsolatedStorage;

namespace Librometer.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private IDialogService _windowServices;
        private INavigationServiceFacade _navigationServiceFacade;

        protected override void ServicesInitialization()
        {
            _windowServices
                = ServiceLocator.Instance.Retrieve<IDialogService>();
        }

        public MainViewModel() { }
        public MainViewModel(INavigationServiceFacade navigationServiceFacade)
        {
            this._navigationServiceFacade = navigationServiceFacade;
        }

        protected override void CommandsInitialization()
        {
            OpenAddBookCommand = new ProxyCommand<MainViewModel>((_) =>
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
                                        string nextBookId = (srvBook.GetLastCreatedId() + 1).ToString().PadLeft(4,'0');

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
                                            CreationDate= DateTime.Now.ToShortDateString(),
                                            IdBook = 0,
                                            ThumbImage = coverPath,
                                            Name=bookVM.Book.Title,
                                            ReaderPage=0
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

            DisplayParametersCommand = new ProxyCommand<MainViewModel>((_) =>
                {
                    _windowServices.OpenNewPage(
                                new Uri("/Librometer.Views;component/SettingsPage.xaml", UriKind.Relative),
                                this._navigationServiceFacade);
                });
        }

        #region Elements de Menu

        public string Settings
        {
            get { return "Paramétrages"; }
        }

        public string Credits
        {
            get { return "Crédits"; }
        }

        #endregion

        /* RGE public ProxyCommand<MainViewModel> QuitApplicationCommand { get; set; }*/
        public ProxyCommand<MainViewModel> DisplayParametersCommand { get; set; }
        public ProxyCommand<MainViewModel> DisplayAboutCommand { get; set; }
        public ProxyCommand<MainViewModel> OpenAddBookCommand { get; set; }
    }
}
 