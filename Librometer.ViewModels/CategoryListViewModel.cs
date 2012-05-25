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

namespace Librometer.ViewModels
{
    public class CategoryListViewModel : BaseListViewModel<CategoryViewModel>
    {
        private IDialogService _windowServices;
        private ICategoryService _categoryService;
        private INavigationServiceFacade _navigationServiceFacade;

        public CategoryListViewModel() {}
        public CategoryListViewModel(INavigationServiceFacade navigationServiceFacade)
        {
            this._navigationServiceFacade = navigationServiceFacade;
        }


        #region Méthodes d'initialisation

        protected override void ServicesInitialization()
        {
            _windowServices
                = ServiceLocator.Instance.Retrieve<IDialogService>();
            _categoryService = ServiceLocator.Instance.Retrieve<ICategoryService>();
        }

        protected override void CommandsInitialization()
        {
            LaunchSearchCommand = new ProxyCommand<CategoryListViewModel>(
                (param) => LaunchSearch(),
                /*_ => !string.IsNullOrEmpty(this.SearchText)*/null,
            this);

            OpenAddCategoryCommand = new ProxyCommand<CategoryListViewModel>((_) =>
            {
                var viewModel = new CategoryViewModel(
                            new CategoryModel(),
                            "Créer une catégorie",
                            this._navigationServiceFacade, false);//TODO:mettre la chaîne "Créer une catégorie" dans une ressource
                viewModel.Category.BeginEdit();
                _windowServices
                            .OpenSaveOrCancelWindow<CategoryViewModel>(
                                    viewModel, this._navigationServiceFacade, (categoryVM) =>
                            {
                                categoryVM.Category.EndEdit();
                                var srv = ServiceLocator.Instance.Retrieve<ICategoryService>();
                                bool ok = srv.Create(categoryVM.Category);
                                srv.ApplyChanges();
                                return ok;

                            }, null);
            });

            DeleteCategoryCommand = new ProxyCommand<CategoryListViewModel>((_) =>
                {
                    if (_windowServices.AskConfirmation(
                                "Suppresion des catégories",
                                "Voulez-vous supprimer votre sélection") == true)
                    {
                        this.SearchText = this.SelectedItems.Count.ToString();
                    }
                });
        }

        protected override System.Collections.ObjectModel.ObservableCollection<CategoryViewModel> LoadItems()
        {
            IsBeingProcessed = true;
            // Obtention des marques pages et création des ViewModel correspondant
            CategoryCriteria searchCriteria = CategoryCriteria.Empty;
            searchCriteria.Name = this.SearchText;

            _categoryService.GetAsync(searchCriteria, (categoryResponse, bookmarks) =>
                {
                    if (categoryResponse.HasError)
                    {
                        // Traitement de l'erreur
                        IsBeingProcessed = false;
                        return;
                    }
                    ObservableCollection<CategoryViewModel> tempList
                        = new ObservableCollection<CategoryViewModel>();
                    foreach (CategoryModel bookmarkModel in bookmarks)
                    {
                        //TODO: mettre le titre dans une ressource
                        CategoryViewModel bookmarkViewModel =
                            new CategoryViewModel(
                                        bookmarkModel, "Categories",
                                        this._navigationServiceFacade, false);
                        tempList.Add(bookmarkViewModel);
                    }

                    Items = tempList;
                    IsBeingProcessed = false;
                });

            return new ObservableCollection<CategoryViewModel>();
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

        #region OpenAddCategoryCommand

        public ProxyCommand<CategoryListViewModel> OpenAddCategoryCommand { get; set; }

        #endregion //OpenAddBookmarkViewCommand

        #region DisplayBookmarkSearchViewCommand

        public ProxyCommand<CategoryListViewModel> DisplayBookmarkSearchViewCommand { get; set; }

        #endregion //DisplayBookmarkSearchViewCommand

        #region LaunchSearchCommand

        private ProxyCommand<CategoryListViewModel> _launchSearchCommand = null;

        public ProxyCommand<CategoryListViewModel> LaunchSearchCommand
        {
            get { return _launchSearchCommand; }
            set
            {
                if (_launchSearchCommand == value) return;
                _launchSearchCommand = value;
                RaisePropertyChanged<ProxyCommand<CategoryListViewModel>>(() => LaunchSearchCommand);
            }
        }

        #endregion //LaunchSearchCommand

        #region DeleteCategoryCommand

        public ProxyCommand<CategoryListViewModel> DeleteCategoryCommand { get; set; }

        #endregion//DeleteCategoryCommand

        #endregion //Propriétés

        private void LaunchSearch()
        {
            this.Refresh();
        }
    }
}
