using System;
using System.Windows;
using System.Windows.Controls;
using PF.VM;
using PF.M;
using System.ComponentModel;

namespace PF.V
{
    /// <summary>
    /// Lógica de interacción para WindowListado.xaml
    /// </summary>

    public partial class WindowListado : Window
    {
        ViewModel vm = new ViewModel();
        DataSelected s = DataSelected.Instancia;
        MainWindow mw;
        public WindowListado(MainWindow mw)
        {
            InitializeComponent();
            this.mw = mw;
            mw.Closed += Mw_Closed;
            this.DataContext = s;
            listaElecciones.ItemsSource = s.Elecciones;
            listaElecciones.SelectionChanged += listaElecciones_SelectionChanged;
            s.PropertyChanged += S_PropertyChanged;
        }

        //EVENTOS
        private void S_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Elecciones")
            {
                listaElecciones.ItemsSource = s.Elecciones;
            }
        }
        private void listaElecciones_SelectionChanged(Object sender, SelectionChangedEventArgs e)
        {
            if (listaElecciones.SelectedItem != null)
            {
                vm.ListaEleccionesSelection(listaElecciones.SelectedItem);
                listaResultados.ItemsSource = s.Partidos;
            }
        }
        private void Mw_Closed(object sender, EventArgs e)
        {
            mw = null;
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
        private void MenuAbrirGraficos_Click(object sender, RoutedEventArgs e)
        {
            if (mw == null)
            {
                mw = new MainWindow(this);
                mw.Closed += Mw_Closed;
                mw.Show();
            }
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
        }
        private void MenuIdiomaEN_Click(object sender, RoutedEventArgs e)
        {
            vm.setLanguageDictionary("EN");
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
