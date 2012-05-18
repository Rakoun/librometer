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

        #region ApplicationBar (ce code casse un peu le pattern MVVM)

        public string Item1Label { get; set; }
        public string Item2Label { get; set; }

        #region Command associé à deux item de l'applicationbar

        public ProxyCommand<BaseViewModel> Item1Command { get; set; }
        public ProxyCommand<BaseViewModel> Item2Command { get; set; }

        #endregion//Command associé à deux item de l'applicationbar

        #endregion//ApplicationBar

    }
}
