using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Librometer.Adapters;

namespace Librometer.Framework
{
    public interface IDialogService
    {
        void OpenSaveOrCancelWindow<T>(
                    T dataContext, INavigationServiceFacade nav, Predicate<T> actionIfOK,
                    Predicate<T> actionIfKO);

        void OpenNewPage(Uri uri, INavigationServiceFacade nav);

        bool AskConfirmation(string title, string message);
        void DisplayInformation(string title, string message);

        void LaunchCameraCaptureTask();
    }
}
