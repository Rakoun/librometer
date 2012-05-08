using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Librometer.Framework
{
    public class ViewModelLocator
    {
        private readonly static Dictionary<string, BaseViewModel> _viewModelCache
            = new Dictionary<string, BaseViewModel>();

        public BaseViewModel this[string key]
        {
            get
            {
                BaseViewModel viewModel;
                _viewModelCache.TryGetValue(key, out viewModel);
                return viewModel;
            }
        }

        /// <summary>
        /// Enregistre un viewModel en fournissant un nom.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="viewModel"></param>
        public static void RegisterViewModel(string key, BaseViewModel viewModel)
        {
            _viewModelCache.Add(key, viewModel);

        }

        /// <summary>
        /// Enregistre un ViewModel en utilisant le nom du type fournit comme 
        /// clef d'accès au ViewModel
        /// </summary>
        public static void RegisterViewModel<TViewModel>(BaseViewModel viewModel)
        {
            RegisterViewModel(typeof(TViewModel).Name, viewModel);
        }
        /// <summary>
        /// Enregistre un ViewModel en lui utilisant comme nom le type du
        /// ViewModel.
        /// </summary>
        public static void RegisterViewModel(BaseViewModel viewModel)
        {
            RegisterViewModel(viewModel.GetType().Name, viewModel);
        }
    }
}
