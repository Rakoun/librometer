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
        /// <summary>
        /// Permet d'exécuter des métodes modifiant le contenu graphique
        /// de la page courante.
        /// </summary>
        /// <remarks>
        /// Ce code permet de ne pas briser le pattern MVVM en évitant de lier
        /// le "ViewModel" et le "View".
        /// </remarks>
        /// <param name="nav"></param>
        /// <param name="editMethodName"></param>
        //code mort void EditCurrentPage<T>(INavigationServiceFacade nav, string editMethodName, T dataContext);

        bool AskConfirmation(string title, string message);
        void DisplayInformation(string title, string message);

        void LaunchCameraCaptureTask<T>(T datacontext, T updatedDataContext, INavigationServiceFacade nav);
    }
}
