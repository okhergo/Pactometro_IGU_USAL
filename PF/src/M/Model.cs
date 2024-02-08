using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Threading;
using System.Windows;

namespace PF.M
{
    internal class Model
    {
        private static Model unicaInstancia = null;
        internal static Model Instancia
        {
            get
            {
                if (unicaInstancia == null) unicaInstancia = new Model();
                return unicaInstancia;
            }
        }

        private ObservableCollection<Eleccion> elecciones;
        public ObservableCollection<Eleccion> Elecciones
        {
            get { return elecciones; }
            set { elecciones = value; On_nuevosPartidos(value); }
        }

        private Eleccion eSeleccionada;
        public Eleccion eleccionSeleccionada 
        {
            get { return eSeleccionada; }
            set { eSeleccionada = value; On_nuevaSeleccion(value); }
        }

        public Model() { }

        //EVENT NUEVA SELECCIÓN
        public event EventHandler<SeleccionEventArgs> nuevaSeleccion;
        private void On_nuevaSeleccion(Eleccion eleccion)
        {
            if (nuevaSeleccion != null) nuevaSeleccion(this, new SeleccionEventArgs(eleccion));
        }
        public void ListaEleccionesSelection(Eleccion e)
        {
            eleccionSeleccionada = e;
        }

        public event EventHandler<AddEventArgs> nuevosPartidos;
        private void On_nuevosPartidos(ObservableCollection<Eleccion> elecciones)
        {
            if (nuevosPartidos != null) nuevosPartidos(this, new AddEventArgs(elecciones));
        }

        //MÉTODOS LENGUAJE Y TEMAS
        public void setLanguageDictionary(Languages l)
        {
            var app = (App)Application.Current;
            ResourceDictionary dict = new ResourceDictionary();
            dict.Source = l.path;
            app.ChangeLanguage(dict.Source);
        }
        public void setTheme(Theme t)
        {
            var app = (App)Application.Current;
            ResourceDictionary theme = new ResourceDictionary();
            theme.Source = t.path;
            app.ChangeTheme(theme.Source);
        }

        //MÉTODOS IMPORT DATOS
        public void importDataFromFile(String ruta)
        {
            ObservableCollection<Partido> partidos = new ObservableCollection<Partido>();
            StreamReader fichero = new StreamReader(ruta);

            char separador = ',';
            char separador2 = ' ';
            String linea;
            String[] filaPartido;
            String[] filaTitulo;

            linea = fichero.ReadLine();
            filaTitulo = linea.Split(separador2);
            while ((linea = fichero.ReadLine()) != null)
            {
                if (linea[0] == '\t') {
                    filaPartido = linea.Split(separador);
                    Partido p = new Partido (filaPartido[1], int.Parse(filaPartido[2]), (Color)ColorConverter.ConvertFromString(filaPartido[3]));
                    partidos.Add(p);
                } 
                else
                {
                    crearEleccion(filaTitulo, partidos);
                    filaTitulo = linea.Split(separador2);
                    partidos.Clear();
                }
            }
            crearEleccion(filaTitulo, partidos);
        }
        public void crearEleccion(String[] fila, ObservableCollection<Partido> partidos)
        {
            ObservableCollection<Partido> partidosGuardados = new ObservableCollection<Partido>();
            foreach (Partido partido in partidos)
            {
                partidosGuardados.Add(partido);
            }
            Eleccion e;
            if (fila[1] == "GENERALES") {
                e = new Eleccion(fila[1], fila[2], partidosGuardados);
            }
            else
            {
                e = new Eleccion(fila[1], fila[2], fila[3], partidosGuardados);
            }
            addEleccion(e);
        }
        public void addEleccion(Eleccion e)
        {
            if (elecciones == null) elecciones = new ObservableCollection<Eleccion>();
            else
            {
                foreach (Eleccion eleccion in elecciones)
                {
                    if (eleccion.fecha == e.fecha) return;
                }
            }
                
            ObservableCollection<Eleccion> eleccionesGuardadas = Elecciones;
            eleccionesGuardadas.Add(e);
            Elecciones = eleccionesGuardadas;
        }
        public int getTotal(string s)
        {
            Eleccion e = new Eleccion();
            int total = 0;
            if (s == "GENERALES")
            {
                total = 350;
            } else
            {
                e.setTotales(s);
                total = e.escañosTotales;
            }
            return total;
        }
    }
}
