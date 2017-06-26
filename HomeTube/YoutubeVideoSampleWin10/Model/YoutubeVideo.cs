using System;

namespace YoutubeVideoSampleWin10.Model
{
    public class YoutubeVideo
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public DateTime? PubDate { get; set; }
        public string YoutubeLink { get; set; }
        public string PlayerLink { get; set; }
        public string Thumbnail { get; set; }
        public string Description { get; set; }
    }
}

