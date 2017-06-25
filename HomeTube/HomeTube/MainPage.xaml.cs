using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;

/// <summary>
/// YouTube Data API v3 sample: search by keyword.
/// Relies on the Google APIs Client Library for .NET, v1.7.0 or higher.
/// See https://developers.google.com/api-client-library/dotnet/get_started
///
/// Set ApiKey to the API key value from the APIs & auth > Registered apps tab of
///   https://cloud.google.com/console
/// Please ensure that you have enabled the YouTube Data API for your project.
/// </summary>

namespace HomeTube
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }
    }
}
