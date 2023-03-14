namespace UI.Pages {
    public partial class UserPage : Page {
        private Button Sender { get; set; }
        private StackPanel TabPanel { get; set; }

        public UserPage(Button sender, StackPanel tabPanel) {
            Sender = sender;
            TabPanel = tabPanel;
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e) {
            foreach (Button item in TabPanel.Children) {
                item.IsEnabled = true;
            }
            Sender.IsEnabled = false;

            switch (User.Group) {
                case Group.Administrator:
                    Administrator administrator = (Administrator)User.Object;
                    UserImage.Source = ToBitmapImage(Administrator_Icon);
                    UserName.Text = administrator.UserName;
                    Password.Text = administrator.Password;
                    GroupName.Text = "Администратор";
                    FullNameLabel.Visibility = Visibility.Collapsed;
                    FullNameBorder.Visibility = Visibility.Collapsed;
                    LastEnterLabel.Visibility = Visibility.Collapsed;
                    LastEnterBorder.Visibility = Visibility.Collapsed;
                    break;
                case Group.Accountant:
                    Accountant accountant = (Accountant)User.Object;
                    UserImage.Source = ToBitmapImage(Accountant_Icon);
                    UserName.Text = accountant.UserName;
                    Password.Text = accountant.Password;
                    GroupName.Text = "Бухгалтер";
                    FullName.Text = accountant.Name;
                    LastEnter.Text = $"{accountant.LastEnter?.ToLongDateString()} {accountant.LastEnter?.ToLongTimeString()}";
                    break;
                case Group.Technician:
                    Technician technician = (Technician)User.Object;
                    UserImage.Source = ToBitmapImage(Accountant_Icon);
                    UserName.Text = technician.UserName;
                    Password.Text = technician.Password;
                    GroupName.Text = "Лаборант";
                    FullName.Text = technician.Name;
                    LastEnter.Text = $"{technician.LastEnter?.ToLongDateString()} {technician.LastEnter?.ToLongTimeString()}";
                    break;
            }
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e) {
            Sender.IsEnabled = true;
        }
    }
}