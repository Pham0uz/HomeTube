using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using MyToolkit.Multimedia;
using YoutubeVideoSampleWin10.Model;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace YoutubeVideoSampleWin10.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class VideoPage : Page
    {
        public VideoPage()
        {
            this.InitializeComponent();

            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested += (s, a) =>
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
                        var url = await YouTube.GetVideoUriAsync(video.Id, YouTubeQuality.Quality480P);
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
    }
}
