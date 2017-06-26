using HomeTube.Model;
using HomeTube.View;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace HomeTube.Model
{
    public class MainPageViewModel : ViewModelBase
    {
        private readonly IYoutubeVideoService m_youtubeSvc;
        private ObservableCollection<YoutubeVideo> m_youtubeVideos;
        private ICommand m_seachCommands;

        public ObservableCollection<YoutubeVideo> YoutubeVideos
        {
            get
            {
                return m_youtubeVideos;
            }
            private set
            {
                m_youtubeVideos = value;
                this.NotifyPropertyChanged(nameof(this.YoutubeVideos));
            }
        }
    }
}
