using System;
using System.Net;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace WPExtensions.Helpers
{
    [KnownType(typeof(BaseMenuItemButton))]
    [KnownType(typeof(BaseIconItemButton))]
    public class BaseButton
    {
        public string Text { get; set; }
        public bool IsVisible { get; set; }
        public bool IsEnabled { get; set; }
    }

    public class BaseMenuItemButton : BaseButton
    {


        
    }

    public class BaseIconItemButton : BaseButton
    {

        public Uri IconUri { get; set; }
    }
}
