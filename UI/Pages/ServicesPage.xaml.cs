using Database.Tables;
using System.Collections.ObjectModel;

namespace UI.Pages {
    public partial class ServicesPage : Page {
        private class ServicesListItem : ListViewItem {
            public Service Service { get; set; }

            private Grid Grid { get; set; } = null!;
            private StackPanel Head { get; set; } = null!;
            private StackPanel Body { get; set; } = null!;
            private Label ServiceName { get; set; } = null!;
            private Border Accent { get; set; } = null!;
            private Label Status { get; set; } = null!;
            private Label Code { get; set; } = null!;
            private Border Spliter1 { get; set; } = null!;
            private Label Price { get; set; } = null!;
            private Border Spliter2 { get; set; } = null!;
            private Label Deadline { get; set; } = null!;
            private void CreateVisual() {
                Grid = new();
                Head = new() { Orientation = Orientation.Horizontal };
                Body = new() { Orientation = Orientation.Horizontal };
                ServiceName = new() { Style = HeadStyle };
                Accent = new() { Style = AccentStyle };
                Status = new() { Style = BodyStyle };
                Code = new() { Style = BodyStyle };
                Spliter1 = new() { Style = SpliterStyle };
                Price = new() { Style = BodyStyle };
                Spliter2 = new() { Style = SpliterStyle };
                Deadline = new() { Style = BodyStyle };

                ServiceName.Content = Service.Name;
                Status.Content = Service.Status.StatusName;
                Code.Content = Service.Code;
                Price.Content = $"{Service.Price} ₽";
                Deadline.Content = $"{Math.Round(Service.Deadline)} ч. {Math.Round(Service.Deadline / 100 % 60)} мин. " +
                $"± {Math.Round(Service.Deviation)} ч. {Math.Round(Service.Deviation / 100 % 60)} мин.";
            }

            public ServicesListItem(Service service) {
                Service = service;

                Style = ListViewItemStyle;
                Loaded += ServicesListItem_Loaded;
            }

