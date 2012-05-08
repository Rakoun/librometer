using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;

namespace Librometer.Model
{
    public partial class BaseModel : IEditableObject
    {
        private Dictionary<PropertyInfo, object> _valuesCache;
        private readonly static Dictionary<Type, IEnumerable<PropertyInfo>> _reflexionCache
            = new Dictionary<Type, IEnumerable<PropertyInfo>>();
        private readonly static object _locker = new object();

        public bool HasBeenModified { get; set; }
        public bool IsNew { get { return -1 == Id; } }
        public bool IsBeingEdited { get; private set; }
        public bool IsActuallyEdited { get; private set; }
        public bool AutoEdit { get; set; }

        private void Editable_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //Met à jour EstReelementEdite
            if (IsBeingEdited) IsActuallyEdited = true;
        }
        private void Editable_PropertyChanging(object sender, PropertyChangingEventArgs e)
        {
            //Si on est en mode auto-édition : début d'édition.
            if (AutoEdit && !IsBeingEdited) BeginEdit();
        }

        /// <summary>
        /// Retourne une liste des prorpriétés de la classe obtenues
        /// via de la reflexion. Celles-ci sont cachées pour de meilleures
        /// performances.
        /// </summary>
        private IEnumerable<PropertyInfo> GetClassProperties()
        {
            IEnumerable<PropertyInfo> classProperties;
            if (!_reflexionCache.TryGetValue(GetType(), out classProperties))
            {
                lock (_locker)
                {
                    if (_reflexionCache.TryGetValue(GetType(), out classProperties))
                        return classProperties;

                    classProperties = GetType()
                        .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                        .Where(p => p.CanRead && p.CanWrite);

                    _reflexionCache.Add(GetType(), classProperties);

                }
            }

            return classProperties;
        }

        /// <summary>
        /// Début de l'édition de l'objet courant.
        /// </summary>
        public void BeginEdit()
        {
            if (IsBeingEdited) return;
            IsBeingEdited = true;
            IEnumerable<PropertyInfo> classProperties
                = GetClassProperties();

            _valuesCache =
                classProperties.ToDictionary<PropertyInfo, PropertyInfo, object>
                (p => p, p => p.GetValue(this, null));
        }

        /// <summary>
        /// Annule les changements effectués
        /// </summary>
        public void CancelEdit()
        {
            if (IsBeingEdited && IsActuallyEdited)
            {
                HasBeenModified = true;
                IEnumerable<PropertyInfo> classProperties
                    = GetClassProperties();

                foreach (PropertyInfo item in classProperties)
                {
                    item.SetValue(this, _valuesCache[item], null);
                }
                _valuesCache = null;
                IsBeingEdited = IsActuallyEdited = false;
            }
            else
            {
                EndEdit();
            }
        }

        /// <summary>
        /// Validation de l'édition.
        /// </summary>
        public void EndEdit()
        {
            if (!IsBeingEdited) return;
            HasBeenModified = true;
            _valuesCache = null;
            IsBeingEdited = IsActuallyEdited = false;
        }
    }
}
