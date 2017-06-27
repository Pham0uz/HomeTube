using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeTube.Model;
using Google.Apis.YouTube.v3;
using Google.Apis.Services;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Util.Store;
using System.IO;
using System.Threading;

namespace HomeTube.Services
{
    public class YouTubeSvc : IYouTubeSvc
    {
        private YouTubeService youtubeService;

        public YouTubeSvc()
        {
            //Youtub Data API Credentials -- workaround because method above doesn't work yet
            youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = "AIzaSyDaeHjO9QK38vHvHHkvPVbrmx0iAi8M0cc",
                ApplicationName = "HomeTube"
            });
        }

        /*
        // YouTubeService Auth -- Google.Apis. currently not supporting UWP
        public YouTubeService Auth()
        {
            UserCredential creds;
            using (var stream = new FileStream("youtube_client_secret.json", FileMode.Open, FileAccess.Read))
            {
                creds = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    new[] { YouTubeService.Scope.YoutubeReadonly },
                    "user",
                    CancellationToken.None,
                    new Google.Apis.Util.Store.FileDataStore("HomeTube")
                    ).Result;
            }

            var service = new YouTubeService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = creds,
                ApplicationName = "HomeTube"
            });

            return service;
        }
        */

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

        //Get Channel Videos
        public async Task<List<YoutubeVideo>> ListItems(string searchQuery, int maxResults)
        {
            var searchItemsListRequest = youtubeService.Search.List("snippet");
            searchItemsListRequest.Q = searchQuery;
            searchItemsListRequest.Type = "video";
            searchItemsListRequest.Order = Google.Apis.YouTube.v3.SearchResource.ListRequest.OrderEnum.ViewCount;
            searchItemsListRequest.MaxResults = maxResults;
            searchItemsListRequest.PageToken = "";

            var searchItemsListResponse = await searchItemsListRequest.ExecuteAsync();
            List<YoutubeVideo> searchItems = new List<YoutubeVideo>();

            foreach (var searchItem in searchItemsListResponse.Items)
            {
                searchItems.Add(
                    new YoutubeVideo
                    {
                        Id = searchItem.Id.VideoId,
                        Title = searchItem.Snippet.Title,
                        Description = searchItem.Snippet.Description,
                        PubDate = searchItem.Snippet.PublishedAt,
                        Thumbnail = searchItem.Snippet.Thumbnails.Medium.Url,
                        YoutubeLink = "https://www.youtube.com/watch?v=" + searchItem.Id.VideoId
                    });
            }
            return searchItems;
        }
    }
}
