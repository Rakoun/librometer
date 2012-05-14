using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Librometer.Adapters;

namespace Librometer.Framework
{
    public interface IDialogService
    {
        void OpenSaveOrCancelWindow<T>(string title,
                    T dataContext, INavigationServiceFacade nav, Predicate<T> actionIfOK,
                    Predicate<T> actionIfKO);

        bool AskConfirmation(string title, string message);
        void DisplayInformation(string title, string message);
    }
}
