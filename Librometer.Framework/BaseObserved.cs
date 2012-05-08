using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Librometer.Framework
{
    public class BaseObserved
        : INotifyPropertyChanged, INotifyPropertyChanging
    {
        #region INotifyPropertyChanging Members
        public event PropertyChangingEventHandler PropertyChanging;

        /// <summary>
        /// Déclenche l'événement PropertyChanging pour une propriété donnée.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exp">L'expression permettant de retrouver la propriété.</param>
        protected void RaisePropertyChanging<T>(Expression<Func<T>> exp)
        {
            var memberExpression = exp.Body as MemberExpression;
            if (memberExpression != null)
            {
                string propertyName = memberExpression.Member.Name;
                PropertyChangingEventHandler handler = PropertyChanging;
                if (handler != null)
                    handler(this, new PropertyChangingEventArgs(propertyName));
            }
        }
        #endregion // INotifyPropertyChanged Members

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Déclenche l'événement PropertyChanged pour une propriété donnée.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exp">L'expression permettant de retrouver la propriété.</param>
        protected void RaisePropertyChanged<T>(Expression<Func<T>> exp)
        {
            var memberExpression = exp.Body as MemberExpression;
            if (memberExpression != null)
            {
                string propertyName = memberExpression.Member.Name;
                RaisePropertyChanged(propertyName);
            }
        }
        /// <summary>
        /// Déclenche l'événement PropertyChanged pour une propriété donnée.
        /// </summary>
        /// <param name="propertyName">Le nom de la propriété.</param>
        private void RaisePropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));

        }

        public void RaisePropertyChangedEntier()
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(string.Empty));
        }

        #endregion // INotifyPropertyChanged Members

    }
}
