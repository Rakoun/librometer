using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace DeepForest.Phone.Assets.Controls
{
    internal partial class ImplicitDataTemplateResources : ResourceDictionary
    {
        private static ImplicitDataTemplateResources _instance = new ImplicitDataTemplateResources();

        internal static ImplicitDataTemplateResources Instance
        {
            get { return _instance; }
        }

        internal DataTemplate ImplicitDataTemplate
        {
            get; private set;
        }

        private ImplicitDataTemplateResources()
        {
            InitializeComponent();
            ImplicitDataTemplate = (DataTemplate)this["ImplicitDataTemplate_240C1930-91F0-4F08-A59C-20D0F9AE34C1"];
        }
    }
}
