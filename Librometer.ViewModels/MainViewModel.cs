using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Librometer.Framework;

namespace Librometer.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        protected override void ServicesInitialization()
        {
            //TOOD:
            base.ServicesInitialization();
        }

        protected override void CommandsInitialization()
        {
            QuitApplicationCommand = new ProxyCommand<BookmarkListViewModel>(
                (param) => LaunchSearch(),
                /*_ => !string.IsNullOrEmpty(this.SearchText)*/null,
            this);
        }

        #region Elements de Menu

        public string Settings
        {
            get { return "Paramétrages"; }
        }

        public string Credits
        {
            get { return "Crédits"; }
        }

        #endregion

        /* RGE public ProxyCommand<MainViewModel> QuitApplicationCommand { get; set; }*/
        public ProxyCommand<MainViewModel> DisplayParametersCommand { get; set; }
        public ProxyCommand<MainViewModel> DisplayAboutCommand { get; set; }
        public ProxyCommand<MainViewModel> OpenAddBookCommand { get; set; }
    }
}
 