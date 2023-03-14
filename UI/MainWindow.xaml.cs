namespace UI {
    public partial class MainWindow : Window {
        private readonly struct Strings {
            public static string ChromeMaximize { get; } = "";
            public static string ChromeRestore { get; } = "";
        }

        private void WindowStateChange() {
            switch (WindowState) {
                case WindowState.Normal:
                    WindowState = WindowState.Maximized;
                    break;
                case WindowState.Maximized:
                    WindowState = WindowState.Normal;
                    break;
            }
        }

        private SignInPage? SignInPage { get; set; }
        private MainPageFrame? MainPageFrame { get; set; }
        public void InitializeAdditionalComponent() {
            SignInPage = new();
            MainPageFrame = MainPageFrame.GetInstance();
        }

        public MainWindow() {
            InitializeComponent();
            InitializeAdditionalComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            _ = Frame.Navigate(SignInPage);
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e) {
            switch (WindowState) {
                case WindowState.Normal:
                    MaximizeButton.Content = Strings.ChromeMaximize;
                    break;
                case WindowState.Maximized:
                    MaximizeButton.Content = Strings.ChromeRestore;
                    break;
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e) {
            if (MainPageFrame != null && MainPageFrame.CanGoBack) {
                MainPageFrame.GoBack();
            }
            else {
                Frame.GoBack();
            }
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e) {
            WindowState = WindowState.Minimized;
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e) {
            WindowStateChange();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e) {
            Application.Current.Shutdown();
        }

        private void DockPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            if (e.ClickCount == 2) {
                WindowStateChange();
            }
            else {
                WindowState = WindowState.Normal;
                DragMove();
            }
        }

        private void Frame_Navigated(object sender, NavigationEventArgs e) {
            BackButton.IsEnabled = Frame.CanGoBack;
        }
    }
}