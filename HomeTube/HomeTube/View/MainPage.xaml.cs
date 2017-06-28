﻿using System;
using System.Net.NetworkInformation;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
using HomeTube.ViewModel;
using Windows.UI.Xaml.Input;
using HomeTube.Model;
using System.Threading.Tasks;
using Windows.UI.Core;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace HomeTube.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private MainPageViewModel MainVM;

        public MainPage()
        {
            InitializeComponent();
            MainVM = App.MainPageViewModel;
            DataContext = MainVM;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            try
            {
                if (NetworkInterface.GetIsNetworkAvailable())
                {
                    //The number of videos you would like to get in one request(from 1 to 20) --> added to MainPageVM
                    //int max_results = 20;

                    //SearchItems.Visibility = Visibility.Collapsed;
                    SearchProgress.Visibility = Visibility.Collapsed;

                    ChannelVideos.Visibility = Visibility.Collapsed;
                    ChannelProgress.Visibility = Visibility.Visible;

                    //Here is the ID of the Channel
                    string YouTubeChannel = "UCenPmqht0q6HPENkS49qSzw";
                    ////If you can't get the Channel Id, use the UserName to get it via this method
                    ////UserName
                    //string userName = "David Lengauer";
                    //string YouTubeChannel = await MainVM.YouTubeService.GetChannelId(userName);

                    foreach (var c in await MainVM.YouTubeService.GetChannelVideos(YouTubeChannel, MainVM.MaxResults))
                    {
                        MainVM.ChannelVideos.Add(c);
                    }

                    ChannelVideos.Visibility = Visibility.Visible;
                    ChannelProgress.Visibility = Visibility.Collapsed;

                    ////Playlist Videos
                    PlaylistVideos.Visibility = Visibility.Collapsed;
                    PlaylistProgress.Visibility = Visibility.Visible;

                    //Here is the ID of the Playlist
                    string YouTubePlaylist = "PLiOKy6RSHbR9hxNtun3OzAt4PiY3ziQpJ";

                    foreach (var p in await MainVM.YouTubeService.GetPlaylistVideos(YouTubePlaylist, MainVM.MaxResults))
                    {
                        MainVM.PlaylistVideos.Add(p);
                    }

                    PlaylistVideos.Visibility = Visibility.Visible;
                    PlaylistProgress.Visibility = Visibility.Collapsed;

                }
                else
                {
                    MessageDialog msg = new MessageDialog("You're not connected to Internet!");
                    await msg.ShowAsync();
                }
            }
            catch { }

            // slightly delay setting focus
            await Task.Factory.StartNew(
                () => Dispatcher.RunAsync(CoreDispatcherPriority.Low,
                    () => txtAutoSuggestBox.Focus(FocusState.Programmatic)));
            txtAutoSuggestBox.Focus(FocusState.Programmatic);

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

            MainVM.YouTubeItems.Clear();

            foreach (var ytItems in await MainVM.YouTubeService.ListItems(MainVM.SearchQuery, MainVM.MaxResults))
            {
                MainVM.YouTubeItems.Add(ytItems);
            }

            //SearchItems.ItemsSource = searchItems;

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

        // autosuggestbox workaround
        //private async void txtSearchBox_KeyDown(object sender, KeyRoutedEventArgs e)
        //{
        //    // on pressing "Enter"
        //    if (e.Key == Windows.System.VirtualKey.Enter)
        //    {
        //        SearchItems.Visibility = Visibility.Collapsed;
        //        SearchProgress.Visibility = Visibility.Visible;

        //        foreach (var ytItems in await MainVM.YouTubeService.ListItems(MainVM.SearchQuery, MainVM.MaxResults))
        //        {
        //            MainVM.YouTubeItems.Add(ytItems);
        //        }

        //        SearchItems.Visibility = Visibility.Visible;
        //        SearchProgress.Visibility = Visibility.Collapsed;
        //    }
        //}
    }
}
