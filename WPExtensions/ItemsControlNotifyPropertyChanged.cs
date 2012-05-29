using System.ComponentModel;
using System.Windows;

namespace WPExtensions
{
    public class ItemsControlNotifyPropertyChanged : FrameworkElement, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

    }
}