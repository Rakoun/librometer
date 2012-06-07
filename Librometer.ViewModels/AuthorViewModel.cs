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
    public class AuthorViewModel : BaseViewModel
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
        public AuthorViewModel() { }

        private IDialogService _windowServices;
        private IAuthorService _authorService;

        public AuthorViewModel(
                    AuthorModel authorModel, 
                    string pageTitle, 
                    INavigationServiceFacade navigationServiceFacade, 
                    bool synchronizedWithSelection)
        {
            this._author = authorModel;
            this._pageTitle = pageTitle;
            this._navigationServiceFacade = navigationServiceFacade;
        }

        protected override void ServicesInitialization()
        {
            this._windowServices = ServiceLocator.Instance.Retrieve<IDialogService>();
            this._authorService = ServiceLocator.Instance.Retrieve<IAuthorService>();
        }

        protected override void CommandsInitialization()
        {
            base.CommandsInitialization();
        }

        #region Propriétés

        #region AuthorModel

        private AuthorModel _author = null;
        public AuthorModel Author
        {
            get { return _author; }
            set
            {
                if (_author != null)
                {

                    _author = value;
                    RaisePropertyChanged<AuthorModel>(() => Author);
                }
            }
        }

        #endregion //AuthorModel

        #region PageTitle

        private string _pageTitle;
        public string PageTitle
        {
            get { return this._pageTitle; }
            set { this._pageTitle = value; }
        }

        #endregion //PageTitle

        #endregion //Propriétés


    }
}
