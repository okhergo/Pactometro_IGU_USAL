using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PF.M
{
    public class SeleccionEventArgs : EventArgs
    {
        private Eleccion e;
        public Eleccion eleccion { get { return e; } set { e = value; } }
        public SeleccionEventArgs(Eleccion eleccion)
        {
            this.eleccion = eleccion;
        }
    }

    public class AddEventArgs : EventArgs
    {
        private ObservableCollection<Eleccion> elecciones;
        public ObservableCollection<Eleccion> Elecciones
        {
            get { return elecciones; }
            set { elecciones = value; }
        }
        public AddEventArgs(ObservableCollection<Eleccion> elecciones)
        {
            this.Elecciones = elecciones;
        }
    }
}
