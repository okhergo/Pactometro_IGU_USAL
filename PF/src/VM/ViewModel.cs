using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using PF.M;

namespace PF.VM
{
    internal class ViewModel
    {
        Model m = Model.Instancia;
        DataSelected s = DataSelected.Instancia;
        CheckData cd;
        PartidosAdded pa;

        public ViewModel()
        {
            m.nuevaSeleccion += Wl_nuevaSeleccion;
            m.nuevosPartidos += Wd_nuevosPartidos;
        }

        public ViewModel(PartidosAdded pa, CheckData cd)
        {
            this.pa = pa;
            this.cd = cd;
            m.nuevosPartidos += Wd_nuevosPartidos;
            cd.PropertyChanged += Cd_PropertyChanged;
        }

        //EVENTOS
        private void Cd_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Tipo") checkPartidos();
            if ((cd.Tipo == true || cd.CA == true) && cd.Fecha == true && cd.Partidos == true)
            {
                if (cd.Checked != true) cd.Checked = true;
            }
            else
            {
                if (cd.Checked == true) cd.Checked = false;
            }
        }
        private void Wd_nuevosPartidos(object sender, AddEventArgs e)
        {
            s.Elecciones = e.Elecciones;
        }
        private void Wl_nuevaSeleccion(object sender, SeleccionEventArgs e)
        {
            s.Eleccion = e.eleccion;
        }
        public void ListaEleccionesSelection(object o)
        {
            m.ListaEleccionesSelection((Eleccion)o);
        }

        //IMPORT DATA
        public void setLanguageDictionary(String language)
        {
            Languages l = new Languages();
            l.setLanguage(language);
            m.setLanguageDictionary(l);
        }
        public void setTheme(String tema)
        {
            Theme t = new Theme();
            t.setTheme(tema);
            m.setTheme(t);
        }
        public void importDataFromFile()
        {
            String ruta;
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.FileName = "data";
            ofd.DefaultExt = "Data";
            ofd.Filter = "Text documents (.txt)|*.txt";

            var dialogResult = ofd.ShowDialog();
            if (dialogResult == true)
            {
                ruta = ofd.FileName;
                m.importDataFromFile(ruta);
            }
        }
        public void importInitialData()
        {
            String ruta = "data.txt";
            m.importDataFromFile(ruta);
        }

        //GETTERS
        public SolidColorBrush obtenerColor(string s)
        {
            return (SolidColorBrush)Application.Current.FindResource(s);
        }
        public string obtenerNombre(string s)
        {
            return (string)Application.Current.FindResource(s);
        }
        public int buscaDatosComparar(Eleccion e, string nombre)
        {
            foreach(Partido p in e.partidos)
            {
                if (p.nombre == nombre)
                {
                    return p.escaños;
                }
            }
            return 0;
        }
        public int calcularVotosMax()
        {
            int max = s.Votos[0];
            for (int j = 0; j < s.EleccionesComparar.Count; j++)
            {
                foreach (Partido p in s.EleccionesComparar[j].partidos)
                {
                    if (p.escaños > max) max = p.escaños;
                }
            }
            return max;
        }
        
        //CREATE AND DELETE
        public void createPartido(string nombre, int escaños, Color color)
        {
            Partido p = new Partido(nombre, escaños, color);
            pa.NuevoPartido = p;
            checkPartidos();
        }
        public void deletePartido(int index)
        {
            ObservableCollection<Partido> partidos = pa.Partidos;
            partidos.Remove(partidos[index]);
            pa.Partidos = partidos;
            checkPartidos();
        }
        public void createEleccion(string tipo, string CA, string fecha)
        {
            Eleccion e;
            if (CA != null)
            {
                e = new Eleccion(tipo, CA, fecha, pa.Partidos);
            } 
            else
            {
                e = new Eleccion(tipo, fecha, pa.Partidos);
            }
            m.addEleccion(e);
        }
        public void checkPartidos()
        {
            int suma = 0, total;
            foreach(Partido p in pa.Partidos)
            {
                suma += p.escaños;
            }
            total = m.getTotal(cd.Ambito);
            if (suma == total && suma != 0) cd.Partidos = true;
            else cd.Partidos = false;
        }
    }
}
