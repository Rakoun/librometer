using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

using Librometer.Framework;
using Librometer.Framework.IOC;
using Librometer.Model;
using Librometer.Model.Services;
using Librometer.Adapters;

namespace Librometer.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private IDialogService _windowServices;
        private INavigationServiceFacade _navigationServiceFacade;

        protected override void ServicesInitialization()
        {
            _windowServices
                = ServiceLocator.Instance.Retrieve<IDialogService>();
        }

        public SettingsViewModel()
        {
            this._pageTitle = "Paramétrage";//TODO: à mettre dans une ressource
        }

        public SettingsViewModel(INavigationServiceFacade navigationServiceFacade)
        {
            this._pageTitle = "Paramétrage";
            this._navigationServiceFacade = navigationServiceFacade;
        }

        protected override void CommandsInitialization()
        {
            OpenCategoriesSettingCommand = new ProxyCommand<SettingsViewModel>((_) =>
            {
                this._windowServices.OpenNewPage(
                            new Uri("/Librometer.Views;component/CategoriesPage.xaml", UriKind.Relative),
                            this._navigationServiceFacade);
            });

            OpenAuthorsSettingCommand = new ProxyCommand<SettingsViewModel>((_) =>
            {
                this._windowServices.OpenNewPage(
                            new Uri("/Librometer.Views;component/AuthorsPage.xaml", UriKind.Relative),
                            this._navigationServiceFacade);
            });

            OpenBooksSettingCommand = new ProxyCommand<SettingsViewModel>((_) =>
            {
                this._windowServices.OpenNewPage(
                            new Uri("/Librometer.Views;component/BooksPage.xaml", UriKind.Relative),
                            this._navigationServiceFacade);
            });

        }

        #region Liste des paramètres

        public string CategoriesSetting
        {
            get { return "Catégories"; }
        }

        public string AuthorsSetting
        {
            get { return "Auteurs"; }
        }

        public string BooksSetting
        {
            get { return "Livres"; }
        }

        #endregion//Liste des paramètres

        #region Propriétés


        #region PageTitle

        private string _pageTitle;
        public string PageTitle
        {
            get { return this._pageTitle; }
            set { this._pageTitle = value; }
        }

        #endregion //PageTitle

        #endregion //Propriétés

        public ProxyCommand<SettingsViewModel> OpenCategoriesSettingCommand { get; set; }
        public ProxyCommand<SettingsViewModel> OpenAuthorsSettingCommand { get; set; }
        public ProxyCommand<SettingsViewModel> OpenBooksSettingCommand { get; set; }
        
    }
}
 