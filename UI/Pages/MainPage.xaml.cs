namespace UI.Pages {
    public partial class MainPage : Page {
        private void EnableAllTab() {
            UserPanel.IsEnabled = true;
            foreach (Button button in TabsPanel.Children) {
                button.IsEnabled = true;
            }
        }
        private void DisablePressedTab(Button tab) {
            EnableAllTab();
            tab.IsEnabled = false;
        }

        private Frame Frame { get; set; } = null!;
        private MainPageFrame? MainPageFrame { get; set; }
        private UserPage? UserPage { get; set; }
        private ServicesPage? ServicesPage { get; set; }
        private PatientsPage? PatientsPage { get; set; }
        private OrdersPage? OrdersPage { get; set; }
        private InsuranceCompaniesPage? InsuranceCompaniesPage { get; set; }
        private AdministratorPage? AdministratorPage { get; set; }
        private SignInPage? SignInPage { get; set; }
        private void InitializeAdditionalComponent() {
            Frame = (Frame)Application.Current.MainWindow.FindName("Frame");
            MainPageFrame = MainPageFrame.GetInstance();
        }

        public MainPage() {
            InitializeComponent();
            InitializeAdditionalComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e) {
            UserPage = new(UserPanel, TabsPanel);

            switch (User.Group) {
                case Group.Administrator:
                    UserName.Content = ((Administrator)User.Object).UserName;
                    UserImage.Source = ToBitmapImage(Administrator_Icon);
                    break;
                case Group.Accountant:
                    UserName.Content = ((Accountant)User.Object).Name;
                    UserImage.Source = ToBitmapImage(Accountant_Icon);
                    Patients.Visibility = Visibility.Collapsed;
                    Administrator.Visibility = Visibility.Collapsed;
                    break;
                case Group.Technician:
                    UserName.Content = ((Technician)User.Object).Name;
                    UserImage.Source = ToBitmapImage(Technician_Icon);
                    InsuranceCompanies.Visibility = Visibility.Collapsed;
                    Administrator.Visibility = Visibility.Collapsed;
                    break;
            }

            Grid.SetRow(MainPageFrame, 1);
            if (!MainGrid.Children.Contains(MainPageFrame)) {
                MainGrid.Children.Add(MainPageFrame);
            }

            _ = MainPageFrame?.Navigate(UserPage);
        }

        private void Services_Click(object sender, RoutedEventArgs e) {
            ServicesPage = new((Button)sender);
            DisablePressedTab((Button)sender);
            _ = MainPageFrame?.Navigate(ServicesPage);
        }

        private void Patients_Click(object sender, RoutedEventArgs e) {
            PatientsPage = new((Button)sender);
            DisablePressedTab((Button)sender);
            _ = MainPageFrame?.Navigate(PatientsPage);
        }

        private void Orders_Click(object sender, RoutedEventArgs e) {
            OrdersPage = new((Button)sender);
            DisablePressedTab((Button)sender);
            _ = MainPageFrame?.Navigate(OrdersPage);
        }

        private void InsuranceCompanies_Click(object sender, RoutedEventArgs e) {
            InsuranceCompaniesPage = new((Button)sender);
            DisablePressedTab((Button)sender);
            _ = MainPageFrame?.Navigate(InsuranceCompaniesPage);
        }

        private void Administrator_Click(object sender, RoutedEventArgs e) {
            AdministratorPage= new((Button) sender);
            DisablePressedTab((Button)sender);
            _ = MainPageFrame?.Navigate(AdministratorPage);
        }

        private void UserPanel_Click(object sender, RoutedEventArgs e) {
            UserPage = new((Button)sender, TabsPanel);
            DisablePressedTab((Button)sender);
            MainPageFrame?.Navigate(UserPage);
        }

        private void SwitchUser_Click(object sender, RoutedEventArgs e) {
            SignInPage = new();
            _ = Frame.Navigate(SignInPage);
        }
    }
}