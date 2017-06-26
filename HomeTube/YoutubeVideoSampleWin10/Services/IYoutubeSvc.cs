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

        //Get Channel Videos
        Task<List<YoutubeVideo>> GetChannelVideos(string channelId, int maxResults);

        //Get Channel Videos With Pagination
        Task<List<YoutubeVideo>> GetChannelVideosWithPagination(string channelId, int maxResults);

        //Get Playlist Videos
        Task<List<YoutubeVideo>> GetPlaylistVideos(string playlistId, int maxResults);

        //Get Cannel Id
        Task<string> GetChannelId(string userName);

        // Search Run
        Task SearchFor(string searchQuery);

        //Get Channel Videos
        Task<List<YoutubeVideo>> ListItems(string searchQuery, int maxResults);
    }
}
