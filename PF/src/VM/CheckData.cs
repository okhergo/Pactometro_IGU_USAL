using PF.M;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PF.VM
{
    internal class CheckData : INotifyPropertyChanged
    {
        private bool _checked;
        private string ambito;
        private bool p;
        private bool ca;
        private bool t;
        private bool f;
        public bool Checked
        {
            get { return _checked; }
            set { _checked = value; OnPropertyChanged("Checked"); }
        }
        public bool Partidos
        {
            get { return p; }
            set { p = value; OnPropertyChanged("Partidos"); }
        }
        public bool CA
        {
            get { return ca; }
            set { ca = value; OnPropertyChanged("Tipo"); }
        }
        public bool Tipo
        {
            get { return t; }
            set { t = value; OnPropertyChanged("Tipo"); }
        }
        public bool Fecha
        {
            get { return f; }
            set { f = value; OnPropertyChanged("Fecha"); }
        }
        public string Ambito
        {
            get { return ambito; }
            set { ambito = value; OnPropertyChanged("Tipo"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public CheckData()
        {
            p = false;
            ca = false;
            t = false; 
            f = false;
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
