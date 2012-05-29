using System;
using Microsoft.Phone.Shell;
using WPExtensions;
using System.Windows;

namespace WPExtensions
{
    public class AdvancedApplicationBarMenuItem : BaseAppBarItem, IApplicationBarMenuItem
    {
        public AdvancedApplicationBarMenuItem()
        {
            ApplicationBarMenuItem = new ApplicationBarMenuItem() { Text = "appbar" }; ;
            RealAppBarItem = ApplicationBarMenuItem;
            AdvancedApplicationBar.AddButton(this);
            ApplicationBarMenuItem.Click += new EventHandler(ApplicationBarMenuItem_Click);
        }

        void ApplicationBarMenuItem_Click(object sender, EventArgs e)
        {
            if(this.DisableLostFocus)
            {
                base.AppBarClick(this, e);   
            }
            else
            {
                UIHelper.GetCurrentPage().Focus();
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    base.AppBarClick(this, e);
                });
            }
            
        }

        
 
        public ApplicationBarMenuItem ApplicationBarMenuItem { get; set; }

    }
}
