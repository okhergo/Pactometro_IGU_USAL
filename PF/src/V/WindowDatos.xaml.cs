using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using PF.VM;

namespace PF.V
{
    /// <summary>
    /// Lógica de interacción para WindowDatos.xaml
    /// </summary>
    public partial class WindowDatos : Window
    {
        Color color = Color.FromArgb(255,0,0,0);
        PartidosAdded pa = new PartidosAdded();
        CheckData cd = new CheckData();
        ViewModel vm;
        public WindowDatos()
        {
            InitializeComponent();
            vm = new ViewModel(pa, cd);
            this.DataContext = pa;
            boxNombre.Focus();
            boxCA.IsEnabled = false;
            saveButton.IsEnabled = false;
            pa.PropertyChanged += Pa_PropertyChanged;
            boxTipo.SelectionChanged += Tipo_SelectionChanged;
            boxCA.SelectionChanged += CA_SelectionChanged;
            pickerFecha.SelectedDateChanged += Fecha_SelectedDateChanged;
            listaPartidosAdded.SelectionChanged += Lista_SelectionChanged;
            cd.PropertyChanged += Cd_PropertyChanged;
        }
        private void Cd_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Checked") saveButton.IsEnabled = cd.Checked;
        }
        private void Pa_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            listaPartidosAdded.ItemsSource = pa.Partidos;
        }
        private void Tipo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int tipo = boxTipo.SelectedIndex;
            if (tipo == 1)
            {
                boxCA.IsEnabled = false;
                cd.Tipo = true;
                boxCA.Text = null;
                cd.Ambito = boxTipo.SelectedItem.ToString();
            }
            else
            {
                boxCA.IsEnabled = true;
                cd.Tipo = false;
            }
        }
        private void CA_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (boxCA.SelectedItem != null)
            {
                cd.CA = true;
                cd.Ambito = boxCA.SelectedItem.ToString();
            }
        }
        private void Fecha_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (pickerFecha.SelectedDate != null) cd.Fecha = true;
        }
        private void Lista_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listaPartidosAdded.SelectedItem != null)
            {
                DialogResult dialogResult = System.Windows.Forms.MessageBox.Show(vm.obtenerNombre("MessageEliminarCuerpo"), vm.obtenerNombre("MessageEliminar"), System.Windows.Forms.MessageBoxButtons.YesNo);
                if (dialogResult == System.Windows.Forms.DialogResult.Yes)
                {
                    vm.deletePartido(listaPartidosAdded.SelectedIndex);
                }
                else
                {
                    listaPartidosAdded.SelectedItem = null;
                }
            }
        }
        private void add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                vm.createPartido(boxNombre.Text, int.Parse(boxEscaños.Text), color);
                boxNombre.Clear();
                boxEscaños.Clear();
                boxNombre.Focus();
            }
            catch
            {
                System.Windows.MessageBox.Show(vm.obtenerNombre("MessageErrorCuerpo"), vm.obtenerNombre("MessageError"), MessageBoxButton.OK, MessageBoxImage.Exclamation);
                boxEscaños.Focus();
            }
        }
        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
        private void save_Click(object sender, RoutedEventArgs e)
        {
            string fecha = pickerFecha.Text.ToString();
            string tipo = boxTipo.Text.ToString();
            if (!cd.Tipo)
            {
                string ca = boxCA.Text.ToString();
                vm.createEleccion(tipo, ca, fecha);
            }
            else
            {
                vm.createEleccion(tipo, null, fecha);
            }
            DialogResult = true;
        }
        private void colorButton_Click(object sender, RoutedEventArgs e)
        {
            WindowColorPicker wcp = new WindowColorPicker();
            wcp.Owner = this;
            wcp.ShowDialog();
            if(wcp.DialogResult == true)
            {
                color = wcp.colorSaved;
                colorPicked.Fill = wcp.brushSaved;
            }
        }
    }
}