            private void ServicesListItem_Loaded(object sender, RoutedEventArgs e) {
                CreateVisual();

                Grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(40) });
                Grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(30) });
                _ = Grid.Children.Add(Head);
                Grid.SetRow(Body, 1);
                _ = Grid.Children.Add(Body);
                _ = Head.Children.Add(ServiceName);
                Accent.Child = Status;
                _ = Head.Children.Add(Accent);
                _ = Body.Children.Add(Code);
                _ = Body.Children.Add(Spliter1);
                _ = Body.Children.Add(Price);
                _ = Body.Children.Add(Spliter2);
                _ = Body.Children.Add(Deadline);
                Content = Grid;
            }
        }

        private object Services { get; set; } = null!;
        private ObservableCollection<ServicesListItem> ServicesListItems { get; set; } = new();
        public async Task GetServices() {
            try {
                using LaboratoryContext database = new();
                switch (OrderBy.SelectedIndex) {
                    case 0:
                        Services = await database.Services
                            .Include(s => s.Status)
                            .Where(s => EF.Functions.Like(s.Name, $"%{Where.Text}%"))
                            .ToListAsync();
                        break;
                    case 1:
                        Services = await database.Services
                            .Include(s => s.Status)
                            .Where(s => EF.Functions.Like(s.Name, $"%{Where.Text}%"))
                            .OrderBy(s => s.Name)
                            .ToListAsync();
                        break;
                    case 2:
                        Services = await database.Services
                            .Include(s => s.Status)
                            .Where(s => EF.Functions.Like(s.Name, $"%{Where.Text}%"))
                            .OrderByDescending(s => s.Name)
                            .ToListAsync();
                        break;
                    case 3:
                        Services = await database.Services
                            .Include(s => s.Status)
                            .Where(s => EF.Functions.Like(s.Name, $"%{Where.Text}%"))
                            .OrderBy(s => s.Code)
                            .ToListAsync();
                        break;
                    case 4:
                        Services = await database.Services
                            .Include(s => s.Status)
                            .Where(s => EF.Functions.Like(s.Name, $"%{Where.Text}%"))
                            .OrderByDescending(s => s.Code)
                            .ToListAsync();
                        break;
                }

                ServicesListItems.Clear();
                foreach (Service service in (List<Service>)Services) {
                    ServicesListItems.Add(new ServicesListItem(service));
                }
                ServicesList.UpdateLayout();
            }
            catch { }
        }

        public async Task GetActualServices() {
            try {
                using LaboratoryContext database = new();
                switch (OrderBy.SelectedIndex) {
                    case 0:
                        Services = await database.Services
                            .Include(s => s.Status)
                            .Where(s => EF.Functions.Like(s.Name, $"%{Where.Text}%"))
                            .Where(s => s.StatusId == 1)
                            .ToListAsync();
                        break;
                    case 1:
                        Services = await database.Services
                            .Include(s => s.Status)
                            .Where(s => EF.Functions.Like(s.Name, $"%{Where.Text}%"))
                            .Where(s => s.StatusId == 1)
                            .OrderBy(s => s.Name)
                            .ToListAsync();
                        break;
                    case 2:
                        Services = await database.Services
                            .Include(s => s.Status)
                            .Where(s => EF.Functions.Like(s.Name, $"%{Where.Text}%"))
                            .Where(s => s.StatusId == 1)
                            .OrderByDescending(s => s.Name)
                            .ToListAsync();
                        break;
                    case 3:
                        Services = await database.Services
                            .Include(s => s.Status)
                            .Where(s => EF.Functions.Like(s.Name, $"%{Where.Text}%"))
                            .Where(s => s.StatusId == 1)
                            .OrderBy(s => s.Code)
                            .ToListAsync();
                        break;
                    case 4:
                        Services = await database.Services
                            .Include(s => s.Status)
                            .Where(s => EF.Functions.Like(s.Name, $"%{Where.Text}%"))
                            .Where(s => s.StatusId == 1)
                            .OrderByDescending(s => s.Code)
                            .ToListAsync();
                        break;
                }

                ServicesListItems.Clear();
                foreach (Service service in (List<Service>)Services) {
                    ServicesListItems.Add(new ServicesListItem(service));
                }
                ServicesList.UpdateLayout();
            }
            catch { }
        }

        public void SetDefault() {
            Where.Text = "";
            OrderBy.SelectedIndex = 0;
        }

        private Button Sender { get; set; }
        private MainPageFrame? MainPageFrame { get; set; }
        private static Style HeadStyle { get; set; } = null!;
        private static Style BodyStyle { get; set; } = null!;
        private static Style AccentStyle { get; set; } = null!;
        private static Style SpliterStyle { get; set; } = null!;
        private static Style ListViewItemStyle { get; set; } = null!;
        private ServicesInsert? ServicesInsert { get; set; }
        private ServicesUpdate? ServicesUpdate { get; set; }
        private void InitializeAdditionalComponent() {
            MainPageFrame = MainPageFrame.GetInstance();
            HeadStyle = (Style)FindResource("HeadStyle");
            BodyStyle = (Style)FindResource("BodyStyle");
            AccentStyle = (Style)FindResource("AccentStyle");
            SpliterStyle = (Style)FindResource("SpliterStyle");
            ListViewItemStyle = (Style)FindResource("ListViewItemStyle");
        }

        public ServicesPage(Button sender) {
            Sender = sender;
            InitializeComponent();
            InitializeAdditionalComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e) {
            ServicesList.ItemsSource = ServicesListItems;

            foreach (Button item in ((StackPanel)Sender.Parent).Children) {
                item.IsEnabled = true;
            }
            Sender.IsEnabled = false;

            if (User.Group == Group.Administrator) {
                await GetServices();
            }
            else {
                await GetActualServices();
            }
        }

        private async void Where_TextChanged(object sender, TextChangedEventArgs e) {
            if (User.Group == Group.Administrator) {
                await GetServices();
            }
            else {
                await GetActualServices();
            }
        }

        private async void OrderBy_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (User.Group == Group.Administrator) {
                await GetServices();
            }
            else {
                await GetActualServices();
            }
        }

        private void Insert_Click(object sender, RoutedEventArgs e) {
            ServicesInsert = new();
            ServicesInsert.Closed += ServicesInsert_Closed;
            if (MainPageFrame != null) {
                MainPageFrame.IsEnabled = false;
            }
            ServicesInsert.Show();
        }

        private async void ServicesInsert_Closed(object? sender, EventArgs e) {
            if (MainPageFrame != null) {
                MainPageFrame.IsEnabled = true;
            }
            SetDefault();
            if (User.Group == Group.Administrator) {
                await GetServices();
            }
            else {
                await GetActualServices();
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e) {
            try {
                ServicesUpdate = new(((ServicesListItem)ServicesList.SelectedItem).Service);
                ServicesUpdate.Closed += ServicesUpdate_Closed;
                if (MainPageFrame != null) {
                    MainPageFrame.IsEnabled = false;
                }
                ServicesUpdate.Show();
            }
            catch { }
        }

        private async void ServicesUpdate_Closed(object? sender, EventArgs e) {
            if (MainPageFrame != null) {
                MainPageFrame.IsEnabled = true;
            }
            SetDefault();
            if (User.Group == Group.Administrator) {
                await GetServices();
            }
            else {
                await GetActualServices();
            }
        }

        private async void Delete_Click(object sender, RoutedEventArgs e) {
            try {
                using LaboratoryContext database = new();
                Service service = ((ServicesListItem)ServicesList.SelectedItem).Service;
                Service def = database.Services.Where(s => s.Id == service.Id).ToList()[0];
                def.StatusId = 6;
                _ = database.Services.Update(def);
                _ = database.SaveChanges();
                SetDefault();
                if (User.Group == Group.Administrator) {
                    await GetServices();
                }
                else {
                    await GetActualServices();
                }
            }
            catch { }
        }
    }
}