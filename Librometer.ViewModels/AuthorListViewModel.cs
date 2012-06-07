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

namespace Librometer.ViewModels
{
    public class AuthorListViewModel : BaseListViewModel<AuthorViewModel>
    {
        private IDialogService _windowServices;
        private IAuthorService _authorService;
        private INavigationServiceFacade _navigationServiceFacade;

        public AuthorListViewModel() { }
        public AuthorListViewModel(INavigationServiceFacade navigationServiceFacade)
        {
            this._navigationServiceFacade = navigationServiceFacade;
        }


        #region Méthodes d'initialisation

        protected override void ServicesInitialization()
        {
            _windowServices
                = ServiceLocator.Instance.Retrieve<IDialogService>();
            _authorService = ServiceLocator.Instance.Retrieve<IAuthorService>();
        }

        protected override void CommandsInitialization()
        {
            LaunchSearchCommand = new ProxyCommand<AuthorListViewModel>(
                (param) => LaunchSearch(),
                /*_ => !string.IsNullOrEmpty(this.SearchText)*/null,
            this);

            OpenAddAuthorCommand = new ProxyCommand<AuthorListViewModel>((_) =>
            {
                var viewModel = new AuthorViewModel(
                            new AuthorModel(),
                            "Créer un auteur",
                            this._navigationServiceFacade, false);//TODO:mettre la chaîne "Créer un auteur" dans une ressource
                viewModel.Author.BeginEdit();
                _windowServices
                            .OpenSaveOrCancelWindow<AuthorViewModel>(
                                    viewModel, this._navigationServiceFacade, (authorVM) =>
                            {
                                authorVM.Author.EndEdit();
                                var srv = ServiceLocator.Instance.Retrieve<IAuthorService>();
                                authorVM.Author.DisplayName = authorVM.Author.FirstName + " " +
                                            authorVM.Author.LastName;
                                bool ok = srv.Create(authorVM.Author);
                                srv.ApplyChanges();
                                return ok;

                            }, null);
            });

            DeleteAuthorCommand = new ProxyCommand<AuthorListViewModel>((_) =>
                {
                    if (_windowServices.AskConfirmation(
                                "Suppresion des auteurs",
                                "Voulez-vous supprimer votre sélection") == true)
                    {
                        //this.SearchText = this.SelectedItems.Count.ToString();
                        foreach(AuthorViewModel author in this.SelectedItems)
                        {
                            _authorService.Delete(author.Author);
                            if (author.Author.ADesErreurs == true)
                            {
                                MessageBox.Show(author.Author.Error);
                            }
                        }
                        _authorService.ApplyChanges();

                        this.IsBtnNewVisible = true;
                        this.IsBtnChoiceVisible = true;
                        this.IsBtnDeleteVisible = false;
                        //this.IsBtnDeleteEnabled = false;TODO: quand on saura intercepter la sélection d'un item
                        this.IsLstBoxInChooseState = false;
                    }
                });

            EditAuthorViewCommand = new ProxyCommand<AuthorListViewModel>((_) =>
                {
                });

            ChoiceAuthorCommand = new ProxyCommand<AuthorListViewModel>((_) =>
                {
                    this.IsBtnNewVisible = false;
                    this.IsBtnChoiceVisible = false;
                    this.IsBtnDeleteVisible = true;
                    this.IsLstBoxInChooseState = true;
                });

            TapCommand = new ProxyCommand<AuthorListViewModel>((param) =>
                {
                    if (this.IsBtnDeleteVisible == false)
                    {
                        AuthorViewModel selectedItem = param as AuthorViewModel;
                        if (selectedItem != null)
                        {
                            selectedItem.Author.BeginEdit();
                            _windowServices
                                        .OpenSaveOrCancelWindow(
                                                selectedItem,
                                                this._navigationServiceFacade,
                                // Méthodes appelée si sauvegarde
                                                (authorVM) =>
                                                {
                                                    authorVM.Author.EndEdit();
                                                    var srv = ServiceLocator.Instance.Retrieve<IAuthorService>();
                                                    authorVM.Author.DisplayName = authorVM.Author.FirstName + " " +
                                                                authorVM.Author.LastName;
                                                    bool ok = srv.Update(authorVM.Author);
                                                    srv.ApplyChanges();
                                                    return ok;
                                                },
                                // Si annulation on annule les changements
                                                (_) =>
                                                {
                                                    selectedItem.Author.CancelEdit(); return true;
                                                });
                        }
                    }
                });
        }

        protected override System.Collections.ObjectModel.ObservableCollection<AuthorViewModel> LoadItems()
        {
            IsBeingProcessed = true;
            // Obtention des marques pages et création des ViewModel correspondant
            AuthorCriteria searchCriteria = AuthorCriteria.Empty;
            searchCriteria.LastName = this.SearchText;

            _authorService.GetAsync(searchCriteria, (authorResponse, authors) =>
                {
                    if (authorResponse.HasError)
                    {
                        // Traitement de l'erreur
                        IsBeingProcessed = false;
                        return;
                    }
                    ObservableCollection<AuthorViewModel> tempList
                        = new ObservableCollection<AuthorViewModel>();
                    foreach (AuthorModel authorModel in authors)
                    {
                        //TODO: mettre le titre dans une ressource
                        AuthorViewModel categoryViewModel =
                            new AuthorViewModel(
                                        authorModel, "Auteurs",
                                        this._navigationServiceFacade, false);
                        tempList.Add(categoryViewModel);
                    }

                    Items = tempList;
                    IsBeingProcessed = false;
                });

            return new ObservableCollection<AuthorViewModel>();
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

        #region OpenAddAuthorCommand

        public ProxyCommand<AuthorListViewModel> OpenAddAuthorCommand { get; set; }

        #endregion //OpenAddAuthorCommand

        #region EditAuthorViewCommand

        public ProxyCommand<AuthorListViewModel> EditAuthorViewCommand { get; set; }

        #endregion //EditAuthorViewCommand

        #region LaunchSearchCommand

        private ProxyCommand<AuthorListViewModel> _launchSearchCommand = null;

        public ProxyCommand<AuthorListViewModel> LaunchSearchCommand
        {
            get { return _launchSearchCommand; }
            set
            {
                if (_launchSearchCommand == value) return;
                _launchSearchCommand = value;
                RaisePropertyChanged<ProxyCommand<AuthorListViewModel>>(() => LaunchSearchCommand);
            }
        }

        #endregion //DeleteAuthorCommand

        #region DeleteCategoryCommand

        public ProxyCommand<AuthorListViewModel> DeleteAuthorCommand { get; set; }

        #endregion //DeleteAuthorCommand

        #endregion //Propriétés

        #region ChoiceAuthorCommand

        public ProxyCommand<AuthorListViewModel> ChoiceAuthorCommand { get; set; }

        #endregion //ChoiceAuthorCommand

        #region TapCommand

        public ProxyCommand<AuthorListViewModel> TapCommand { get; set; }

        #endregion // TapCommand

        private void LaunchSearch()
        {
            this.Refresh();
        }
    }
}
