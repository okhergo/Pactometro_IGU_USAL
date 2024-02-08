using System;
using System.Windows;

namespace PF
{
    /// <summary>
    /// Lógica de interacción para App.xaml
    /// </summary>
    public partial class App : Application
    {
        public ResourceDictionary Language
        {
            get { return Resources.MergedDictionaries[0]; }
        }
        public ResourceDictionary Theme
        {
            get { return Resources.MergedDictionaries[1]; }
        }

        public void ChangeLanguage(Uri uri)
        {
            Language.MergedDictionaries.Clear();
            Language.MergedDictionaries.Add(new ResourceDictionary() { Source = uri });
        }

        public void ChangeTheme(Uri uri)
        {
            Theme.MergedDictionaries.Clear();
            Theme.MergedDictionaries.Add(new ResourceDictionary() { Source = uri });
        }
    }
}
