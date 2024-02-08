using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PF.VM
{
    public class ColorPicked : INotifyPropertyChanged
    {
        private Color c;
        private Brush b;
        public Color color
        {
            get { return c; }
            set
            {
                if (value != c)
                {
                    c = value;
                    colorR = c.R; colorG = c.G; colorB = c.B;
                } else
                {
                    c = value;
                }
                OnPropertyChanged("color");
            }
        }
        public Brush brush
        {
            get { return b; }
            set { b = value; OnPropertyChanged("brush"); }
        }
        public int colorR
        {
            get { return c.R; }
            set { c.R = (byte)value; brush = new SolidColorBrush(c); color = c; OnPropertyChanged("colorR"); }
        }
        public int colorG
        {
            get { return c.G; }
            set { c.G = (byte)value; brush = new SolidColorBrush(c); color = c; OnPropertyChanged("colorG"); }
        }
        public int colorB
        {
            get { return c.B; }
            set { c.B = (byte)value; brush = new SolidColorBrush(c); color = c; OnPropertyChanged("colorB"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ColorPicked()
        {
            c = Color.FromArgb(255, 0, 0, 0);
            b = new SolidColorBrush(c);
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
