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

// include the rssItems class to your work
using RSSreader.Data;
using Windows.Web.Http;
using System.Xml.Linq;
using System.Globalization;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace RSSreader
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {


        //The Windows.Web.Http.HttpClient class provides the main class for 
        // sending HTTP requests and receiving HTTP responses from a resource identified by a URI.
        private HttpClient httpClient;
        private HttpResponseMessage response;

        // This is the feed address that will be parsed and displayed
        private String feedAddress = "http://www.cnet.com/rss/news/";


        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            httpClient = new HttpClient();

            // Add a user-agent header
            var headers = httpClient.DefaultRequestHeaders;

            // HttpProductInfoHeaderValueCollection is a collection of 
            // HttpProductInfoHeaderValue items used for the user-agent header

            headers.UserAgent.ParseAdd("ie");
            headers.UserAgent.ParseAdd("Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)");

        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            if (!string.IsNullOrEmpty(e.Parameter as string))
            {
                feedAddress = e.Parameter as string;
            }
            AppBarButton_Click(this, null);
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }

        public async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            response = new HttpResponseMessage();

            // if 'feedAddress' value changed the new URI must be tested --------------------------------
            // if the new 'feedAddress' doesn't work, 'feedStatus' informs the user about the incorrect input.

            //  feedStatus.Text = "Test if URI is valid";

            Uri resourceUri;
            if (!Uri.TryCreate(feedAddress.Trim(), UriKind.Absolute, out resourceUri))
            {
                //  feedStatus.Text = "Invalid URI, please re-enter a valid URI";
                return;
            }
            if (resourceUri.Scheme != "http" && resourceUri.Scheme != "https")
            {
                //  feedStatus.Text = "Only 'http' and 'https' schemes supported. Please re-enter URI";
                return;
            }
            // ---------- end of test---------------------------------------------------------------------

            string responseText;
            //feedStatus.Text = "Waiting for response ...";

            try
            {
                response = await httpClient.GetAsync(resourceUri);

                response.EnsureSuccessStatusCode();

                responseText = await response.Content.ReadAsStringAsync();
                //   statusPanel.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

            }
            catch (Exception ex)
            {
                //// Need to convert int HResult to hex string
                //feedStatus.Text = "Error = " + ex.HResult.ToString("X") +
                //    "  Message: " + ex.Message;
                responseText = "";
            }
            //feedStatus.Text = response.StatusCode + " " + response.ReasonPhrase;

            // now 'responseText' contains the feed as a verified text.
            // next 'responseText' is converted as the rssItems class model definition to be displayed as a list

            List<rssItems> lstData = new List<rssItems>();
            XElement _xml = XElement.Parse(responseText);
            XNamespace media = XNamespace.Get("http://search.yahoo.com/mrss/");
            XNamespace dc = "http://purl.org/dc/elements/1.1/";
            CultureInfo dateformat = new CultureInfo("fr-FR");
            channelTitle.Text = _xml.Elements("channel").Elements("title").First().Value;


            foreach (XElement value in _xml.Elements("channel").Elements("item"))
            {
                rssItems _item = new rssItems();

                _item.Title = ShortenTitle(value.Element("title").Value);

                _item.Thumbnail = value.Element(media + "thumbnail") != null ? value.Element(media + "thumbnail").Attribute("url").Value : "/Assets/Square71x71Logo.scale-240.png";


                _item.Link = value.Element("link").Value;

                _item.Desc = value.Element("description").Value;

                _item.Date = value.Element("pubDate").Value.Split(' ')[4];
                _item.Date = _item.Date.Remove(_item.Date.Length - 3);



                _item.Author = ShortenCreator(value.Element(dc + "creator").Value);

                lstData.Add(_item);


            }

            // lstRSS is bound to the lstData: the final result of the RSS parsing
            lstRSS.DataContext = lstData;

        }

        private string ShortenTitle(string title)
        {
            if (title.Length > 55)
                return title.Substring(0, 52) + "...";
            return title;
        }

        private string ShortenCreator(string creator)
        {
            if (creator.Length > 20)
                return creator.Substring(0, 17) + "...";
            return creator;
        }

        private async void lstRSS_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // if item clicked navigate to the webpage

            var selected = lstRSS.SelectedItem as rssItems;

            if (selected != null)
            {
                WebView webBrowserTask = new WebView();
                Uri targetUri = new Uri(selected.Link);

                //webbrowser task launcher for Windows 8.1
                //http://msdn.microsoft.com/en-us/library/windows/apps/xaml/hh701480.aspx
                var success = await Windows.System.Launcher.LaunchUriAsync(targetUri);

            }

        }

        private void AppBarButton_Click_1(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(FeedPicker));
        }
    }
}
