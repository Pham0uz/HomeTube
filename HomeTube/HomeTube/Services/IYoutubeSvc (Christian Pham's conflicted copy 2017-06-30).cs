using Google.Apis.YouTube.v3;
using HomeTube.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HomeTube.Services
{
    public interface IYouTubeSvc
    {
        /*
        //YouTube Service Auth -- not compatible with UWP
        YouTubeService Auth();
        */

        //Get Channel Videos With Pagination
        //Task<List<YoutubeVideo>> GetChannelVideosWithPagination(string channelId, int maxResults);

        //Get Cannel Id
        Task<string> GetChannelId(string userName);

        //Get List YouTube Items
        Task<List<YoutubeVideo>> ListItems(string searchQuery, int maxResults, string type);

        //List ChannelVideos of
        Task<List<YoutubeVideo>> ListChannelVideos(string channelId, int maxResults);

        //List PlaylistVideos of
        Task<List<YoutubeVideo>> ListPlaylistVideos(string playlistId, int maxResults);
    }
}
