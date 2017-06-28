using HomeTube.Model;
using HomeTube.Services;
using HomeTube.View;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace HomeTube.ViewModel
{
    public class MainPageViewModel : ViewModelBase
    {
        private IYouTubeSvc m_youtubeSvc;

        private ObservableCollection<YoutubeVideo> m_youtubeItems;
        private ObservableCollection<YoutubeVideo> m_channelVideos;
        private ObservableCollection<YoutubeVideo> m_playlistVideos;
        private int m_maxResults;
        private ICommand m_searchCommands;

        private string m_searchQuery;

        private bool isLoading;

        private MediaElement m_mediaElement;
        private int m_currentElementInList;

        private string m_header;

        public ObservableCollection<YoutubeVideo> YouTubeItems
        {
            get
            {
                return m_youtubeItems;
            }
            set
            {
                m_youtubeItems = value;
                NotifyPropertyChanged(nameof(YouTubeItems));
            }
        }

        public ObservableCollection<YoutubeVideo> ChannelVideos
        {
            get
            {
                return m_channelVideos;
            }
            set
            {
                m_channelVideos = value;
                NotifyPropertyChanged(nameof(ChannelVideos));
            }
        }

        public ObservableCollection<YoutubeVideo> PlaylistVideos
        {
            get
            {
                return m_playlistVideos;
            }
            set
            {
                m_playlistVideos = value;
                NotifyPropertyChanged(nameof(PlaylistVideos));
            }
        }

        public string SearchQuery
        {
            get
            {
                return m_searchQuery;
            }
            set
            {
                m_searchQuery = value;
                NotifyPropertyChanged(nameof(SearchQuery));
            }
        }

        public IYouTubeSvc YouTubeService
        {
            get
            {
                return m_youtubeSvc;
            }
            set
            {
                m_youtubeSvc = value;
                NotifyPropertyChanged(nameof(YouTubeService));
            }
        }

        public MediaElement MediaElement
        {
            get
            {
                return m_mediaElement;
            }
            set
            {
                m_mediaElement= value;
                NotifyPropertyChanged(nameof(MediaElement));
            }
        }

        public int CurrentElementInList
        {
            get
            {
                return m_currentElementInList;
            }
            set
            {
                m_currentElementInList = value;
                NotifyPropertyChanged(nameof(SearchQuery));
            }
        }

        public int MaxResults
        {
            get
            {
                return m_maxResults;
            }
            set
            {
                m_maxResults = value;
                NotifyPropertyChanged(nameof(SearchQuery));
            }
        }

        public string Header
        {
            get
            {
                return m_header;
            }
            set
            {
                m_header = value;
                NotifyPropertyChanged(nameof(SearchQuery));
            }
        }

        public bool IsLoading
        {
            get
            {
                return this.isLoading;
            }
            set
            {
                this.isLoading = value;
                this.NotifyPropertyChanged(nameof(this.IsLoading));
                this.NotifyPropertyChanged(nameof(this.IsNotLoading));
            }
        }

        public bool IsNotLoading
        {
            get
            {
                return !this.IsLoading;
            }
        }

        public ICommand SearchCommand
        {
            get
            {
                return this.m_searchCommands;
            }
        }

        public MainPageViewModel(IYouTubeSvc youtubeSvc)
        {
            if (youtubeSvc == null)
            {
                throw new ArgumentException(nameof(youtubeSvc));
            }

            m_youtubeSvc = youtubeSvc;

            YouTubeItems = new ObservableCollection<YoutubeVideo>();
            ChannelVideos = new ObservableCollection<YoutubeVideo>();
            PlaylistVideos = new ObservableCollection<YoutubeVideo>();

            m_maxResults = 20;

            m_header = "Search";

            //this.m_searchCommands = new RelayCommand(async () => {
            //    await LoadDataAsync();
            //});
        }

        //public async Task LoadDataAsync()
        //{
        //    this.IsLoading = true;

        //    this.YouTubeItems.Clear();
        //    foreach (var c in await this.m_youtubeSvc.GetCanteens(this.NameFilter, this.DishFilter))
        //    {
        //        this.Canteens.Add(c);
        //    }

        //    this.IsLoading = false;
        //}
   }
}
