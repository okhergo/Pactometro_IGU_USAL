using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;
using PF.VM;

namespace PF.V
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ViewModel vm = new ViewModel();
        WindowListado wl;
        DataSelected s = DataSelected.Instancia;

        double altura = 800, anchura = 500, anchoBarra, separador;
        int numPartidos = 0;
        int maxVotos = 0;
        bool flagData = false, flagPactometro = false; int selectedGrafico = 0;
        TranslateTransform tt;
        Line ejex, ejey;
        Brush color;

        //Colecciones de datos para el pactometro
        Collection<double> posicionesPactometro = new Collection<double>();
        Dictionary<double, Columnas> columnas = new Dictionary<double, Columnas>();

        public MainWindow(WindowListado wl)
        {
            InitializeComponent();
            this.wl = wl;
            wl.Closed += Wl_Closed;

            this.DataContext = s;
            s.PropertyChanged += S_PropertyChanged;

            tt = new TranslateTransform();
            tt.Y = 20;
        }
        public MainWindow()
        {
            InitializeComponent();
            vm.setLanguageDictionary(Thread.CurrentThread.CurrentCulture.ToString());
            vm.importInitialData();
            wl = new WindowListado(this);
            wl.Closed += Wl_Closed;
            wl.Show();

            this.DataContext = s;
            s.PropertyChanged += S_PropertyChanged;

            tt = new TranslateTransform();
            tt.Y = 20;
        }

        //EVENTOS
        private void S_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Eleccion")
            {
                flagData = true;
                posicionesPactometro.Clear();
                flagPactometro = false;
                dibuja();
            }
        }
        private void Wl_Closed(object sender, EventArgs e)
        {
            wl = null;
        }
        public void Window_SizeChanged(Object sender, SizeChangedEventArgs e)
        {
            altura = e.NewSize.Height - 190;
            anchura = e.NewSize.Width - 80;
            if (flagData) dibuja();
        }
        private void MenuSalir_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        //DATOS
        private void MenuNuevosDatos_Click(object sender, RoutedEventArgs e)
        {
            WindowDatos wd = new WindowDatos();
            wd.ShowDialog();
        }
        private void MenuImportarDatos_Click(object sender, RoutedEventArgs e)
        {
            vm.importDataFromFile();
        }
        private void MenuAbrirDatos_Click(object sender, RoutedEventArgs e)
        {
            if (wl == null)
            {
                wl = new WindowListado(this);
                wl.Closed += Wl_Closed;
                wl.Show();
            }
        }

        //GRAFICOS
        private void dibuja()
        {
            canvas.Children.Clear();
            canvas.Width = anchura + 45;

            numPartidos = s.Eleccion.partidos.Count;

            if (numPartidos < 2) anchoBarra = (anchura - 50) / 2;
            else anchoBarra = (anchura - 50) / ((numPartidos - 1) * 2);

            separador = anchoBarra * 3 / 5;

            switch (selectedGrafico)
            {
                case 0:
                    graficoEleccion();
                    break;
                case 1:
                    graficoPactometro();
                    break;
                case 2:
                    graficoComparativa();
                    break;
            }
        }
        private void graficoPactometro()
        {
            anchoBarra = (anchura - 50) / 6;
            separador = anchoBarra;
            double posicion = Math.Round((anchura / 2) - anchoBarra / 2); ;
            //Si es la primera vez, inicializa las colecciones
            if (!flagPactometro)
            {
                columnas[posicion] = new Columnas("Abstencion");
                columnas[posicion + Math.Round(separador + anchoBarra)] = new Columnas("AFavor");
                columnas[posicion - Math.Round(separador + anchoBarra)] = new Columnas("EnContra");
                int v = s.Eleccion.escañosTotales;

                foreach (Columnas c in columnas.Values)
                {
                    c.Votos = v;
                    v = 0;
                }
                for (int i = 0; i < numPartidos; i++)
                    posicionesPactometro.Insert(i, posicion);
            }

            //Limpia las alturas guardadas
            foreach (Columnas c in columnas.Values)
                c.Altura = 0;

            //Calcula la columna con mas votos
            maxVotos = 0;
            foreach (Columnas c in columnas.Values)
                if (c.Votos > maxVotos) maxVotos = c.Votos;

            //Pintar linea de mayoria absoluta
            double alturaMayoria = Math.Abs(altura / maxVotos) * s.Eleccion.mayoria;
            if (alturaMayoria > altura)
            {
                maxVotos = s.Eleccion.mayoria;
                alturaMayoria = altura;
            }
            pintarMayoria(alturaMayoria, "" + s.Eleccion.mayoria, "Absoluta", 45);

            //Pintar linea de mayoria simple
            double votosSies = columnas[posicion + Math.Round(separador + anchoBarra)].Votos;
            if (votosSies < s.Eleccion.mayoria && votosSies > 0)
            {
                alturaMayoria = Math.Abs(altura / maxVotos) * votosSies;
                pintarMayoria(alturaMayoria, "" + votosSies, "Simple", 22);
            }


            //Recorre por todos los partidos de la eleccion seleccionada
            for (int i = 0; i < numPartidos; i++)
            { 
                Rectangle r = new Rectangle();
                r.Height = (s.Votos[i] * altura / maxVotos);
                r.Width = anchoBarra;
                color = s.Colores[i];
                r.Fill = color;
                r.Name = "id" + i;
                r.RenderTransform = tt;
                r.ToolTip = s.Nombres[i] + "\n" + s.Votos[i];
                r.MouseLeftButtonDown += OnClickRectanglePactometro;

                //Coloca en la columna correspondiente
                posicion = posicionesPactometro[i];
                Canvas.SetLeft(r, posicion);
                Canvas.SetTop(r, columnas[posicion].Altura);

                canvas.Children.Add(r);

                //Actualiza la altura de la columna
                columnas[posicion].Altura += r.Height;
            }
            pintarEjesPactometro();
            flagPactometro = true;
        }
        private void pintarMayoria(double alturaMayoria, string votos, string nombre, int pos)
        {
            Line l = new Line();
            l.Stroke = Brushes.Gray;
            l.StrokeThickness = 1;
            l.X1 = 20;
            l.X2 = anchura + 25;
            l.Y1 = l.Y2 = alturaMayoria;
            l.RenderTransform = tt;
            canvas.Children.Add(l);

            Label t = new Label();
            t.Content = votos;
            t.Foreground = Brushes.Gray;
            ScaleTransform st = new ScaleTransform();
            st.ScaleY = -1;
            t.RenderTransform = st;
            Canvas.SetTop(t, alturaMayoria + pos);
            Canvas.SetLeft(t, 15);
            canvas.Children.Add(t);

            Label tn = new Label();
            tn.Content = vm.obtenerNombre(nombre);
            tn.Foreground = vm.obtenerColor("Text");
            tn.RenderTransform = st;
            Canvas.SetTop(tn, alturaMayoria + pos);
            Canvas.SetRight(tn, 15);
            canvas.Children.Add(tn);
        }
        private void pintarEjesPactometro()
        {
            Line l = new Line();
            l.Stroke = Brushes.Gray;
            l.StrokeThickness = 1;
            l.X1 = 20;
            l.X2 = anchura + 25;
            l.Y1 = l.Y2 = 0;
            l.RenderTransform = tt;
            canvas.Children.Add(l);

            foreach(double d in columnas.Keys)
            {
                Label t = new Label();
                t.Content = vm.obtenerNombre(columnas[d].Nombre);
                t.Foreground = vm.obtenerColor("Text");
                ScaleTransform st = new ScaleTransform();
                st.ScaleY = -1;
                t.RenderTransform = st;
                Canvas.SetTop(t, 20);
                Canvas.SetLeft(t, d+25);
                canvas.Children.Add(t);

                Label tn = new Label();
                tn.Content = columnas[d].Votos;
                tn.Foreground = vm.obtenerColor("Text");
                tn.RenderTransform = st;
                Canvas.SetTop(tn, 20);
                Canvas.SetLeft(tn, d);
                canvas.Children.Add(tn);
            }
        }

        private void OnClickRectanglePactometro(object sender, MouseButtonEventArgs e)
        {
            //Obtiene la informacion del rectangulo seleccionado
            Rectangle r = (Rectangle) sender;
            double posicion = Canvas.GetLeft(r);
            int indice = int.Parse(Regex.Replace(r.Name, @"^id(.*)", "$1"));

            //Resta los votos eliminados de la columna actual
            columnas[posicion].Votos -= s.Votos[indice];

            //Calcula la nueva columna
            double p = Math.Round((anchura / 2) - anchoBarra / 2);
            if (posicion == p) posicion = p + Math.Round(separador + anchoBarra);
            else if (posicion == p + Math.Round(separador + anchoBarra))
                posicion = p - Math.Round(separador + anchoBarra);
            else posicion = p;

            posicionesPactometro[indice] = posicion;

            //Suma los votos añadidos a la nueva columna
            columnas[posicion].Votos += s.Votos[indice];

            dibuja();
        }
        private void graficoComparativa()
        {
            int votosMax = vm.calcularVotosMax();

            redimensionarCanvas();
            pintarEjes();

            int numEleccionesComparar = s.EleccionesComparar.Count;
            double distancia = 70;
            anchoBarra /= (numEleccionesComparar + 1);
            for (int i = 0; i < numPartidos; i++)
            {
                Rectangle r = new Rectangle();
                r.Height = (s.Votos[i] * altura / votosMax);
                r.Width = anchoBarra;
                color = s.Colores[i];
                r.Fill = color;
                r.RenderTransform = tt;
                r.ToolTip = s.Nombres[i] + "\n" + s.Votos[i];
                canvas.Children.Add(r);
                Canvas.SetLeft(r, distancia);

                for (int j = 0; j < numEleccionesComparar; j++) {
                    int votos = vm.buscaDatosComparar(s.EleccionesComparar[j], s.Eleccion.partidos[i].nombre);
                    Rectangle rect = new Rectangle();
                    rect.Height = (votos * altura / votosMax);
                    rect.Width = anchoBarra;

                    SolidColorBrush b = color as SolidColorBrush;
                    Color c = b.Color;
                    color = new SolidColorBrush(Color.FromArgb((byte)(255 / (j + 2)), c.R, c.G, c.B));

                    rect.Fill = color;
                    rect.RenderTransform = tt;
                    rect.ToolTip = s.Nombres[i] + "\n" + votos;
                    canvas.Children.Add(rect);
                    distancia += anchoBarra;
                    Canvas.SetLeft(rect, distancia);
                }
                distancia += anchoBarra + separador;
            }

            pintarMarcasEjes(votosMax);
            pintarLeyenda();
        }
        private void pintarLeyenda()
        {
            double distancia = 30;
            string fecha = s.Eleccion.fecha;
            for (int j = 0; j <= s.EleccionesComparar.Count; j++)
            {
                Brush color = new SolidColorBrush(Color.FromArgb((byte)(255 / (j + 1)), 0, 0, 0)); 
                Rectangle rect = new Rectangle();
                rect.Height = 10;
                rect.Width = 30; 
                rect.Fill = color;
                Canvas.SetLeft(rect, anchura - 80);
                Canvas.SetBottom(rect, distancia);
                canvas.Children.Add(rect);

                Label t = new Label();
                t.Content = "" + fecha;
                t.Foreground = vm.obtenerColor("Text");
                ScaleTransform st = new ScaleTransform();
                st.ScaleY = -1;
                t.RenderTransform = st;
                Canvas.SetLeft(t, anchura - 40);
                Canvas.SetBottom(t, distancia - 35);
                canvas.Children.Add(t);

                if (j < s.EleccionesComparar.Count) fecha = s.EleccionesComparar[j].fecha;
                distancia += 15;
            }
        }

        private void graficoEleccion()
        {
            redimensionarCanvas();
            pintarEjes();
            pintarMarcasEjes(s.Votos[0]);

            double distancia = 70;
            for (int i = 0; i < numPartidos; i++)
            {
                Rectangle r = new Rectangle(); 
                r.Height = (s.Votos[i] * altura / s.Votos[0]);
                r.Width = anchoBarra;
                color = s.Colores[i];
                r.Fill = color;
                r.RenderTransform = tt;
                r.ToolTip = s.Nombres[i] + "\n" + s.Votos[i];
                canvas.Children.Add(r);
                Canvas.SetLeft(r, distancia);
                distancia += anchoBarra + separador;
            }
        }
        private void pintarMarcasEjes(int votosMax)
        {
            int distanciaMin;
            double alturaPorVoto = Math.Abs(altura / votosMax);
            if (alturaPorVoto < 5) distanciaMin = 10;
            else distanciaMin = 5;
            alturaPorVoto *= distanciaMin;

            double alt = alturaPorVoto;
            int cont = 1;
            while (alt < altura)
            {
                Line l = new Line();
                l.Stroke = Brushes.Gray;
                l.StrokeThickness = 1;
                l.X1 = 35;
                l.X2 = 45;
                l.Y1 = l.Y2 = alt;
                l.RenderTransform = tt;
                canvas.Children.Add(l);

                Label t = new Label();
                t.Content = ""+cont*distanciaMin;
                t.Foreground = vm.obtenerColor("Text");
                ScaleTransform st = new ScaleTransform();
                st.ScaleY = -1;
                t.RenderTransform = st;
                Canvas.SetTop(t, alt+35);
                Canvas.SetLeft(t, 5);
                canvas.Children.Add(t);

                alt += alturaPorVoto;
                cont++;
            }
        }
        private void redimensionarCanvas()
        {
            int minBarra = 40;
            if (anchoBarra < minBarra)
            {
                anchoBarra = minBarra;
                separador = minBarra * 3 / 5;
                double newSize = 61 + (anchoBarra + separador) * numPartidos;
                if (canvas.Width < newSize) canvas.Width = newSize;
            }
        }
        private void pintarEjes()
        {
            ejex = new Line();
            ejey = new Line();

            ejex.Stroke = Brushes.Gray;
            ejex.StrokeThickness = 1;
            canvas.Children.Add(ejex);

            ejey.Stroke = Brushes.Gray;
            ejey.StrokeThickness = 1;
            canvas.Children.Add(ejey);

            ejex.X1 = 40;
            ejex.X2 = canvas.Width - 10;
            ejex.Y1 = ejex.Y2 = 20;

            ejey.X1 = ejey.X2 = 40;
            ejey.Y1 = altura + 20;
            ejey.Y2 = 20;
        }
        private void MenuComparativa_Click(object sender, RoutedEventArgs e)
        {
            selectedGrafico = 0;
            if (flagData) dibuja();
        }
        private void MenuPactometro_Click(object sender, RoutedEventArgs e)
        {
            selectedGrafico = 1;
            if (flagData) dibuja();
        }
        private void MenuEvolucion_Click(object sender, RoutedEventArgs e)
        {
            selectedGrafico = 2;
            if (flagData) dibuja();
        }

        //MODOS DE VISUALIZACION
        private void MenuTemaClaro_Click(object sender, RoutedEventArgs e)
        {
            vm.setTheme("Claro");
        }
        private void MenuTemaOscuro_Click(object sender, RoutedEventArgs e)
        {
            vm.setTheme("Oscuro");
        }
        private void MenuIdiomaES_Click(object sender, RoutedEventArgs e)
        {
            vm.setLanguageDictionary("ES");
            dibuja();
        }
        private void MenuIdiomaEN_Click(object sender, RoutedEventArgs e)
        {
            vm.setLanguageDictionary("EN");
            dibuja();
        }

        //AYUDA
        private void MenuAyuda_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(string.Format(vm.obtenerNombre("AyudaCuerpo"), Environment.NewLine), 
                string.Format(vm.obtenerNombre("AyudaTitulo"), Environment.NewLine), MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void MenuSobre_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(string.Format(vm.obtenerNombre("SobreCuerpo"), Environment.NewLine), 
                string.Format(vm.obtenerNombre("SobreTitulo"), Environment.NewLine), MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
