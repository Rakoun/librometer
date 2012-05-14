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
    public class BookmarkViewModel : BaseViewModel
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
        public BookmarkViewModel() { }

        public BookmarkViewModel(BookmarkModel bookmarkModel, bool synchronizedWithSelection)
        {
            this._bookmark = bookmarkModel;

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

        private BookmarkModel _bookmark = null;

        public BookmarkModel Bookmark
        {
            get { return _bookmark; }
            set
            {
                if (_bookmark != null)
                {

                    _bookmark = value;
                    RaisePropertyChanged<BookmarkModel>(() => Bookmark);
                }
            }
        }

        #endregion //Propriétés


    }
}
