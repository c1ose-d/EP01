using Microsoft.EntityFrameworkCore;

namespace UI.Pages.InnerWindows {
    public partial class OrdersOpen : Window {
        private class ProvisionListItem : ListViewItem {
            public Provision Provision { get; set; }
            private Grid Grid { get; set; } = null!;
            private StackPanel Head { get; set; } = null!;
            private StackPanel Body { get; set; } = null!;
            private Label Service { get; set; } = null!;
            private Border Accent { get; set; } = null!;
            private Label Status { get; set; } = null!;
            private Label Technician { get; set; } = null!;
            private void CreateVisual() {
                Grid = new();
                Head = new() { Orientation = Orientation.Horizontal };
                Body = new() { Orientation = Orientation.Horizontal };
                Service = new() { Style = HeadStyle };
                Accent = new() { Style = AccentStyle };
                Status = new() { Style = BodyStyle };
                Technician = new() { Style = BodyStyle };

                Service.Content = Provision.Service.Name;
                Status.Content = Provision.Status.StatusName;
                Technician.Content = Provision.Technician.Name;
            }

            public ProvisionListItem(Provision provision) {
                Provision = provision;
                Style = ListViewItemStyle;
                Loaded += ProvisionListItem_Loaded;
            }

            private void ProvisionListItem_Loaded(object sender, RoutedEventArgs e) {
                CreateVisual();

                Grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(40) });
                Grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(30) });
                _ = Grid.Children.Add(Head);
                Grid.SetRow(Body, 1);
                _ = Grid.Children.Add(Body);
                _ = Head.Children.Add(Service);
                Accent.Child = Status;
                _ = Head.Children.Add(Accent);
                _ = Body.Children.Add(Technician);

                Content = Grid;
            }
        }

        private object Provision { get; set; } = null!;
        private ObservableCollection<ProvisionListItem> ProvisionListItems { get; set; } = new();
        public async Task GetProvision() {
            try {
                using LaboratoryContext database = new();
                Provision = await database.Provision
                    .Include(p => p.Status)
                    .Include(p => p.Service)
                    .Include(p => p.Technician)
                    .Include(p => p.Order)
                    .Where(p => p.OrderId == Order.Id)
                    .ToListAsync();

                ProvisionListItems.Clear();
                foreach (Provision provision in (List<Provision>)Provision) {
                    ProvisionListItems.Add(new ProvisionListItem(provision));
                }
                ProvisionList.UpdateLayout();
            }
            catch { }
        }

        private static Style? ListViewItemStyle { get; set; }
        private static Style HeadStyle { get; set; } = null!;
        private static Style BodyStyle { get; set; } = null!;
        private static Style AccentStyle { get; set; } = null!;
        private void InitializeAdditionalComponent() {
            ListViewItemStyle = (Style)Application.Current.FindResource("ListViewItemStyle");
            HeadStyle = (Style)FindResource("HeadStyle");
            BodyStyle = (Style)FindResource("BodyStyle");
            AccentStyle = (Style)FindResource("AccentStyle");
        }

        Order Order { get; set; }
        public OrdersOpen(Order order) {
            Order = order;
            InitializeComponent();
            InitializeAdditionalComponent();
            ProvisionList.ItemsSource = ProvisionListItems;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e) {
            Accountant.Text = Order.Accountant.Name;
            Patient.Text = Order.Patient.Name;
            Sum.Text = $"{Order.Sum} ₽";
            Deadline.Text = $"{Math.Round(Order.Deadline)} ч. {Math.Round(Order.Deadline / 100 % 60)} мин.";
            await GetProvision();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            DragMove();
        }

        private async void Approve_Click(object sender, RoutedEventArgs e) {
            try {
                using LaboratoryContext database = new();
                Provision def = database.Provision.Where(p => p.Id == ((ProvisionListItem)ProvisionList.SelectedItem).Provision.Id).ToList()[0];
                def.StatusId = 5;
                database.Provision.Update(def);
                database.SaveChanges();
                await GetProvision();
            }
            catch { }
        }

        private void Update_Click(object sender, RoutedEventArgs e) {
            using LaboratoryContext database = new();
            bool isClosed = true;
            foreach (ProvisionListItem item in ProvisionList.Items) {
                if (item.Provision.StatusId != 5) {
                    isClosed = false;
                    break;
                }
            }
            if (isClosed) {
                Order def = database.Orders.Where(p => p.Id == Order.Id).ToList()[0];
                def.StatusId = 5;
                database.Orders.Update(def);
                database.SaveChanges();
            }

            Close();
        }
    }
}
