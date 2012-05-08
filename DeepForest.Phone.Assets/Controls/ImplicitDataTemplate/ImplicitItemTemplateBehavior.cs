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
using System.Windows.Interactivity;
using System.Windows.Controls.Primitives;
using System.Collections.Specialized;

namespace DeepForest.Phone.Assets.Controls
{
    public class ImplicitItemTemplateBehavior : Behavior<ItemsControl>
    {
        protected override void OnAttached()
        {
            AssociatedObject.ItemTemplate = ImplicitDataTemplateResources.Instance.ImplicitDataTemplate;
            
            base.OnAttached();
        }

        protected override void OnDetaching()
        {
            AssociatedObject.ItemTemplate = null;

            base.OnDetaching();
        }
    }
}
