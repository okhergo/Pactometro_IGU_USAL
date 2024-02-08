using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using PF.M;

namespace PF.VM
{
    public class PartidosAdded : INotifyPropertyChanged
    {
        private ObservableCollection<Partido> p;
        public ObservableCollection<Partido> Partidos
        {
            get { return p; }
            set { p = value; OnPropertyChanged("Partidos"); }
        }
        public Partido NuevoPartido
        {
            set { p.Add(value); OnPropertyChanged("Partidos"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public PartidosAdded()
        {
            p = new ObservableCollection<Partido>();
        }

        protected void OnPropertyChanged(string s)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(s));
            }
        }
    }
}
