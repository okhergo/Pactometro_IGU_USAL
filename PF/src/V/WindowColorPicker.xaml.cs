using PF.VM;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PF.V
{
    /// <summary>
    /// Lógica de interacción para WindowColorPicker.xaml
    /// </summary>

    partial class WindowColorPicker : Window
    {
        ColorPicked colorPicked = new ColorPicked();
        public Color colorSaved { get; set; }
        public Brush brushSaved { get; set; }
        public WindowColorPicker()
        {
            InitializeComponent();
            DataContext = colorPicked;
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            colorSaved = colorPicked.color;
            brushSaved = colorPicked.brush;
            DialogResult = true;
        }
        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
