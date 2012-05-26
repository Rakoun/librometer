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
using System.Collections.ObjectModel;

namespace WindowsPhoneApplicationTest.ViewModels {
    public class ViewModel : INotifyPropertyChanged {

        public event PropertyChangedEventHandler PropertyChanged;



        public ObservableCollection<SimpleModel> SimpleModels {
            get;
            private set;
        }

        public bool IsDataLoaded {
            get;
            private set;
        }



        public ViewModel() {
             this.SimpleModels = new ObservableCollection<SimpleModel>();
        }



        public void LoadData() {
            for (int i = 1; i <= 1000; i++) {
                this.SimpleModels.Add(new SimpleModel() { Name = "Runtime Item " + i.ToString(), Description = "This is the Runtime Item " + i.ToString() });
            }
        }



        protected void NotifyPropertyChanged(string thePropertyName) {
            if (this.PropertyChanged != null) {
                this.PropertyChanged(this, new PropertyChangedEventArgs(thePropertyName));
            }
        }
    }
}
