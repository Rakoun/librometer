﻿using System;
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

namespace Librometer.ViewModels
{
    public class CategoryListViewModel : BaseListViewModel<CategoryViewModel>
    {
        private IDialogService _windowServices;
        private ICategoryService _categoryService;
        private INavigationServiceFacade _navigationServiceFacade;

        public CategoryListViewModel() { }
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
                        //this.SearchText = this.SelectedItems.Count.ToString();
                        foreach(CategoryViewModel category in this.SelectedItems)
                        {
                            _categoryService.Delete(category.Category);
                        }
                        _categoryService.ApplyChanges();

                        this.IsBtnNewVisible = true;
                        this.IsBtnChoiceVisible = true;
                        this.IsBtnDeleteVisible = false;
                        //this.IsBtnDeleteEnabled = false;TODO: quand on saura intercepter la sélection d'un item
                        this.IsLstBoxInChooseState = false;
                    }
                });

            EditCategoryViewCommand = new ProxyCommand<CategoryListViewModel>((_) =>
                {
                });

            ChoiceCategoryCommand = new ProxyCommand<CategoryListViewModel>((_) =>
                {
                    this.IsBtnNewVisible = false;
                    this.IsBtnChoiceVisible = false;
                    this.IsBtnDeleteVisible = true;
                    this.IsLstBoxInChooseState = true;
                });

            TapCommand = new ProxyCommand<CategoryListViewModel>((param) =>
                {
                    if (this.IsBtnDeleteVisible == false)
                    {
                        CategoryViewModel selectedItem = param as CategoryViewModel;
                        if (selectedItem != null)
                        {
                            selectedItem.Category.BeginEdit();
                            _windowServices
                                        .OpenSaveOrCancelWindow(
                                                selectedItem,
                                                this._navigationServiceFacade,
                                // Méthodes appelée si sauvegarde
                                                (categoryVM) =>
                                                {
                                                    categoryVM.Category.EndEdit();
                                                    var srv = ServiceLocator.Instance.Retrieve<ICategoryService>();
                                                    bool ok = srv.Update(categoryVM.Category);
                                                    srv.ApplyChanges();
                                                    return ok;
                                                },
                                // Si annulation on annule les changements
                                                (_) =>
                                                {
                                                    selectedItem.Category.CancelEdit(); return true;
                                                });
                        }
                    }
                });
        }

        protected override System.Collections.ObjectModel.ObservableCollection<CategoryViewModel> LoadItems()
        {
            IsBeingProcessed = true;
            // Obtention des marques pages et création des ViewModel correspondant
            CategoryCriteria searchCriteria = CategoryCriteria.Empty;
            searchCriteria.Name = this.SearchText;

            _categoryService.GetAsync(searchCriteria, (categoryResponse, categories) =>
                {
                    if (categoryResponse.HasError)
                    {
                        // Traitement de l'erreur
                        IsBeingProcessed = false;
                        return;
                    }
                    ObservableCollection<CategoryViewModel> tempList
                        = new ObservableCollection<CategoryViewModel>();
                    foreach (CategoryModel categoryModel in categories)
                    {
                        //TODO: mettre le titre dans une ressource
                        CategoryViewModel categoryViewModel =
                            new CategoryViewModel(
                                        categoryModel, "Categories",
                                        this._navigationServiceFacade, false);
                        tempList.Add(categoryViewModel);
                    }

                    Items = tempList;
                    IsBeingProcessed = false;
                });

            return new ObservableCollection<CategoryViewModel>();
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

        #region OpenAddCategoryCommand

        public ProxyCommand<CategoryListViewModel> OpenAddCategoryCommand { get; set; }

        #endregion //EditCategoryViewCommand

        #region EditCategoryViewCommand

        public ProxyCommand<CategoryListViewModel> EditCategoryViewCommand { get; set; }

        #endregion //EditCategoryViewCommand

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

        #endregion //DeleteCategoryCommand

        #endregion //Propriétés

        #region ChoiceCategoryCommand

        public ProxyCommand<CategoryListViewModel> ChoiceCategoryCommand { get; set; }

        #endregion //ChoiceCategoryCommand

        #region TapCommand

        public ProxyCommand<CategoryListViewModel> TapCommand { get; set; }

        #endregion // TapCommand

        private void LaunchSearch()
        {
            this.Refresh();
        }

        private void EditCategory()
        {

        }
    }
}
