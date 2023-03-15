namespace UI
{
    public partial class MainWindow : Window
    {
        private readonly struct Strings
        {
            public static string ChromeMaximize { get; } = "";
            public static string ChromeRestore { get; } = "";
        }

        private void WindowStateChange()
        {
            switch (WindowState)
            {
                case WindowState.Normal:
                    WindowState = WindowState.Maximized;
                    break;
                case WindowState.Maximized:
                    WindowState = WindowState.Normal;
                    break;
            }
        }

        DateTime Start { get; set; }
        private async Task TimerAsync()
        {
            try
            {
                TitleBarToolTip.StaysOpen = false;
            }
            catch { }
            if (DateTime.Now >= Start.AddMinutes(5))
            {
                TitleBarToolTip.StaysOpen = true;
                Message.Content = "Приложение закроется через 3 минуты!";
            }
            if (DateTime.Now >= Start.AddMinutes(10))
            {
                Application.Current.Shutdown();
            }
            await Task.Delay(10000);
        }

        private SignInPage? SignInPage { get; set; }
        private MainPageFrame? MainPageFrame { get; set; }
        public void InitializeAdditionalComponent()
        {
            SignInPage = new();
            MainPageFrame = MainPageFrame.GetInstance();
        }

        public MainWindow()
        {
            InitializeComponent();
            InitializeAdditionalComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Start = DateTime.Now;
            _ = Frame.Navigate(SignInPage);
            while (true)
            {
                await TimerAsync();
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            switch (WindowState)
            {
                case WindowState.Normal:
                    MaximizeButton.Content = Strings.ChromeMaximize;
                    break;
                case WindowState.Maximized:
                    MaximizeButton.Content = Strings.ChromeRestore;
                    break;
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainPageFrame != null && MainPageFrame.CanGoBack)
            {
                MainPageFrame.GoBack();
            }
            else
            {
                Frame.GoBack();
            }
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowStateChange();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void DockPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                WindowStateChange();
            }
            else
            {
                WindowState = WindowState.Normal;
                DragMove();
            }
        }

        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {
            BackButton.IsEnabled = Frame.CanGoBack;
        }
    }
}