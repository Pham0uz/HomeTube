using System;

namespace HomeTube.Model
{
    public sealed class YoutubeVideo
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string PubDate { get; set; }
        public string YoutubeLink { get; set; }
        public string PlayerLink { get; set; }
        public string Thumbnail { get; set; }
        public string Description { get; set; }

        public string Type { get; set; }
    }
}

