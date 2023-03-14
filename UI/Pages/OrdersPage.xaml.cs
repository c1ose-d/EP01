using Database.Tables;

namespace UI.Pages {
    public partial class OrdersPage : Page {
        private class OrdersListItem : ListViewItem {
            public Order Order { get; set; }
            private Grid Grid { get; set; } = null!;
            private StackPanel Head { get; set; } = null!;
            private StackPanel Body { get; set; } = null!;
            private List<StackPanel>? Foot { get; set; }
            private Label Info { get; set; } = null!;
            private Border Accent { get; set; } = null!;
            private Label Status { get; set; } = null!;
            private Label Accountant { get; set; } = null!;
            private Border Spliter1 { get; set; } = null!;
            private Label Sum { get; set; } = null!;
            private Border Spliter2 { get; set; } = null!;
            private Label Deadline { get; set; } = null!;
            private void CreateVisual() {
                Grid = new();
                Head = new() { Orientation = Orientation.Horizontal };
                Body = new() { Orientation = Orientation.Horizontal };
                Info = new() { Style = HeadStyle };
                Accent = new() { Style = AccentStyle };
                Status = new() { Style = BodyStyle };
                Accountant = new() { Style = BodyStyle };
                Spliter1 = new() { Style = SpliterStyle };
                Sum = new() { Style = BodyStyle };
                Spliter2 = new() { Style = SpliterStyle };
                Deadline = new() { Style = BodyStyle };

                Info.Content = $"Заказ № {Order.Id} от {Order.Date?.ToShortDateString()}";
                Status.Content = Order.Status.StatusName;
                Accountant.Content = Order.Accountant.Name;
                Sum.Content = $"{Order.Sum} ₽";
                Deadline.Content = $"{Math.Round(Order.Deadline)} ч. {Math.Round(Order.Deadline / 100 % 60)} мин.";

                Foot = new();
                using LaboratoryContext database = new();
                foreach (Provision provision in Order.Provision) {
                    StackPanel foot = new() { Orientation = Orientation.Horizontal };
                    if (provision.StatusId == 5) {
                        Label temp = new() { Content = $"{database.Provision.Include(p => p.Technician).Where(s => s.Id == provision.Id).ToList()[0].Technician.Name}: {database.Provision.Include(p => p.Service).Where(s => s.Id == provision.Id).ToList()[0].Service.Name}", Style = FooterStyle, Foreground = GreenBrush };
                        foot.Children.Add(temp);
                    }
                    else {
                        Label temp = new() { Content = $"{database.Provision.Include(p => p.Technician).Where(s => s.Id == provision.Id).ToList()[0].Technician.Name}: {database.Provision.Include(p => p.Service).Where(s => s.Id == provision.Id).ToList()[0].Service.Name}", Style = FooterStyle };
                        foot.Children.Add(temp);
                    }
                    Foot.Add(foot);
                }
            }

