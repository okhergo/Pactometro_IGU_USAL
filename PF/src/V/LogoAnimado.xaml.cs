using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace PF.V
{
    public partial class LogoAnimado : Window
    {
        DispatcherTimer tmr;
        Storyboard myStoryboard;
        public LogoAnimado()
        {
            InitializeComponent();
            tmr = new DispatcherTimer(); 
            tmr.Tick += OnTick;
            tmr.Interval = new TimeSpan(0, 0, 6);
            tmr.Start();
        }
        private void OnTick(object sender, EventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            tmr.Stop();
            this.Close();
        }
        private void myLabelLoaded(object sender, RoutedEventArgs e)
        {
            myStoryboard.Begin(this);
        }
        private void R4_Completed(object sender, EventArgs e)
        {
            MyRectangle5.Width = 40;
            Label label = new Label();
            label.Foreground = (SolidColorBrush)Application.Current.FindResource("Background");
            label.FontFamily = new FontFamily("Poppins Bold");
            label.FontSize = 50;
            label.Name = "myLabel";
            this.RegisterName(label.Name, label);
            Canvas.SetLeft(label, 90);
            Canvas.SetTop(label, 240);
            label.Content = "EL PACTÓMETRO";

            DoubleAnimation myDoubleAnimation = new DoubleAnimation();
            myDoubleAnimation.From = 0.0;
            myDoubleAnimation.To = 1.0;
            myDoubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));

            myStoryboard = new Storyboard();
            myStoryboard.Children.Add(myDoubleAnimation);
            Storyboard.SetTargetName(myDoubleAnimation, label.Name);
            Storyboard.SetTargetProperty(myDoubleAnimation, new PropertyPath(Control.OpacityProperty));
            label.Loaded += new RoutedEventHandler(myLabelLoaded);
            canvas.Children.Add(label);
            this.Content = canvas;
        }
        private void R3_Completed(object sender, EventArgs e)
        {
            MyRectangle4.Width = 40;
        }
        private void R2_Completed(object sender, EventArgs e)
        {
            MyRectangle3.Width = 40;
        }
        private void R1_Completed(object sender, EventArgs e)
        {
            MyRectangle2.Width = 40;
        }
    }
}
