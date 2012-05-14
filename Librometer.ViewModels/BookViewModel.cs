using System;
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

        public BookViewModel(BookModel bookModel, bool synchronizedWithSelection)
        {
            this._book = bookModel;

        }

        protected override void ServicesInitialization()
        {
            base.ServicesInitialization();
        }

        protected override void CommandsInitialization()
        {
            base.CommandsInitialization();
        }

        #region Propriétés

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

        public string TEST
        {
            get { return "bonjour"; }
        }

        #endregion //Propriétés
    }
}
