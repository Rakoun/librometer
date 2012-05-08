using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Linq.Expressions;
//using System.ComponentModel.DataAnnotations;
using Librometer.DataAnnotations;
using System.Reflection;

namespace Librometer.Model
{
    public partial class BaseModel : INotifyPropertyChanged, INotifyPropertyChanging, IDataErrorInfo
    {
        public BaseModel()
        {
            // Abonnement à la méthode de validation
            this.PropertyChanged += ValidateProperty;
            this.PropertyChanged += Editable_PropertyChanged;
            this.PropertyChanging += Editable_PropertyChanging;
        }

        public  int _id = -1;
        /// <summary>
        /// L'identifiant de cette entité
        /// </summary>
        public virtual int Id
        {
            get { return _id; }
            set
            {
                if (value != _id)
                {
                    _id = value;
                    RaisePropertyChanged(() => Id);
                }
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Déclenche l’événement PropertyChanged pour une propriété
        /// donnée.
        /// </summary>
        /// <typeparam name=”T”></typeparam>
        /// <param name=”exp”>L’expression permettant de retrouver la
        /// propriété.</param>
        protected void RaisePropertyChanged<T>(Expression<Func<T>> exp)
        {
            var memberExpression = exp.Body as MemberExpression;
            if (memberExpression != null)
            {
                string propertyName = memberExpression.Member.Name;
                PropertyChangedEventHandler handler = PropertyChanged;
                if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion // INotifyPropertyChanged Members

        #region INotifyPropertyChanging Members

        public event PropertyChangingEventHandler PropertyChanging;
        /// <summary>
        /// Déclenche l’événement PropertyChanging pour une propriété
        /// donnée.
        /// </summary>
        /// <typeparam name=”T”></typeparam>
        /// <param name=”exp”>L’expression permettant de retrouver la
        /// propriété.</param>
        protected void RaisePropertyChanging<T>(Expression<Func<T>> exp)
        {
            var memberExpression = exp.Body as MemberExpression;
            if (memberExpression != null)
            {
                string propertyName = memberExpression.Member.Name;
                PropertyChangingEventHandler handler = PropertyChanging;
                if (handler != null) handler(this, new PropertyChangingEventArgs(propertyName));
            }
        }
        #endregion // INotifyPropertyChanging Members

        #region IDataErrorInfo implementation
        public void ReplaceGolablValidationErrors(List<string> listeErreurs)
        {
            _errors = listeErreurs
                .Aggregate<string>((agg, err) => agg + Environment.NewLine + err);
            RaisePropertyChanged(() => Error);
        }

        private string _errors = String.Empty;
        public string Error
        {
            get
            {
                return validationErrors
                      .Values.Aggregate(_errors
                      , (agg, err) => agg + Environment.NewLine + err);
            }
        }

        private readonly Dictionary<string, string> validationErrors
            = new Dictionary<string, string>();
        private void ValidateProperty(object sender, PropertyChangedEventArgs e)
        {
            //Creation du contexte de validation
            string propertyName = e.PropertyName;
            if (String.IsNullOrEmpty(propertyName)
                || string.Equals(e.PropertyName, "ADesErreurs")) return;
            ValidationContext context =
                new ValidationContext(this, null, null)
                {
                    MemberName = propertyName,
                    DisplayName = propertyName,
                };

            ICollection<ValidationResult> validationResults
                = new List<ValidationResult>();

            //Obtention de la valeur de la propriete
            PropertyInfo proprieteInfo = this.GetType()
                .GetProperty(propertyName);
            if (proprieteInfo == null) return;
            object valeurDeLaPropriete = proprieteInfo.GetValue(this, null);

            //Application des règles de validation
            Validator
                .TryValidateProperty(valeurDeLaPropriete, context, validationResults);

            //Construction du message d'erreur
            string errors =
                validationResults
                .Aggregate<ValidationResult, string>(string.Empty,
                (a, b) => a += Environment.NewLine + b.ErrorMessage);

            if (!String.IsNullOrEmpty(errors))
            {
                errors = String.Format("[{0}]:{1}", propertyName, errors);
                validationErrors[propertyName] = errors;
            }
            else
            {
                validationErrors.Remove(propertyName);
            }

            RaisePropertyChanged<Boolean>(() => ADesErreurs);
        }

        public string this[string columnName]
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Indique si le modèle courant possède des erreurs
        /// </summary>
        public bool ADesErreurs
        {
            get
            {
                return !String.IsNullOrEmpty(Error)
                    || validationErrors.Values.Count(err => !String.IsNullOrEmpty(err)) > 0;
            }
        }

        #endregion
    }
}
