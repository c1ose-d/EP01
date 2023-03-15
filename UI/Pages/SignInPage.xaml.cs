namespace UI.Pages
{
    public partial class SignInPage : Page
    {
        private void RemoveError()
        {
            if (StackPanel.Children.Contains(Error))
            {
                StackPanel.Children.Remove(Error);
            }
        }

        private List<Administrator> Administrator { get; set; } = null!;
        private async Task GetAdministrator()
        {
            using LaboratoryContext laboratory = new();
            Administrator = await laboratory.Administrator.ToListAsync();
        }
        private List<Accountant> Accountant { get; set; } = null!;
        private async Task GetAccountant()
        {
            using LaboratoryContext laboratory = new();
            Accountant = await laboratory.Accountant.ToListAsync();
        }
        private List<Technician> Technicians { get; set; } = null!;
        private async Task GetTechnicians()
        {
            using LaboratoryContext laboratory = new();
            Technicians = await laboratory.Technicians.ToListAsync();
        }

        private DateTime Start { get; set; }

        private Color Red { get; set; }
        private Style? TitleStyle { get; set; }
        private Frame Frame { get; set; } = null!;
        private MainPage? MainPage { get; set; }
        private Label? Error { get; set; }
        private void InitializeAdditionalComponent()
        {
            Red = (Color)Application.Current.FindResource("Red");
            TitleStyle = (Style)Application.Current.FindResource("TitleStyle");
            Frame = (Frame)Application.Current.MainWindow.FindName("Frame");
            MainPage = new();
            Error = new()
            {
                Foreground = new SolidColorBrush(Red),
                HorizontalAlignment = HorizontalAlignment.Center,
                Content = "Введены неверные логин или пароль!",
                Style = TitleStyle
            };
        }

        public SignInPage()
        {
            InitializeComponent();
            InitializeAdditionalComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await GetAdministrator();
            await GetAccountant();
            await GetTechnicians();
            while (Frame != null && Frame.CanGoBack)
            {
                _ = Frame.RemoveBackEntry();
            }
        }

        private void UserName_TextChanged(object sender, TextChangedEventArgs e)
        {
            RemoveError();
        }

        private void Password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            VisiblePassword.Text = Password.Password;
            RemoveError();
        }

        private void ShowPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            switch (Password.Visibility)
            {
                case Visibility.Visible:
                    ShowPasswordButton.Content = "";
                    VisiblePassword.Visibility = Visibility.Visible;
                    Password.Visibility = Visibility.Hidden;
                    break;
                case Visibility.Hidden:
                    ShowPasswordButton.Content = "";
                    VisiblePassword.Visibility = Visibility.Hidden;
                    Password.Visibility = Visibility.Visible;
                    break;
            }
        }

        private void SignIn_Click(object sender, RoutedEventArgs e)
        {
            Start = DateTime.Now;
            foreach (Administrator administrator in Administrator)
            {
                if (administrator.UserName == UserName.Text && administrator.Password == Password.Password)
                {
                    User.Object = administrator;
                    User.Group = Group.Administrator;
                    _ = Frame.Navigate(MainPage);
                    break;
                }
            }
            using LaboratoryContext database = new();
            foreach (Accountant accountant in Accountant)
            {
                if (accountant.UserName == UserName.Text && accountant.Password == Password.Password)
                {
                    User.Object = accountant;
                    User.Group = Group.Accountant;
                    Accountant def = database.Accountant.Where(s => s.Id == accountant.Id).ToList()[0];
                    def.LastEnter = Start;
                    _ = database.Accountant.Update(def);
                    _ = database.SaveChanges();
                    _ = Frame.Navigate(MainPage);
                    break;
                }
            }
            foreach (Technician technician in Technicians)
            {
                if (technician.UserName == UserName.Text && technician.Password == Password.Password)
                {
                    User.Object = technician;
                    User.Group = Group.Technician;
                    Technician def = database.Technicians.Where(s => s.Id == technician.Id).ToList()[0];
                    def.LastEnter = Start;
                    _ = database.Technicians.Update(def);
                    _ = database.SaveChanges();
                    _ = Frame.Navigate(MainPage);
                    break;
                }
            }
            RemoveError();
            _ = StackPanel.Children.Add(Error);
        }

        private void SignUp_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            _ = Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri)
            {
                UseShellExecute = true
            });
            e.Handled = true;
        }
    }
}