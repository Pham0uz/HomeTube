using HomeTube.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeVideoSpace
{
    public interface IYoutubeVideoService
    {

        //Get Channel Videos
        Task<List<YoutubeVideo>> GetChannelVideos(string channelId, int maxResults);

        Task<List<YoutubeVideo>> GetChannelVideosWithPagination(string channelId, int maxResults);

        Task<List<YoutubeVideo>> GetPlaylistVideos(string playlistId, int maxResults);

        Task<string> GetChannelId(string userName);

        Task<List<YoutubeVideo>> ListItems(string searchQuery, int maxResults);



    }
}
