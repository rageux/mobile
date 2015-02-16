using RSSreader.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Pour en savoir plus sur le modèle d’élément Page vierge, consultez la page http://go.microsoft.com/fwlink/?LinkID=390556

namespace RSSreader
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class FeedPicker : Page
    {
        private static ObservableCollection<feedItems> FeedList;
        private string SelectedFeed = null;
        private GridViewItem selectedGVI = null;
        //public List<String> LName = new List<String>();
        //public List<String> LUrl = new List<String>();

        public FeedPicker()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoqué lorsque cette page est sur le point d'être affichée dans un frame.
        /// </summary>
        /// <param name="e">Données d'événement décrivant la manière dont l'utilisateur a accédé à cette page.
        /// Ce paramètre est généralement utilisé pour configurer la page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            InitSavedFeed();
        }

        private void InitSavedFeed()
        {
            FeedList = new ObservableCollection<feedItems>();

            var LUrl = Helpers.StorageHelper.GetStoredURL();
            var LName = Helpers.StorageHelper.GetStoredName();

            if (LUrl != null)
            {
                for (int i = 0; i < LUrl.Count; i++)
                {
                    feedItems tmp = new feedItems();
                    tmp.name = LName[i];
                    tmp.url = LUrl[i];

                    FeedList.Add(tmp);
                }
            }

            lstFeed.DataContext = FeedList;
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void AppBarButton_Click_1(object sender, RoutedEventArgs e)
        {

            var selected = lstFeed.SelectedItem as feedItems;
            if (selected != null)
                Frame.Navigate(typeof(MainPage), selected.url);
        }


        private void AppBarButton_Click_2(object sender, RoutedEventArgs e)
        {
            Popup userEntry = new Popup();
            userEntry.Height = 100;
            userEntry.Width = 400;
            userEntry.HorizontalOffset = 50;
            userEntry.VerticalOffset = 350;
            PopUpUserControler control = new PopUpUserControler();
            userEntry.Child = control;
            userEntry.IsOpen = true;



            control.buttonOK.Click += (s, args) =>
            {
                userEntry.IsOpen = false;
                var urlflux = control.tbx.Text;
                var nameflux = control.tbxN.Text;

                feedItems n = new feedItems();

                n.name = nameflux;
                n.url = urlflux;

                FeedList.Add(n);
                saveUserData();

                lstFeed.DataContext = FeedList;
            };

            control.buttonCANCEL.Click += (s, args) =>
            {
                userEntry.IsOpen = false;
            };

        }

        private void AppBarButton_Click_3(object sender, RoutedEventArgs e)
        {
            var selected = lstFeed.SelectedItem as feedItems;

            if (selected != null)
                FeedList.RemoveAt(FeedList.IndexOf(selected));
            saveUserData();
        }

        private void lstFeed_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            var item = lstFeed.ContainerFromItem(lstFeed.SelectedItem as feedItems) as GridViewItem;
            if (selectedGVI != null)
            {
                selectedGVI.Background = null;
                if ((selectedGVI.Equals(item)))
                    return;
            }

            if (item != null)
            {
                item.Background = new SolidColorBrush(Colors.Orange);
                item.Background.Opacity = 0.40;
            }
            selectedGVI = item;
        }

        private void saveUserData()
        {

            var LName = new List<string>();
            var LUrl = new List<string>();

            foreach (var e in FeedList)
            {
                LName.Add(e.name);
                LUrl.Add(e.url);
            }
            Helpers.StorageHelper.StoreName(LName);
            Helpers.StorageHelper.StoreURL(LUrl);

        }
    }
}
