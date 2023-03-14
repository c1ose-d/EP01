using System.Windows.Automation.Peers;

namespace UI.Pages {
    public partial class AdministratorPage : Page {
        private class AccountantListItem : ListViewItem {
            public Accountant Accountant { get; set; }
            private Grid Grid { get; set; } = null!;
            private StackPanel Head { get; set; } = null!;
            private StackPanel Body { get; set; } = null!;
            private Label AccountantName { get; set; } = null!;
            private Border Accent { get; set; } = null!;
            private Label Status { get; set; } = null!;
            private Label UserName { get; set; } = null!;
            private Border Spliter { get; set; } = null!;
            private Label LastEnter { get; set; } = null!;
            private void CreateVisual() {
                Grid = new();
                Head = new() { Orientation = Orientation.Horizontal };
                Body = new() { Orientation = Orientation.Horizontal };
                AccountantName = new() { Style = HeadStyle };
                Accent = new() { Style = AccentStyle };
                Status = new() { Style = BodyStyle };
                UserName = new() { Style = BodyStyle };
                Spliter = new() { Style = SpliterStyle };
                LastEnter = new() { Style = BodyStyle };

                AccountantName.Content = Accountant.Name;
                Status.Content = "Бухгалтер";
                UserName.Content = Accountant.UserName;
                LastEnter.Content = $"{Accountant.LastEnter?.ToLongDateString()} {Accountant.LastEnter?.ToLongTimeString()}";
            }

            public AccountantListItem(Accountant accountant) {
                Accountant = accountant;

                Style = ListViewItemStyle;
                Loaded += InsuranceCompaniesListItem_Loaded;
            }

