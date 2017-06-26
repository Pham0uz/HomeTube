using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using HomeTube.Model;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace HomeTube.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                if (NetworkInterface.GetIsNetworkAvailable())
                {
                    //The number of videos you would like to get in one request(from 1 to 50)
                    int max_results = 50;


                    
                    ////Channel Videos
                    ChannelVideos.Visibility = Visibility.Collapsed;
                    ChannelProgress.Visibility = Visibility.Visible;

                    //Here is the Id of the Channel
                    string YoutubeChannel = "UCFtEEv80fQVKkD4h1PF-Xqw";
                    ////If you can't get the Channel Id, use the UserName to get it via this method
                    ////UserName
                    //string userName = "Microsoft";
                    //string YoutubeChannel = await GetChannelId(userName);

                    var channelVideos = await GetChannelVideos(YoutubeChannel, max_results);
                    ChannelVideos.ItemsSource = channelVideos;

                    ChannelVideos.Visibility = Visibility.Visible;
                    ChannelProgress.Visibility = Visibility.Collapsed;
                    /////////

                    ////Playlist Videos
                    PlaylistVideos.Visibility = Visibility.Collapsed;
                    PlaylistProgress.Visibility = Visibility.Visible;

                    //Here is the ID of the Playlist
                    string YoutubePlaylist = "PLFPUGjQjckXH0a_oCO0Cpt91JEVEIRRPn";
                    var playlistVideos = await GetPlaylistVideos(YoutubePlaylist, max_results);
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


        //Youtub Data API Credentials
        YouTubeService youtubeService = new YouTubeService(
                new BaseClientService.Initializer
                {
                    ApiKey = "AIzaSyDaeHjO9QK38vHvHHkvPVbrmx0iAi8M0cc",
                    ApplicationName = "HomeTube"
                });

        //Get Channel Videos
        public async Task<List<YoutubeVideo>> GetChannelVideos(string channelId, int maxResults)
        {
            var channelItemsListRequest = youtubeService.Search.List("snippet");
            channelItemsListRequest.ChannelId = channelId;
            channelItemsListRequest.Type = "video";
            channelItemsListRequest.Order = Google.Apis.YouTube.v3.SearchResource.ListRequest.OrderEnum.Date;
            channelItemsListRequest.MaxResults = maxResults;
            channelItemsListRequest.PageToken = "";

            var channelItemsListResponse = await channelItemsListRequest.ExecuteAsync();
            List<YoutubeVideo> channelVideos = new List<YoutubeVideo>();

            foreach (var channelItem in channelItemsListResponse.Items)
            {
                channelVideos.Add(
                    new YoutubeVideo
                    {
                        Id = channelItem.Id.VideoId,
                        Title = channelItem.Snippet.Title,
                        Description = channelItem.Snippet.Description,
                        PubDate = channelItem.Snippet.PublishedAt,
                        Thumbnail = channelItem.Snippet.Thumbnails.Medium.Url,
                        YoutubeLink = "https://www.youtube.com/watch?v=" + channelItem.Id.VideoId
                    });
            }

            return channelVideos;
        }

        //Get Channel Videos With Pagination
        public async Task<List<YoutubeVideo>> GetChannelVideosWithPagination(string channelId, int maxResults)
        {
            var channelItemsListRequest = youtubeService.Search.List("snippet");
            channelItemsListRequest.ChannelId = channelId;
            channelItemsListRequest.Type = "video";
            channelItemsListRequest.Order = Google.Apis.YouTube.v3.SearchResource.ListRequest.OrderEnum.Date;
            channelItemsListRequest.MaxResults = maxResults;
            channelItemsListRequest.PageToken = "";

            var channelItemsListResponse = await channelItemsListRequest.ExecuteAsync();
            List<YoutubeVideo> channelVideos = new List<YoutubeVideo>();
            var nextPageToken = "";
            int i = 0;
            while (i < 3)
            {
                foreach (var channelItem in channelItemsListResponse.Items)
                {
                    channelVideos.Add(
                        new YoutubeVideo
                        {
                            Id = channelItem.Id.VideoId,
                            Title = channelItem.Snippet.Title,
                            Description = channelItem.Snippet.Description,
                            PubDate = channelItem.Snippet.PublishedAt,
                            Thumbnail = channelItem.Snippet.Thumbnails.Medium.Url,
                            YoutubeLink = "https://www.youtube.com/watch?v=" + channelItem.Id.VideoId
                        });
                }

                i++;

                nextPageToken = channelItemsListResponse.NextPageToken;

            }

            return channelVideos;
        }

        //Get Playlist Videos
        public async Task<List<YoutubeVideo>> GetPlaylistVideos(string playlistId, int maxResults)
        {
            var playlistItemsListRequest = youtubeService.PlaylistItems.List("snippet");
            playlistItemsListRequest.PlaylistId = playlistId;
            playlistItemsListRequest.MaxResults = maxResults;
            playlistItemsListRequest.PageToken = "";

            var playlistItemsListResponse = await playlistItemsListRequest.ExecuteAsync();
            List<YoutubeVideo> playlistVideos = new List<YoutubeVideo>();

            foreach (var playlistItem in playlistItemsListResponse.Items)
            {
                playlistVideos.Add(
                    new YoutubeVideo
                    {
                        Id = playlistItem.Snippet.ResourceId.VideoId,
                        Title = playlistItem.Snippet.Title,
                        Description = playlistItem.Snippet.Description,
                        PubDate = playlistItem.Snippet.PublishedAt,
                        Thumbnail = playlistItem.Snippet.Thumbnails.Medium.Url,
                        YoutubeLink = "https://www.youtube.com/watch?v=" + playlistItem.Snippet.ResourceId.VideoId
                    });
            }

            return playlistVideos;
        }

        //Get Cannel Id
        public async Task<string> GetChannelId(string userName)
        {
            var channelIdRequest = youtubeService.Channels.List("id");
            channelIdRequest.ForUsername = userName;

            var channelIdResponse = await channelIdRequest.ExecuteAsync();

            return channelIdResponse.Items.FirstOrDefault().Id;
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
