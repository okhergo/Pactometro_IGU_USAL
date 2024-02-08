using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PF.M
{
    enum languages
    {
        ES,
        EN
    }
    public class Languages
    {
        public Uri path { set; get; }

        public Languages(){}

        public void setLanguage(String language)
        {
            switch (language)
            {
                case "ES":
                    path = new Uri("/Resources/StringResourcesES.xaml", UriKind.Relative);
                    break;
                case "EN":
                    path = new Uri("/Resources/StringResourcesEN.xaml", UriKind.Relative);
                    break;
                default:
                    path = new Uri("/Resources/StringResourcesES.xaml", UriKind.Relative);
                    break;
            }
        }
    }
}
