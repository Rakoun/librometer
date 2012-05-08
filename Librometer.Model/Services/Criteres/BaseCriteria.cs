using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Librometer.Model.Services
{
    /// <summary>
    /// Définit un critère de recherche pour les méthodes
    /// des services d'accès aux données
    /// </summary>
    /// <typeparam name="T">Le type concret du critère</typeparam>
    public abstract class BaseCriteria<T> where T : BaseCriteria<T>, new()
    {
        public static T Empty { get { return new T(); } }


        /// <summary>
        /// Taille limite du jeu d'éléments renvoyés.
        /// </summary>
        public int TailleLimite { get; set; }
    }

    public class BaseCriteria : BaseCriteria<BaseCriteria> { }
}
