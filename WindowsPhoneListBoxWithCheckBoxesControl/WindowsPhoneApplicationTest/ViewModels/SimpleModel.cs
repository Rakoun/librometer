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
using System.ComponentModel;

namespace WindowsPhoneApplicationTest.ViewModels {
    public class SimpleModel : INotifyPropertyChanged {
        
        protected string itsName;

        protected string itsDescription;



        public event PropertyChangedEventHandler PropertyChanged;



        public string Name {
            get { return this.itsName; }
            set {
                this.itsName = value;
                NotifyPropertyChanged("Name");
            }
        }

        public string Description {
            get { return this.itsDescription; }
            set {
                this.itsDescription = value;
                NotifyPropertyChanged("Description");
            }
        }



        protected void NotifyPropertyChanged(string thePropertyName) {
            if (this.PropertyChanged != null) {
                this.PropertyChanged(this, new PropertyChangedEventArgs(thePropertyName));
            }
        }
    }
}
