using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Pour en savoir plus sur le modèle d'élément Contrôle utilisateur, consultez la page http://go.microsoft.com/fwlink/?LinkId=234236

namespace RSSreader
{
    public sealed partial class PopUpUserControler : UserControl
    {
        public PopUpUserControler()
        {
            this.InitializeComponent();
        }

        private void tbx_GotFocus(object sender, RoutedEventArgs e)
        {
            tbx.Text = "";
        }

        private void tbxN_GotFocus(object sender, RoutedEventArgs e)
        {
            tbxN.Text = "";
        }
    }
}
