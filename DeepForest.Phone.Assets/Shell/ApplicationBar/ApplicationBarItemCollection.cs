using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using Microsoft.Phone.Shell;

namespace DeepForest.Phone.Assets.Shell
{
    public abstract class ApplicationBarItemCollection<T> : DependencyObjectCollection<T> where T : ApplicationBarMenuItem
    {        
        internal void Attach(object dataContext, IApplicationBar sysAppBar)
        {
            foreach (var item in this)
            {
                item.DataContext = dataContext;
                item.Attach(sysAppBar);
            }
        }

        internal void Dettach(IApplicationBar sysAppBar)
        {
            foreach (var item in this)
            {
                item.Dettach(sysAppBar);
                item.DataContext = null;
            }
        }        
    }

    public class ApplicationBarMenuItemCollection : ApplicationBarItemCollection<ApplicationBarMenuItem>
    {
    }

    public class ApplicationBarIconButtonCollection : ApplicationBarItemCollection<ApplicationBarIconButton>
    {
    }
}
