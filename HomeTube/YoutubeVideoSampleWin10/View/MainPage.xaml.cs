using System;
using System.Net.NetworkInformation;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using HomeTube.Model;
using System.Collections.ObjectModel;
using HomeTube.Services;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace HomeTube.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private IYouTubeSvc m_youtubeService;
        public MainPage()
        {
            this.InitializeComponent();
            m_youtubeService = new YouTubeSvc();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                if (NetworkInterface.GetIsNetworkAvailable())
                {
                    //The number of videos you would like to get in one request(from 1 to 50)
                    int max_results = 50;

                    ////Search Items
                    SearchItems.Visibility = Visibility.Collapsed;
                    SearchProgress.Visibility = Visibility.Collapsed;

                    ////Channel Videos
                    ChannelVideos.Visibility = Visibility.Collapsed;
                    ChannelProgress.Visibility = Visibility.Visible;

                    //Here is the Id of the Channel
                    string YoutubeChannel = "UCFtEEv80fQVKkD4h1PF-Xqw";
                    ////If you can't get the Channel Id, use the UserName to get it via this method
                    ////UserName
                    //string userName = "Microsoft";
                    //string YoutubeChannel = await GetChannelId(userName);

                    var channelVideos = await m_youtubeService.GetChannelVideos(YoutubeChannel, max_results);
                    ChannelVideos.ItemsSource = channelVideos;

                    ChannelVideos.Visibility = Visibility.Visible;
                    ChannelProgress.Visibility = Visibility.Collapsed;
                    /////////

                    ////Playlist Videos
                    PlaylistVideos.Visibility = Visibility.Collapsed;
                    PlaylistProgress.Visibility = Visibility.Visible;

                    //Here is the ID of the Playlist
                    string YoutubePlaylist = "PLFPUGjQjckXH0a_oCO0Cpt91JEVEIRRPn";
                    var playlistVideos = await m_youtubeService.GetPlaylistVideos(YoutubePlaylist, max_results);
                    PlaylistVideos.ItemsSource = playlistVideos;

                    PlaylistVideos.Visibility = Visibility.Visible;
                    PlaylistProgress.Visibility = Visibility.Collapsed;
                    /////////

                }
                else
                {
                    MessageDialog msg = new MessageDialog("You're not connected to Internet!");
                    await msg.ShowAsync();
                }
            }
            catch { }

            base.OnNavigatedTo(e);
        }
         
        // Auto-Suggest Box
        private ObservableCollection<String> suggestions;

        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)

        {

            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)

            {

                //suggestions.Clear();

                //suggestions.Add(sender.Text + "1");

                //suggestions.Add(sender.Text + "2");

                //suggestions.Add(sender.Text + "3");

                //suggestions.Add(sender.Text + "4");

                //suggestions.Add(sender.Text + "5");

                //suggestions.Add(sender.Text + "6");

                //suggestions.Add(sender.Text + "7");

                //suggestions.Add(sender.Text + "8");

                //suggestions.Add(sender.Text + "9");

                //sender.ItemsSource = suggestions;

            }

        }

        private async void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)

        {

            if (args.ChosenSuggestion != null)

                txtAutoSuggestBox.Text = args.ChosenSuggestion.ToString();

            else

                txtAutoSuggestBox.Text = sender.Text;

            SearchItems.Visibility = Visibility.Collapsed;
            SearchProgress.Visibility = Visibility.Visible;

            var searchItems = await m_youtubeService.ListItems(txtAutoSuggestBox.Text, 50);
            SearchItems.ItemsSource = searchItems;

            SearchItems.Visibility = Visibility.Visible;
            SearchProgress.Visibility = Visibility.Collapsed;

        }

        private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)

        {

            txtAutoSuggestBox.Text = "Choosen";

        }

        //After selecting a video, navigate to the VideoPage
        private void Videos_ItemClick(object sender, ItemClickEventArgs e)
        {
            YoutubeVideo video = e.ClickedItem as YoutubeVideo;
            if (video != null)
                Frame.Navigate(typeof(VideoPage), video);
        }

    }
}
