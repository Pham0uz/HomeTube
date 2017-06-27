﻿using System;
using System.Linq;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using HomeTube.ViewModel;
using HomeTube.Services;
using Windows.Storage;
using Windows.Media.SpeechRecognition;
using System.Diagnostics;
using HomeTube.View;

namespace HomeTube
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {

        // could be decoupled via a ViewModelLocator but in this case we have only one ViewModel
        public static MainPageViewModel MainPageViewModel { get; private set; }
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {

#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif
            EnsureInstancedMainVM();

            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    rootFrame.Navigate(typeof(View.MainPage), e.Arguments);
                }
                // Ensure the current window is active
                Window.Current.Activate();
            }

            try
            {
                // Install the main VCD. Since there's no simple way to test that the VCD has been imported, or that it's your most recent
                // version, it's not unreasonable to do this upon app load.
                StorageFile vcdStorageFile = await Package.Current.InstalledLocation.GetFileAsync(@"YouTubeCommands.xml");

                await Windows.ApplicationModel.VoiceCommands.VoiceCommandDefinitionManager.InstallCommandDefinitionsFromStorageFileAsync(vcdStorageFile);

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Installing Voice Commands Failed: " + ex.ToString());
            }
        }

        protected override void OnActivated(IActivatedEventArgs args)
        {
            base.OnActivated(args);

            EnsureInstancedMainVM();

            // If the app was launched via a Voice Command, this corresponds to the "show trip to <location>" command. 
            // Protocol activation occurs when a tile is clicked within Cortana (via the background task)
            if (args.Kind == ActivationKind.VoiceCommand)
            {
                // The arguments can represent many different activation types. Cast it so we can get the
                // parameters we care about out.
                var commandArgs = args as VoiceCommandActivatedEventArgs;

                Windows.Media.SpeechRecognition.SpeechRecognitionResult speechRecognitionResult = commandArgs.Result;

                // Get the name of the voice command and the text spoken. See AdventureWorksCommands.xml for
                // the <Command> tags this can be filled with.
                string voiceCommandName = speechRecognitionResult.RulePath[0];
                string textSpoken = speechRecognitionResult.Text;

                // The commandMode is either "voice" or "text", and it indictes how the voice command
                // was entered by the user.
                // Apps should respect "text" mode by providing feedback in silent form.
                string commandMode = this.SemanticInterpretation("commandMode", speechRecognitionResult);

                switch (voiceCommandName)
                {
                    case "listItems":
                        // Access the value of the {searchQuery} phrase in the voice command
                        var searchQuery = this.SemanticInterpretation("searchQuery", speechRecognitionResult);

                        // set the view model's search string
                        MainPageViewModel.SearchQuery = searchQuery;

                        break;

                    //case "searchMealForeground":
                    //    // Access the value of the {destination} phrase in the voice command
                    //    var meal = this.SemanticInterpretation("meal", speechRecognitionResult);

                    //    // set the view model's search string
                    //    App.MainPageViewModel.DishFilter = meal;

                    //    break;
                    default:
                        // If we can't determine what page to launch, go to the default entry point.
                        Debug.WriteLine("default");

                        break;
                }
            }

            // Re"peat the same basic initialization as OnLaunched() above, taking into account whether
            // or not the app is already active.
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            rootFrame.Navigate(typeof(MainPage));

            // Ensure the current window is active
            Window.Current.Activate();
        }

        private string SemanticInterpretation(string interpretationKey, SpeechRecognitionResult speechRecognitionResult)
        {
            return speechRecognitionResult.SemanticInterpretation.Properties[interpretationKey].FirstOrDefault();
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)

        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }

        private void EnsureInstancedMainVM()
        {
            if (App.MainPageViewModel == null)
            {
                App.MainPageViewModel = new MainPageViewModel(new YouTubeSvc());
            }
        }
    }
}