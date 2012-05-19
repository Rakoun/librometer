using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Librometer.Framework;
using Librometer.Framework.IOC;
using Librometer.Model;
using Librometer.Model.Services;
using Librometer.Adapters;

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
                                        var srv = ServiceLocator.Instance.Retrieve<IBookService>();
                                        bool ok = srv.Create(bookVM.Book);
                                        srv.ApplyChanges();
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
 