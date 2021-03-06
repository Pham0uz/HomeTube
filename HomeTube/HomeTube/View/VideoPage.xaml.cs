﻿using System;
using System.Diagnostics;
using System.Net.NetworkInformation;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using MyToolkit.Multimedia;
using HomeTube.Model;
using HomeTube.ViewModel;
using System.Linq;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace HomeTube.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class VideoPage : Page
    {
        MainPageViewModel MainVM;
        public VideoPage()
        {
            this.InitializeComponent();

            MainVM = App.MainPageViewModel;
            MainVM.MediaElement = player;

            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            SystemNavigationManager.GetForCurrentView().BackRequested += (s, a) =>
            {
                Debug.WriteLine("BackRequested");
                if (Frame.CanGoBack)
                {
                    Frame.GoBack();
                    a.Handled = true;
                }
            };
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                if (NetworkInterface.GetIsNetworkAvailable())
                {
                    player.Visibility = Visibility.Collapsed;
                    progress.Visibility = Visibility.Visible;

                    string videoId = String.Empty;
                    YoutubeVideo video = e.Parameter as YoutubeVideo;
                    if (video != null && !video.Id.Equals(String.Empty))
                    {
                        //Get The Video Uri and set it as a player source
                        var url = await YouTube.GetVideoUriAsync(video.Id, YouTubeQuality.Quality2160P);
                        player.Source = url.Uri;
                        Debug.WriteLine($"Debug: {url.Uri}");
                        MainVM.CurrentElementInList = MainVM.YouTubeItems.IndexOf(video);

                    }

                    player.Visibility = Visibility.Visible;
                    progress.Visibility = Visibility.Collapsed;
                }
                else
                {
                    MessageDialog message = new MessageDialog("You're not connected to Internet!");
                    await message.ShowAsync();
                    this.Frame.GoBack();
                }
            }
            catch (YouTubeUriNotFoundException yte)
            {
                await new MessageDialog(yte.Message).ShowAsync();
            }

            base.OnNavigatedTo(e);
        }

        private void Element_MediaEnded(object sender, RoutedEventArgs e)
        {
            int currentIdx = MainVM.CurrentElementInList + 1;
            var video = MainVM.YouTubeItems.ElementAtOrDefault(currentIdx);
            if (video != null)
                Frame.Navigate(typeof(VideoPage), video);
        }

        //public void JumpTo(string time)
        //{
        //    string[] t = new string[2];
        //    t = time.Split('.');
        //    player.Position = new TimeSpan(0, int.Parse(t[0]), int.Parse(t[1]));
        //}
    }
}
