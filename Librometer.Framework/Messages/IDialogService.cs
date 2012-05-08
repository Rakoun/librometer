using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Librometer.Framework
{
    public interface IDialogService
    {
        void OpenSaveOrCancelWindow<T>(string title, double heigh,
                    double width, T data, Predicate<T> actionIfOK,
                    Predicate<T> actionIfKO);

        bool AskConfirmation(string title, string message);
        void DisplayInformation(string title, string message);
    }
}
