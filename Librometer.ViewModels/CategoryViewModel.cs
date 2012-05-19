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

namespace Librometer.ViewModels
{
    public class CategoryViewModel : BaseViewModel
    {
        private INavigationServiceFacade _navigationServiceFacade;

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
        public CategoryViewModel() { }

        private IDialogService _windowServices;
        private ICategoryService _categoryService;

        public CategoryViewModel(
                    CategoryModel categoryModel, 
                    string pageTitle, 
                    INavigationServiceFacade navigationServiceFacade, 
                    bool synchronizedWithSelection)
        {
            this._category = categoryModel;
            this._pageTitle = pageTitle;
            this._navigationServiceFacade = navigationServiceFacade;
        }

        protected override void ServicesInitialization()
        {
            this._windowServices = ServiceLocator.Instance.Retrieve<IDialogService>();
            this._categoryService = ServiceLocator.Instance.Retrieve<ICategoryService>();
        }

        protected override void CommandsInitialization()
        {
            base.CommandsInitialization();
        }

        #region Propriétés

        #region CategoryModel

        private CategoryModel _category = null;
        public CategoryModel Category
        {
            get { return _category; }
            set
            {
                if (_category != null)
                {

                    _category = value;
                    RaisePropertyChanged<CategoryModel>(() => Category);
                }
            }
        }

        #endregion //CategoryModel

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


    }
}