            private void InsuranceCompaniesListItem_Loaded(object sender, RoutedEventArgs e) {
                CreateVisual();

                Grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(40) });
                Grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(30) });
                _ = Grid.Children.Add(Head);
                Grid.SetRow(Body, 1);
                _ = Grid.Children.Add(Body);
                _ = Head.Children.Add(AccountantName);
                Accent.Child = Status;
                _ = Head.Children.Add(Accent);
                _ = Body.Children.Add(UserName);
                _ = Body.Children.Add(Spliter);
                _ = Body.Children.Add(LastEnter);

                Content = Grid;
            }
        }

        private class TechnicianListItem : ListViewItem {
            public Technician Technician { get; set; }
            private Grid Grid { get; set; } = null!;
            private StackPanel Head { get; set; } = null!;
            private StackPanel Body { get; set; } = null!;
            private Label TechnicianName { get; set; } = null!;
            private Border Accent { get; set; } = null!;
            private Label Status { get; set; } = null!;
            private Label UserName { get; set; } = null!;
            private Border Spliter { get; set; } = null!;
            private Label LastEnter { get; set; } = null!;
            private void CreateVisual() {
                Grid = new();
                Head = new() { Orientation = Orientation.Horizontal };
                Body = new() { Orientation = Orientation.Horizontal };
                TechnicianName = new() { Style = HeadStyle };
                Accent = new() { Style = AccentStyle };
                Status = new() { Style = BodyStyle };
                UserName = new() { Style = BodyStyle };
                Spliter = new() { Style = SpliterStyle };
                LastEnter = new() { Style = BodyStyle };

                TechnicianName.Content = Technician.Name;
                Status.Content = "Лаборант";
                UserName.Content = Technician.UserName;
                LastEnter.Content = $"{Technician.LastEnter?.ToLongDateString()} {Technician.LastEnter?.ToLongTimeString()}";
            }

            public TechnicianListItem(Technician technician) {
                Technician = technician;

                Style = ListViewItemStyle;
                Loaded += InsuranceCompaniesListItem_Loaded;
            }

            private void InsuranceCompaniesListItem_Loaded(object sender, RoutedEventArgs e) {
                CreateVisual();

                Grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(40) });
                Grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(30) });
                _ = Grid.Children.Add(Head);
                Grid.SetRow(Body, 1);
                _ = Grid.Children.Add(Body);
                _ = Head.Children.Add(TechnicianName);
                Accent.Child = Status;
                _ = Head.Children.Add(Accent);
                _ = Body.Children.Add(UserName);
                _ = Body.Children.Add(Spliter);
                _ = Body.Children.Add(LastEnter);

                Content = Grid;
            }
        }

        private object Accountant { get; set; } = null!;
        private object Technician { get; set; } = null!;
        private ObservableCollection<ListViewItem> UserListItems { get; set; } = new();
        public async Task GetUsers() {
            try {
                using LaboratoryContext database = new();
                switch (OrderBy.SelectedIndex) {
                    case 0:
                        Accountant = await database.Accountant
                            .Where(o => EF.Functions.Like(o.Id.ToString(), $"%{Where.Text}%"))
                            .ToListAsync();
                        break;
                    case 1:
                        Accountant = await database.Accountant
                            .Where(o => EF.Functions.Like(o.Id.ToString(), $"%{Where.Text}%"))
                            .OrderBy(o => o.Name)
                            .ToListAsync();
                        break;
                    case 2:
                        Accountant = await database.Accountant
                            .Where(o => EF.Functions.Like(o.Id.ToString(), $"%{Where.Text}%"))
                            .OrderByDescending(o => o.Name)
                            .ToListAsync();
                        break;
                    case 3:
                        Accountant = await database.Accountant
                            .Where(o => EF.Functions.Like(o.Id.ToString(), $"%{Where.Text}%"))
                            .OrderBy(o => o.LastEnter)
                            .ToListAsync();
                        break;
                    case 4:
                        Accountant = await database.Accountant
                            .Where(o => EF.Functions.Like(o.Id.ToString(), $"%{Where.Text}%"))
                            .OrderByDescending(o => o.LastEnter)
                            .ToListAsync();
                        break;
                }
                switch (OrderBy.SelectedIndex) {
                    case 0:
                        Technician = await database.Technicians
                            .Where(o => EF.Functions.Like(o.Id.ToString(), $"%{Where.Text}%"))
                            .ToListAsync();
                        break;
                    case 1:
                        Technician = await database.Technicians
                            .Where(o => EF.Functions.Like(o.Id.ToString(), $"%{Where.Text}%"))
                            .OrderBy(o => o.LastEnter)
                            .ToListAsync();
                        break;
                    case 2:
                        Technician = await database.Technicians
                            .Where(o => EF.Functions.Like(o.Id.ToString(), $"%{Where.Text}%"))
                            .OrderByDescending(o => o.LastEnter)
                            .ToListAsync();
                        break;
                    case 3:
                        Technician = await database.Technicians
                            .Where(o => EF.Functions.Like(o.Id.ToString(), $"%{Where.Text}%"))
                            .OrderBy(o => o.LastEnter)
                            .ToListAsync();
                        break;
                    case 4:
                        Technician = await database.Technicians
                            .Where(o => EF.Functions.Like(o.Id.ToString(), $"%{Where.Text}%"))
                            .OrderByDescending(o => o.LastEnter)
                            .ToListAsync();
                        break;
                }
                UserListItems.Clear();
                foreach (Accountant accountant in (List<Accountant>)Accountant) {
                    UserListItems.Add(new AccountantListItem(accountant));
                }
                foreach (Technician technician in (List<Technician>)Technician) {
                    UserListItems.Add(new TechnicianListItem(technician));
                }
                UserList.UpdateLayout();
            }
            catch { }
        }

        private Button Sender { get; set; }
        private static Style HeadStyle { get; set; } = null!;
        private static Style BodyStyle { get; set; } = null!;
        private static Style AccentStyle { get; set; } = null!;
        private static Style SpliterStyle { get; set; } = null!;
        private static Style ListViewItemStyle { get; set; } = null!;
        private void InitializeAdditionalComponent() {
            HeadStyle = (Style)FindResource("HeadStyle");
            BodyStyle = (Style)FindResource("BodyStyle");
            AccentStyle = (Style)FindResource("AccentStyle");
            SpliterStyle = (Style)FindResource("SpliterStyle");
            ListViewItemStyle = (Style)FindResource("ListViewItemStyle");
        }


        public AdministratorPage(Button sender) {
            Sender = sender;
            InitializeComponent();
            InitializeAdditionalComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e) {
            UserList.ItemsSource = UserListItems;
            foreach (Button item in ((StackPanel)Sender.Parent).Children) {
                item.IsEnabled = true;
            }
            Sender.IsEnabled = false;
            await GetUsers();
        }

        private async void Where_TextChanged(object sender, TextChangedEventArgs e) {
            await GetUsers();
        }

        private async void OrderBy_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            await GetUsers();
        }
    }
}