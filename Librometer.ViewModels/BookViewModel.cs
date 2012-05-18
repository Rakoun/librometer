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

        private IDialogService _windowServices;
        private IAuthorService _authorService;
        private ICategoryService _categoryService;

        public BookViewModel(BookModel bookModel, string pageTitle, bool synchronizedWithSelection)
        {
            this._book = bookModel;
            this._pageTitle = pageTitle;
            Item1Label = "Catégories";//TODO: mettre dans une ressource
            Item2Label = "Auteurs";//TODO: mettre dans une ressource

            if (_categoryService != null)
                this._categories =
                    new ObservableCollection<CategoryModel>(this._categoryService.GetByCriteria(CategoryCriteria.Empty));

            if (_authorService != null)
                this._authors =
                    new ObservableCollection<AuthorModel>(this._authorService.GetByCriteria(AuthorCriteria.Empty));
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
                    _windowServices.LaunchCameraCaptureTask();
                });

            Item1Command = new ProxyCommand<BaseViewModel>((_)=>
            {
                MessageBox.Show("Item1Command");
            });

            Item2Command = new ProxyCommand<BaseViewModel>((_) =>
            {
                MessageBox.Show("Item2Command)");
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
            set { this._pageTitle = value; }
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

        #endregion //Propriétés

        public ProxyCommand<BookViewModel> DisplayCameraCommand { get; set; }

    }
}
