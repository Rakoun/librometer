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

namespace Librometer.Framework
{
    public class DialogService : IDialogService
    {
        public void OpenSaveOrCancelWindow<T>(string title, double heigh,
                    double width, T data, Predicate<T> actionIfOK,
                    Predicate<T> actionIfKO)
        {
        }

        public bool AskConfirmation(string title, string message)
        {
            return MessageBoxResult.Yes ==
                MessageBox.Show(message, title, MessageBoxButton.OKCancel);
        }

        public void DisplayInformation(string title, string message)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK);
        }
    }
}
