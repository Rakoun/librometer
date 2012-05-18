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
    public class CameraViewModel : BaseViewModel
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
        public CameraViewModel() { }

        public CameraViewModel(string pageTitle, bool synchronizedWithSelection)
        {
            this._pageTitle = pageTitle;
        }

        protected override void ServicesInitialization()
        {
        }

        protected override void CommandsInitialization()
        {
            base.CommandsInitialization();
        }

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

        public ProxyCommand<BookViewModel> TakePhotoCommand { get; set; }

    }
}
