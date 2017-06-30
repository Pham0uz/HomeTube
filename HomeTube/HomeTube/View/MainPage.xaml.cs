using System;
using System.Net.NetworkInformation;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
using HomeTube.ViewModel;
using Windows.UI.Xaml.Input;
using HomeTube.Model;
using System.Threading.Tasks;
using Windows.UI.Core;
using System.Text.RegularExpressions;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace HomeTube.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private MainPageViewModel MainVM;

        public MainPage()
        {
            InitializeComponent();
            MainVM = App.MainPageViewModel;
            DataContext = MainVM;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            try
            {
                if (NetworkInterface.GetIsNetworkAvailable())
                {
                    if (!e.NavigationMode.Equals(NavigationMode.Back))
                    {
                        var selectedItem = e.Parameter as YoutubeVideo;

                        if (e.NavigationMode.Equals(NavigationMode.New))
                        {
                            if (selectedItem != null)
                            {
                                SearchProgress.Visibility = Visibility.Collapsed;

                                MainVM.YouTubeItems.Clear();

                                if (selectedItem.Type == "Channel")
                                {
                                    foreach (var v in await MainVM.YouTubeService.ListChannelVideos(selectedItem.Id, 50))
                                    {
                                        MainVM.YouTubeItems.Add(v);
                                    }
                                }
                                else // selectedItem.Type == "Playlist"
                                {
                                    foreach (var v in await MainVM.YouTubeService.ListPlaylistVideos(selectedItem.Id, 50))
                                    {
                                        MainVM.YouTubeItems.Add(v);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageDialog msg = new MessageDialog("You're not connected to Internet!");
                        await msg.ShowAsync();
                    }
                }
            }
            catch (Exception ex)
            {
//#if DEBUG
//                string exMsg = $"MainPage.xaml {ex.Message}";
//                await new MessageDialog(exMsg).ShowAsync();
//#endif
            }
        }

        // Auto-Suggest Box
        private ObservableCollection<String> suggestions;

        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                //suggestions.Clear();

                //suggestions.Add(sender.Text + "1");

                //suggestions.Add(sender.Text + "2");

                //suggestions.Add(sender.Text + "3");

                //suggestions.Add(sender.Text + "4");

                //suggestions.Add(sender.Text + "5");

                //suggestions.Add(sender.Text + "6");

                //suggestions.Add(sender.Text + "7");
                //suggestions.Add(sender.Text + "8");
                //suggestions.Add(sender.Text + "9");
                //sender.ItemsSource = suggestions;
            }
        }

        private async void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion != null)
                txtAutoSuggestBox.Text = args.ChosenSuggestion.ToString();
            else
                txtAutoSuggestBox.Text = sender.Text;

            SearchItems.Visibility = Visibility.Collapsed;
            SearchProgress.Visibility = Visibility.Visible;

            MainVM.YouTubeItems.Clear();

            foreach (var ytItems in await MainVM.YouTubeService.ListItems(MainVM.SearchQuery, MainVM.MaxResults, "video"))
            {
                MainVM.YouTubeItems.Add(ytItems);
            }

            SearchItems.Visibility = Visibility.Visible;
            SearchProgress.Visibility = Visibility.Collapsed;

        }

        private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            txtAutoSuggestBox.Text = "Choosen";
        }

        //After selecting a video, navigate to the VideoPage
        private void Videos_ItemClick(object sender, ItemClickEventArgs e)
        {
            YoutubeVideo video = e.ClickedItem as YoutubeVideo;
            if (video != null)
                Frame.Navigate(typeof(VideoPage), video);
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (success())
            {
                LoginPanel.Visibility = Visibility.Collapsed;
                Info.Visibility = Visibility.Visible;
                ErrorEMail.Visibility = Visibility.Collapsed;
                ErrorPassword.Visibility = Visibility.Collapsed;
                EmailTextBox.Text = "";
                PasswordBox.Password = "";
            }
        }
        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            LoginPanel.Visibility = Visibility.Visible;
            Info.Visibility = Visibility.Collapsed;
        }
        private bool success()
        {
            // check email and pw
            return true;
        }

        private void EmailTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            if (!regex.IsMatch(EmailTextBox.Text))
            {
                ErrorEMail.Visibility = Visibility.Visible;
            }
            else
            {
                ErrorEMail.Visibility = Visibility.Collapsed;
            }
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(PasswordBox.Password))
            {
                ErrorPassword.Visibility = Visibility.Visible;
            }
            else
            {
                ErrorPassword.Visibility = Visibility.Collapsed;
            }
        }
    }
}
