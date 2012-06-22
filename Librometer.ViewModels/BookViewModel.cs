using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Librometer.Framework;
using Librometer.Model;
using Librometer.Framework.IOC;
using Librometer.Model.Services;
using Librometer.Adapters;
using System.Collections;

namespace Librometer.ViewModels
{
    public class BookViewModel : BaseViewModel
    {
        public enum States
        {
            Perfect,
            HasErrors
        }

        #region States
        private States _state = States.Perfect;
        public States EtatCourant
        {
            get { return _state; }
            set
            {
                if (_state == value) return;
                _state = value;
                RaisePropertyChanged<States>(() => EtatCourant);
            }
        }
        #endregion  //State

        // Pour Blend
        public BookViewModel() { }

        // Constructeur de copie
        public BookViewModel(BookViewModel bvm)
        {
            this._authors = bvm._authors;
            this._authorService = bvm._authorService;
            this._book = bvm.Book;
            this._categories = bvm._categories;
            this._categoryService = bvm._categoryService;
            this._isNewPhoto = bvm._isNewPhoto;
            this._navigationServiceFacade = bvm._navigationServiceFacade;
            this._pageTitle = bvm._pageTitle;
            this._rates = bvm._rates;
            this._selectedAuthor = bvm._selectedAuthor;
            this._selectedCategory = bvm._selectedCategory;
            this._state = bvm._state;
            this._windowServices = bvm._windowServices;
        }

        private IDialogService _windowServices;
        private IAuthorService _authorService;
        private ICategoryService _categoryService;
        private INavigationServiceFacade _navigationServiceFacade;

        public BookViewModel(
                    BookModel bookModel, string pageTitle,
                    INavigationServiceFacade navigationServiceFacade,
                    bool synchronizedWithSelection)
        {
            this._book = bookModel;
            this._pageTitle = pageTitle;
            this._navigationServiceFacade = navigationServiceFacade;

            if (_categoryService != null)
                this._categories =
                    new ObservableCollection<CategoryModel>(this._categoryService.GetByCriteria(CategoryCriteria.Empty));

            if (_authorService != null)
                this._authors =
                    new ObservableCollection<AuthorModel>(this._authorService.GetByCriteria(AuthorCriteria.Empty));

            #region Initialisation supplémentaire pour problème dans le ListPicker

            IEnumerator enumcat = this._categories.GetEnumerator();
            enumcat.MoveNext();
            _selectedCategory = enumcat.Current as CategoryModel;

            IEnumerator enumaut = this._authors.GetEnumerator();
            enumaut.MoveNext();
            _selectedAuthor = enumaut.Current as AuthorModel;

            #endregion // Initialisation supplémentaire pour problème dans le ListPicker




        }

        protected override void ServicesInitialization()
        {
            this._windowServices = ServiceLocator.Instance.Retrieve<IDialogService>();
            this._authorService = ServiceLocator.Instance.Retrieve<IAuthorService>();
            this._categoryService = ServiceLocator.Instance.Retrieve<ICategoryService>();
        }

        protected override void CommandsInitialization()
        {
            DisplayCameraCommand = new ProxyCommand<BookViewModel>((_)=>
                {
                    BookViewModel updatedDataContext = new BookViewModel(this);
                    _windowServices.LaunchCameraCaptureTask<BookViewModel>(this, updatedDataContext, this._navigationServiceFacade);
                });
        }

        #region Propriétés

        #region BookModel

        private BookModel _book = null;
        public BookModel Book
        {
            get { return _book; }
            set
            {
                if (_book != null)
                {

                    _book = value;
                    RaisePropertyChanged<BookModel>(() => Book);
                }
            }
        }

        #endregion //BookModel

        #region Categories

        private ObservableCollection<CategoryModel> _categories;
        public ObservableCollection<CategoryModel> Categories
        {
            get { return _categories; }
            set
            {
                if (_categories == value) return;

                _categories = value;
                RaisePropertyChanged<ObservableCollection<CategoryModel>>(() => Categories);
            }
        }

        #endregion //Categories

        #region Authors

        private ObservableCollection<AuthorModel> _authors;
        public ObservableCollection<AuthorModel> Authors
        {
            get { return _authors; }
            set
            {
                if (_authors == value) return;

                _authors = value;
                RaisePropertyChanged<ObservableCollection<AuthorModel>>(() => Authors);
            }
        }

        #endregion //Authors

        #region PageTitle

        private string _pageTitle;
        public string PageTitle
        {
            get { return this._pageTitle; }
            set
            {
                if (this._pageTitle == value) return;

                this._pageTitle = value;
                RaisePropertyChanged<string>(() => PageTitle);
            }
        }

        #endregion //PageTitle



        #region Rates

        private int[] _rates = new int[] { 1, 2, 3, 4, 5 };
        public int [] Rates
        {
            get { return _rates; }
            set
            {
                if (_rates == value) return;
                _rates = value;
                RaisePropertyChanged<int[]>(() => Rates);
            }
        }

        #endregion  //State

        #region SelectedCategory

        private CategoryModel _selectedCategory;
        public CategoryModel SelectedCategory
        {
            get { return _selectedCategory; }
            set
            {
                if (_selectedCategory == value) return;
                if (_selectedCategory == null)
                {
                    IEnumerator enumcat = this._categories.GetEnumerator();
                    enumcat.MoveNext();
                    _selectedCategory = enumcat.Current as CategoryModel;
                }

                _selectedCategory = value;
                RaisePropertyChanged<CategoryModel>(() => SelectedCategory);
            }
        }

        #endregion // SelectedCategory

        #region SelectedAuthor

        private AuthorModel _selectedAuthor;
        public AuthorModel SelectedAuthor
        {
            get { return _selectedAuthor; }
            set
            {
                if (_selectedAuthor == value) return;
                if (_selectedAuthor == null)
                {
                    IEnumerator enumaut = this._authors.GetEnumerator();
                    enumaut.MoveNext();
                    _selectedAuthor = enumaut.Current as AuthorModel;
                }

                _selectedAuthor = value;
                RaisePropertyChanged<AuthorModel>(() => SelectedAuthor);
            }
        }

        #endregion // SelectedCategory

        #region IsNewPhoto

        private bool _isNewPhoto;
        public bool IsNewPhoto
        {
            get
            {
                //TODO: il y a peut-être mieux
                if (ParamOne == "NewPhoto")
                {
                    _isNewPhoto = true;
                }
                else
                {
                    _isNewPhoto = false;
                }
                return _isNewPhoto;
            }
            set
            {
                if (_isNewPhoto == value) return;

                _isNewPhoto = value;
                RaisePropertyChanged<bool>(() => IsNewPhoto);
            }
        }

        #endregion // IsNewPhoto

        #endregion //Propriétés

        public ProxyCommand<BookViewModel> DisplayCameraCommand { get; set; }

        public override string ToString()
        {
            return "base";
        }

    }
}
