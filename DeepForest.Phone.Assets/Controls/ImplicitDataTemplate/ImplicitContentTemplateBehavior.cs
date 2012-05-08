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
using System.Windows.Data;
using System.Windows.Interactivity;

namespace DeepForest.Phone.Assets.Controls
{
    public class ImplicitContentTemplateBehavior : Behavior<ContentControl>
    {
        protected override void OnAttached()
        {
            var binding = new Binding("Content")
            {
                Mode = BindingMode.OneWay,
                Source = AssociatedObject,
                Converter = new DataTemplateConverter(AssociatedObject),
            };

            BindingOperations.SetBinding(AssociatedObject, ContentControl.ContentTemplateProperty, binding);
            
            base.OnAttached();
        }

        private class DataTemplateConverter : IValueConverter
        {
            private ContentControl _contentControl;

            public DataTemplateConverter(ContentControl contentControl)
            {
                this._contentControl = contentControl;
            }

            #region IValueConverter Members

            public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                return ImplicitDataTemplateResolver.Resolve(_contentControl);
            }

            public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                throw new NotSupportedException();
            }

            #endregion
        }
    }
}
