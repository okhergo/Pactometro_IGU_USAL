using PF.M;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PF.VM
{
    public class DataSelected : INotifyPropertyChanged
    {
        private static DataSelected unicaInstancia = null;
        internal static DataSelected Instancia
        {
            get
            {
                if (unicaInstancia == null) unicaInstancia = new DataSelected();
                return unicaInstancia;
            }
        }
        private Collection<Eleccion> elecciones;
        private Collection<Eleccion> eleccionesComparar;
        private Eleccion e;
        private Collection<Brush> colores;
        private Collection<Partido> partidos;
        private Collection<int> votos;
        private Collection<string> nombres;
        private string t;
        public Collection<Eleccion> Elecciones
        {
            get { return elecciones; }
            set { elecciones = value; OnPropertyChanged("Elecciones"); }
        }
        public Eleccion Eleccion
        {
            get { return e; }
            set
            {
                e = value;
                title = value.titulo + " - " + value.fecha;
                partidos = value.partidos; 
                votos.Clear(); colores.Clear(); nombres.Clear(); eleccionesComparar.Clear();
                foreach (Partido p in partidos)
                {
                    votos.Add(p.escaños);
                    colores.Add(p.color);
                    nombres.Add(p.nombre);
                }
                foreach (Eleccion eleccion in elecciones)
                {
                    if (eleccion.titulo == e.titulo)
                        if (eleccion.fecha != e.fecha)
                        {
                            eleccionesComparar.Add(eleccion);
                        }
                }
                OnPropertyChanged("Eleccion");
            }
        }
        public Collection<Partido> Partidos
        {
            get { return partidos; }
            set { partidos = value; OnPropertyChanged("partidos"); }
        }
        public Collection<Eleccion> EleccionesComparar
        {
            get { return eleccionesComparar; }
        }
        public Collection<int> Votos
        {
            get { return votos; }
            set { votos = value; }
        }
        public Collection<string> Nombres
        {
            get { return nombres; }
            set { nombres = value; }
        }
        public Collection<Brush> Colores
        {
            get { return colores; }
            set { colores = value; }
        }
        public string title
        {
            get { return t; }
            set { t = value; OnPropertyChanged("title"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string s)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(s));
            }
        }

        private DataSelected() 
        {
            title = "";
            colores = new Collection<Brush>();
            partidos = new Collection<Partido>();
            votos = new Collection<int>();
            nombres = new Collection<string>();
            eleccionesComparar = new Collection<Eleccion>();
        }
    }
}