            public OrdersListItem(Order order) {
                Order = order;

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
                _ = Head.Children.Add(Info);
                Accent.Child = Status;
                _ = Head.Children.Add(Accent);
                _ = Body.Children.Add(Accountant);
                _ = Body.Children.Add(Spliter1);
                _ = Body.Children.Add(Sum);
                _ = Body.Children.Add(Spliter2);
                _ = Body.Children.Add(Deadline);
                for (int i = 0; i < Foot?.Count; i++) {
                    Grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(20) });
                    Grid.SetRow(Foot[i], 2 + i);
                    Grid.Children.Add(Foot[i]);
                }
                Content = Grid;
            }
        }

        private object Orders { get; set; } = null!;
        private ObservableCollection<OrdersListItem> OrdersListItems { get; set; } = new();
        public async Task GetOrders() {
            try {
                using LaboratoryContext database = new();
                switch (OrderBy.SelectedIndex) {
                    case 0:
                        Orders = await database.Orders
                            .Include(o => o.Status)
                            .Include(o => o.Accountant)
                            .Include(o => o.Patient)
                            .Include(o => o.Provision)
                            .Where(o => EF.Functions.Like(o.Id.ToString(), $"%{Where.Text}%"))
                            .Where(o => o.StatusId != null)
                            .ToListAsync();
                        break;
                    case 1:
                        Orders = await database.Orders
                            .Include(o => o.Status)
                            .Include(o => o.Accountant)
                            .Include(o => o.Patient)
                            .Include(o => o.Provision)
                            .Where(o => EF.Functions.Like(o.Id.ToString(), $"%{Where.Text}%"))
                            .OrderBy(o => o.Id)
                            .ToListAsync();
                        break;
                    case 2:
                        Orders = await database.Orders
                            .Include(o => o.Status)
                            .Include(o => o.Accountant)
                            .Include(o => o.Patient)
                            .Include(o => o.Provision)
                            .Where(o => EF.Functions.Like(o.Id.ToString(), $"%{Where.Text}%"))
                            .OrderByDescending(o => o.Id)
                            .ToListAsync();
                        break;
                    case 3:
                        Orders = await database.Orders
                            .Include(o => o.Status)
                            .Include(o => o.Accountant)
                            .Include(o => o.Patient)
                            .Include(o => o.Provision)
                            .Where(o => EF.Functions.Like(o.Id.ToString(), $"%{Where.Text}%"))
                            .OrderBy(o => o.Date)
                            .ToListAsync();
                        break;
                    case 4:
                        Orders = await database.Orders
                            .Include(o => o.Status)
                            .Include(o => o.Accountant)
                            .Include(o => o.Patient)
                            .Include(o => o.Provision)
                            .Where(o => EF.Functions.Like(o.Id.ToString(), $"%{Where.Text}%"))
                            .OrderByDescending(o => o.Date)
                            .ToListAsync();
                        break;
                }
                OrdersListItems.Clear();
                foreach (Order order in (List<Order>)Orders) {
                    OrdersListItems.Add(new OrdersListItem(order));
                }
                OrdersList.UpdateLayout();
            }
            catch { }
        }
        public async Task GetActualOrders() {
            try {
                using LaboratoryContext database = new();
                switch (OrderBy.SelectedIndex) {
                    case 0:
                        Orders = await database.Orders
                            .Include(o => o.Status)
                            .Include(o => o.Accountant)
                            .Include(o => o.Patient)
                            .Include(o => o.Provision)
                            .Where(o => EF.Functions.Like(o.Id.ToString(), $"%{Where.Text}%"))
                            .Where(o => o.StatusId == 1)
                            .Where(o => o.StatusId != null)
                            .ToListAsync();
                        break;
                    case 1:
                        Orders = await database.Orders
                            .Include(o => o.Status)
                            .Include(o => o.Accountant)
                            .Include(o => o.Patient)
                            .Include(o => o.Provision)
                            .Where(o => EF.Functions.Like(o.Id.ToString(), $"%{Where.Text}%"))
                            .Where(o => o.StatusId == 1)
                            .Where(o => o.StatusId != null)
                            .OrderBy(o => o.Id)
                            .ToListAsync();
                        break;
                    case 2:
                        Orders = await database.Orders
                            .Include(o => o.Status)
                            .Include(o => o.Accountant)
                            .Include(o => o.Patient)
                            .Include(o => o.Provision)
                            .Where(o => EF.Functions.Like(o.Id.ToString(), $"%{Where.Text}%"))
                            .Where(o => o.StatusId == 1)
                            .Where(o => o.StatusId != null)
                            .OrderByDescending(o => o.Id)
                            .ToListAsync();
                        break;
                    case 3:
                        Orders = await database.Orders
                            .Include(o => o.Status)
                            .Include(o => o.Accountant)
                            .Include(o => o.Patient)
                            .Include(o => o.Provision)
                            .Where(o => EF.Functions.Like(o.Id.ToString(), $"%{Where.Text}%"))
                            .Where(o => o.StatusId == 1)
                            .Where(o => o.StatusId != null)
                            .OrderBy(o => o.Date)
                            .ToListAsync();
                        break;
                    case 4:
                        Orders = await database.Orders
                            .Include(o => o.Status)
                            .Include(o => o.Accountant)
                            .Include(o => o.Patient)
                            .Include(o => o.Provision)
                            .Where(o => EF.Functions.Like(o.Id.ToString(), $"%{Where.Text}%"))
                            .Where(o => o.StatusId == 1)
                            .Where(o => o.StatusId != null)
                            .OrderByDescending(o => o.Date)
                            .ToListAsync();
                        break;
                }
                OrdersListItems.Clear();
                foreach (Order order in (List<Order>)Orders) {
                    OrdersListItems.Add(new OrdersListItem(order));
                }
                OrdersList.UpdateLayout();
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
        private static Style FooterStyle { get; set; } = null!;
        private static Style AccentStyle { get; set; } = null!;
        private static Style SpliterStyle { get; set; } = null!;
        private static Color Green { get; set; }
        private static SolidColorBrush GreenBrush { get; set; } = null!;
        private static Style ListViewItemStyle { get; set; } = null!;
        private OrdersInsert? OrdersInsert { get; set; }
        private OrdersOpen? OrdersOpen { get; set; }
        private void InitializeAdditionalComponent() {
            MainPageFrame = MainPageFrame.GetInstance();
            HeadStyle = (Style)FindResource("HeadStyle");
            BodyStyle = (Style)FindResource("BodyStyle");
            FooterStyle = (Style)FindResource("FooterStyle");
            AccentStyle = (Style)FindResource("AccentStyle");
            Green = (Color)FindResource("Green");
            GreenBrush = new(Green);
            SpliterStyle = (Style)FindResource("SpliterStyle");
            ListViewItemStyle = (Style)FindResource("ListViewItemStyle");
        }

        public OrdersPage(Button sender) {
            Sender = sender;
            InitializeComponent();
            InitializeAdditionalComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e) {
            OrdersList.ItemsSource = OrdersListItems;
            foreach (Button item in ((StackPanel)Sender.Parent).Children) {
                item.IsEnabled = true;
            }
            Sender.IsEnabled = false;

            if (User.Group == Group.Administrator) {
                await GetOrders();
            }
            else {
                await GetActualOrders();
            }
        }

        private async void Where_TextChanged(object sender, TextChangedEventArgs e) {
            if (User.Group == Group.Administrator) {
                await GetOrders();
            }
            else {
                await GetActualOrders();
            }
        }

        private async void OrderBy_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (User.Group == Group.Administrator) {
                await GetOrders();
            }
            else {
                await GetActualOrders();
            }
        }

        private void Insert_Click(object sender, RoutedEventArgs e) {
            OrdersInsert = new();
            OrdersInsert.Closed += OrdersInsert_Closed;
            if (MainPageFrame != null) {
                MainPageFrame.IsEnabled = false;
            }
            OrdersInsert.Show();
        }

        private async void OrdersInsert_Closed(object? sender, EventArgs e) {
            if (MainPageFrame != null) {
                MainPageFrame.IsEnabled = true;
            }
            SetDefault();
            if (User.Group == Group.Administrator) {
                await GetOrders();
            }
            else {
                await GetActualOrders();
            }
        }

        private void Open_Click(object sender, RoutedEventArgs e) {
            try {
                OrdersOpen = new(((OrdersListItem)OrdersList.SelectedItem).Order);
                OrdersOpen.Closed += OrdersOpen_Closed;
                if (MainPageFrame != null) {
                    MainPageFrame.IsEnabled = false;
                }
                OrdersOpen.Show();
            }
            catch { }
        }

        private async void OrdersOpen_Closed(object? sender, EventArgs e) {
            if (MainPageFrame != null) {
                MainPageFrame.IsEnabled = true;
            }
            SetDefault();
            if (User.Group == Group.Administrator) {
                await GetOrders();
            }
            else {
                await GetActualOrders();
            }
        }

        private async void Delete_Click(object sender, RoutedEventArgs e) {
            try {
                using LaboratoryContext database = new();
                Order order = ((OrdersListItem)OrdersList.SelectedItem).Order;
                Order def = database.Orders.Where(s => s.Id == order.Id).ToList()[0];
                def.StatusId = 6;
                _ = database.Orders.Update(def);
                _ = database.SaveChanges();
                SetDefault();
                if (User.Group == Group.Administrator) {
                    await GetOrders();
                }
                else {
                    await GetActualOrders();
                }
            }
            catch { }
        }
    }
}