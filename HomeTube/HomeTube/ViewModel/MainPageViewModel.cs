using HomeTube.Model;
using HomeTube.Services;
using HomeTube.View;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace HomeTube.ViewModel
{
    public class MainPageViewModel : ViewModelBase
    {
        private readonly IYouTubeSvc m_youtubeSvc;
        private ObservableCollection<YoutubeVideo> m_youtubeItems;
        private ICommand m_searchCommands;
        // private string filters??
        private string m_searchQuery;

        private bool isLoading;

        public ObservableCollection<YoutubeVideo> YouTubeItems
        {
            get
            {
                return m_youtubeItems;
            }
            private set
            {
                m_youtubeItems = value;
                NotifyPropertyChanged(nameof(YouTubeItems));
            }
        }

        public string SearchQuery
        {
            get
            {
                return m_searchQuery;
            }
            private set
            {
                m_searchQuery = value;
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
