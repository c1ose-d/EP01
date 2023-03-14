namespace UI.Pages.InnerWindows {
    public partial class OrdersInsert : Window {
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
        private static ProvisionInsert? ProvisionInsert { get; set; }
        private void InitializeAdditionalComponent() {
            ListViewItemStyle = (Style)Application.Current.FindResource("ListViewItemStyle");
            HeadStyle = (Style)FindResource("HeadStyle");
            BodyStyle = (Style)FindResource("BodyStyle");
            AccentStyle = (Style)FindResource("AccentStyle");
        }

        private Order Order { get; set; }
        public OrdersInsert() {
            InitializeComponent();
            InitializeAdditionalComponent();
            ProvisionList.ItemsSource = ProvisionListItems;

            using LaboratoryContext database = new();
            Order = new();
            _ = database.Orders.Add(Order);
            _ = database.SaveChanges();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            DragMove();
        }

        private async void Accountant_DropDownOpened(object sender, EventArgs e) {
            using LaboratoryContext database = new();
            List<Accountant> accountant = await database.Accountant
                .Where(a => EF.Functions.Like(a.Name, $"%{Accountant.Text}%"))
                .ToListAsync();

            Accountant.Items.Clear();
            foreach (Accountant item in accountant) {
                _ = Accountant.Items.Add(new ComboBoxItem() { Content = item.Name, DataContext = item.Id });
            }
        }

        private async void Patient_DropDownOpened(object sender, EventArgs e) {
            using LaboratoryContext database = new();
            List<Patient> patients = await database.Patients
                .Where(p => EF.Functions.Like(p.Name, $"%{Patient.Text}%"))
                .Where(p => p.Status.Id == 1)
                .ToListAsync();

            Patient.Items.Clear();
            foreach (Patient patient in patients) {
                _ = Patient.Items.Add(new ComboBoxItem() { Content = patient.Name, DataContext = patient.Id });
            }
        }

        private void Insert_Click(object sender, RoutedEventArgs e) {
            ProvisionInsert = new(Order);
            ProvisionInsert.Closed += ProvisionInsert_Closed;
            IsEnabled = false;
            ProvisionInsert.Show();
        }

        private async void ProvisionInsert_Closed(object? sender, EventArgs e) {
            IsEnabled = true;
            await GetProvision();
            decimal sum = 0, deadline = 0;
            foreach (ProvisionListItem item in ProvisionList.Items) {
                sum += item.Provision.Service.Price;
                deadline += item.Provision.Service.Deadline;
            }
            Sum.Text = $"{sum} ₽";
            Deadline.Text = $"{Math.Round(deadline)} ч. {Math.Round(deadline / 100 % 60)} мин.";
        }

        private async void Delete_Click(object sender, RoutedEventArgs e) {
            try {
                using LaboratoryContext database = new();
                _ = database.Provision.Remove(((ProvisionListItem)ProvisionList.SelectedItem).Provision);
                _ = database.SaveChanges();
                await GetProvision();
            }
            catch { }
        }

        private void Update_Click(object sender, RoutedEventArgs e) {
            using LaboratoryContext database = new();
            Order defOrder = database.Orders.Where(o => o.Id == Order.Id).ToList()[0];
            List<Provision> defProvision = new();
            foreach (ProvisionListItem item in ProvisionList.Items) {
                defProvision.Add(database.Provision.Where(p => p.Id == item.Provision.Id).ToList()[0]);
            }

            defOrder.Date = DateTime.Now;
            defOrder.AccountantId = Convert.ToInt32(((ComboBoxItem)Accountant.SelectedItem).DataContext);
            defOrder.PatientId = Convert.ToInt32(((ComboBoxItem)Patient.SelectedItem).DataContext);
            decimal sum = 0, deadline = 0;
            foreach (ProvisionListItem item in ProvisionList.Items) {
                sum += item.Provision.Service.Price;
                deadline += item.Provision.Service.Deadline;
            }
            defOrder.Sum = sum;
            defOrder.Deadline = deadline;
            defOrder.StatusId = 1;
            _ = database.Orders.Update(defOrder);
            foreach (Provision item in defProvision) {
                item.Start = DateTime.Now;
                item.End = DateTime.Now.AddHours((double)deadline);
                _ = database.Provision.Update(item);
            }
            _ = database.SaveChanges();
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e) {
            using LaboratoryContext database = new();
            _ = database.Orders.Remove(Order);
            foreach (ProvisionListItem item in ProvisionList.Items) {
                _ = database.Provision.Remove(item.Provision);
                _ = database.SaveChanges();
            }
            _ = database.SaveChanges();
            Close();
        }
    }
}
