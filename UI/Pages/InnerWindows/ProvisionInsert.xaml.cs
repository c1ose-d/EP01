namespace UI.Pages.InnerWindows {
    public partial class ProvisionInsert : Window {
        private Order Order { get; set; }
        private Color Gray50 { get; set; }
        private Color Red { get; set; }
        private SolidColorBrush? Gray50Brush { get; set; }
        private SolidColorBrush? RedBrush { get; set; }
        private void InitializeAdditionalComponent() {
            Gray50 = (Color)Application.Current.FindResource("Gray.50");
            Red = (Color)Application.Current.FindResource("Red");
            Gray50Brush = new(Gray50);
            RedBrush = new(Red);
        }

        public ProvisionInsert(Order order) {
            Order = order;
            InitializeComponent();
            InitializeAdditionalComponent();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            DragMove();
        }

        private async void Service_DropDownOpened(object sender, EventArgs e) {
            using LaboratoryContext database = new();
            List<Service> services = await database.Services
                .Where(s => EF.Functions.Like(s.Name, $"%{Service.Text}%"))
                .ToListAsync();

            Service.Items.Clear();
            foreach (Service service in services) {
                _ = Service.Items.Add(new ComboBoxItem() { Content = service.Name, DataContext = service.Id });
            }
        }

        private async void Technician_DropDownOpened(object sender, EventArgs e) {
            using LaboratoryContext database = new();
            List<Technician> technicians = await database.Technicians
                .Where(a => EF.Functions.Like(a.Name, $"%{Technician.Text}%"))
                .ToListAsync();

            Technician.Items.Clear();
            foreach (Technician technician in technicians) {
                _ = Technician.Items.Add(new ComboBoxItem() { Content = technician.Name, DataContext = technician.Id });
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e) {
            ServiceBorder.Background = Gray50Brush;
            TechnicianBorder.Background = Gray50Brush;
            bool isReady = true;
            using LaboratoryContext database = new();
            Provision provision = new();
            if (Service.SelectedIndex == -1) {
                isReady = false;
                ServiceBorder.Background = RedBrush;
            }
            if (Technician.SelectedIndex == -1) {
                isReady = false;
                TechnicianBorder.Background = RedBrush;
            }
            if (isReady) {
                provision.ServiceId = Convert.ToInt32(((ComboBoxItem)Service.SelectedItem).DataContext);
                provision.TechnicianId = Convert.ToInt32(((ComboBoxItem)Technician.SelectedItem).DataContext);
                provision.Start = DateTime.Now;
                provision.End = DateTime.Now;
                provision.StatusId = 1;
                provision.OrderId = Order.Id;
                _ = database.Provision.Add(provision);
                _ = database.SaveChanges();
                Close();
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e) {
            Close();
        }
    }
}
