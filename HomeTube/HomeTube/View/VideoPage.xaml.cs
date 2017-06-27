using System;
using System.Diagnostics;
using System.Net.NetworkInformation;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using MyToolkit.Multimedia;
using HomeTube.Model;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace HomeTube.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class VideoPage : Page
    {
        public VideoPage()
        {
            this.InitializeComponent();

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

            App.MainPageViewModel.MediaElement = player;
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
                        var url = await YouTube.GetVideoUriAsync(video.Id, YouTubeQuality.QualityHigh);
                        player.Source = url.Uri;

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
            catch { }

            base.OnNavigatedTo(e);
        }

        public void Pause()
        {
            player.Pause();
        }

        public void Play()
        {
            player.Play();
        }
        public void VolumeUp(int steps)
        {
            player.Volume = player.Volume + steps;
        }
        public void VolumeDown(int steps)
        {
            player.Volume = player.Volume - steps;
        }
        public void SkipUp(int steps)
        {
            player.Position = player.Position + new TimeSpan(0,0,steps);
        }
        public void SkipBack(int steps)
        {
            player.Position = player.Position - new TimeSpan(0, 0, steps);
        }
        //public void JumpTo(string time)
        //{
        //    string[] t = new string[2];
        //    t = time.Split('.');
        //    player.Position = new TimeSpan(0, int.Parse(t[0]), int.Parse(t[1]));
        //}
    }
}
