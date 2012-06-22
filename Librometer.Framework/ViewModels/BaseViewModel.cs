using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Librometer.Framework
{
/// <summary>
    /// Classe de base pour tous les ViewModel
    /// </summary>
    public class BaseViewModel : BaseObserved
    {
        public BaseViewModel()
        {
            CommandsInitialization();
            ServicesInitialization();
        }

        protected virtual void ServicesInitialization() { }
        protected virtual void CommandsInitialization() { }

        public event EventHandler LocalCanExecuteChanged;

        /// <summary>
        /// Déclenche l'événément indiquant que les conditions
        /// d'éxecution des commandes doivent être réévaluées.
        /// </summary>
        protected void TriggereLocalCanExecuteChanged()
        {
            EventHandler handler = LocalCanExecuteChanged;
            if (handler != null)
                handler.Invoke(this, EventArgs.Empty);
        }

        public string ApplicationName
        {
            get {return "Librometer";}//TODO: à mettre en ressource
        }

        #region ParamOne

        private string _paramOne;
        /// <summary>
        /// TODO: il y a peut-être mieux
        /// ParamOne s'utilise conjointement avec la propriété IsNewPhoto de la classe
        /// BookViewModel. La problématique était de pouvoir mettre à jour le BookViewModel
        /// , DataContext de la vue dynamique SaveOrCancelPage. En effet lors d'un clic
        /// sur le bouton permettant de prendre une photo, on veut être capable de savoir
        /// si la photo a été prise ou pas. Si elle a été prise, celà signifie qu'une nouvelle
        /// photo est associée au livre donc qu'on doit la sauvegarder dans le téléphone.
        /// Le DataContext, ici BookViewModel étant passé à la méthode template LaunchCameraCaptureTask,
        /// nous n'avons aucune information sur son type donc nous ne pouvons pas affecter à true la
        /// propriété IsNewPhoto en l'état => en effet il aurait fallu modifier la dépendance
        /// entre les projets. Dans notre cas on a une référence circulaire si on essaye d'ajouter la 
        /// référence au projet Librometer.Views au projet Librometer.Framework. Etant donné que
        /// la classe BaseViewModel est dans le projet Librometer.Framework on en profite pour y ajotuer
        /// notre nouvelle propriété. On s'en servira dans BookViewModel pour positionner la valeur
        /// de IsNewPhoto.
        /// </summary>
        public string ParamOne
        {
            get { return _paramOne; }
            set
            {
                if (_paramOne == value) return;

                _paramOne = value;
                RaisePropertyChanged<string>(() => ParamOne);
            }
        }

        #endregion // ParamOne
    }
}
