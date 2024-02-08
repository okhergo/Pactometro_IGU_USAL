using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PF.M
{
    enum themes
    {
        Claro,
        Oscuro
    }
    public class Theme
    {
        public Uri path { set; get; }

        public Theme() { }

        public void setTheme(String tema)
        {
            switch (tema)
            {
                case "Claro":
                    path = new Uri("/Resources/WhiteTheme.xaml", UriKind.Relative);
                    break;
                case "Oscuro":
                    path = new Uri("/Resources/DarkTheme.xaml", UriKind.Relative);
                    break;
                default:
                    path = new Uri("/Resources/WhiteTheme.xaml", UriKind.Relative);
                    break;
            }
        }
    }
}
