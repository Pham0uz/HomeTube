﻿using System;
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
using System.Diagnostics;

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

        ////Get Channel Videos With Pagination
        //public async Task<List<YoutubeVideo>> GetChannelVideosWithPagination(string channelId, int maxResults)
        //{
        //    var channelItemsListRequest = youtubeService.Search.List("snippet");
        //    channelItemsListRequest.ChannelId = channelId;
        //    channelItemsListRequest.Type = "video";
        //    channelItemsListRequest.Order = Google.Apis.YouTube.v3.SearchResource.ListRequest.OrderEnum.Date;
        //    channelItemsListRequest.MaxResults = maxResults;
        //    channelItemsListRequest.PageToken = "";

        //    var channelItemsListResponse = await channelItemsListRequest.ExecuteAsync();
        //    List<YoutubeVideo> channelVideos = new List<YoutubeVideo>();
        //    var nextPageToken = "";
        //    int i = 0;
        //    while (i < 3)
        //    {
        //        foreach (var channelItem in channelItemsListResponse.Items)
        //        {
        //            channelVideos.Add(
        //                new YoutubeVideo
        //                {
        //                    Id = channelItem.Id.VideoId,
        //                    Title = channelItem.Snippet.Title,
        //                    Description = channelItem.Snippet.Description,
        //                    PubDate = channelItem.Snippet.PublishedAt.Value.Date.ToString("d"),
        //                    Thumbnail = channelItem.Snippet.Thumbnails.Medium.Url,
        //                    YoutubeLink = "https://www.youtube.com/watch?v=" + channelItem.Id.VideoId
        //                });
        //        }

        //        i++;

        //        nextPageToken = channelItemsListResponse.NextPageToken;

        //    }

        //    return channelVideos;
        //}

        //Get Cannel Id
        public async Task<string> GetChannelId(string userName)
        {
            var channelIdRequest = youtubeService.Channels.List("id");
            channelIdRequest.ForUsername = userName;

            var channelIdResponse = await channelIdRequest.ExecuteAsync();

            return channelIdResponse.Items.FirstOrDefault().Id;
        }

        public async Task<List<YoutubeVideo>> ListItems(string searchQuery, int maxResults, string type)
        {
            var searchItemsListRequest = youtubeService.Search.List("snippet");
            searchItemsListRequest.Q = searchQuery;
            searchItemsListRequest.Type = type;
            if (type.Equals("video"))
                searchItemsListRequest.EventType = SearchResource.ListRequest.EventTypeEnum.Completed;
            // default order
            //searchItemsListRequest.Order = Google.Apis.YouTube.v3.SearchResource.ListRequest.OrderEnum.Relevance;
            searchItemsListRequest.MaxResults = maxResults;
            searchItemsListRequest.PageToken = "";

            var searchItemsListResponse = await searchItemsListRequest.ExecuteAsync();
            List<YoutubeVideo> searchItems = new List<YoutubeVideo>();

            //for type
            string searchItemType = "";
            string searchItemId = "";

            foreach (var searchItem in searchItemsListResponse.Items)
            {
                if (searchItem.Id.Kind == "youtube#video")
                {
                    searchItemType = "Video";
                    searchItemId = searchItem.Id.VideoId;
                }
                else if (searchItem.Id.Kind == "youtube#channel")
                {
                    searchItemType = "Channel";
                    searchItemId = searchItem.Id.ChannelId;
                }
                else
                {
                    searchItemType = "Playlist";
                    searchItemId = searchItem.Id.PlaylistId;
                }

                searchItems.Add(
                    new YoutubeVideo
                    {
                        Id = searchItemId,
                        Title = searchItem.Snippet.Title,
                        Description = searchItem.Snippet.Description,
                        PubDate = searchItem.Snippet.PublishedAt.Value.Date.ToString("d"),
                        Thumbnail = searchItem.Snippet.Thumbnails.High.Url,
                        YoutubeLink = "https://www.youtube.com/watch?v=" + searchItem.Id.VideoId,
                        Type = searchItemType
                    });
            }
            return searchItems;
        }

        public async Task<List<YoutubeVideo>> ListChannelVideos(string channelId, int maxResults)
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
                        PubDate = channelItem.Snippet.PublishedAt.Value.Date.ToString("d"),
                        Thumbnail = channelItem.Snippet.Thumbnails.Medium.Url,
                        YoutubeLink = "https://www.youtube.com/watch?v=" + channelItem.Id.VideoId
                    });
            }

            return channelVideos;
        }

        public async Task<List<YoutubeVideo>> ListPlaylistVideos(string playlistId, int maxResults)
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
                        PubDate = playlistItem.Snippet.PublishedAt.Value.Date.ToString("d"),
                        Thumbnail = playlistItem.Snippet.Thumbnails.Medium.Url,
                        YoutubeLink = "https://www.youtube.com/watch?v=" + playlistItem.Snippet.ResourceId.VideoId
                    });
            }

            return playlistVideos;
        }
    }
}
