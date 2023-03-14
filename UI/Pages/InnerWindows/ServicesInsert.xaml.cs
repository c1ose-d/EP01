namespace UI.Pages.InnerWindows {
    public partial class ServicesInsert : Window {
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

        public ServicesInsert() {
            InitializeComponent();
            InitializeAdditionalComponent();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            DragMove();
        }

        private void Update_Click(object sender, RoutedEventArgs e) {
            ServiceNameBorder.Background = Gray50Brush;
            CodeBorder.Background = Gray50Brush;
            PriceBorder.Background = Gray50Brush;
            DeadlineBorder.Background = Gray50Brush;
            DeviationBorder.Background = Gray50Brush;
            bool isReady = true;
            using LaboratoryContext database = new();
            Service service = new();
            if (ServiceName.Text == "") {
                isReady = false;
                ServiceNameBorder.Background = RedBrush;
            }
            if (Code.Text == "") {
                isReady = false;
                CodeBorder.Background = RedBrush;
            }
            if (Price.Text == "" || !decimal.TryParse(Price.Text, out decimal price)) {
                isReady = false;
                PriceBorder.Background = RedBrush;
            }
            if (Deadline.Text == "" || !decimal.TryParse(Deadline.Text, out decimal deadline)) {
                isReady = false;
                DeadlineBorder.Background = RedBrush;
            }
            if (Deviation.Text == "" || !decimal.TryParse(Deviation.Text, out decimal deviation)) {
                isReady = false;
                DeviationBorder.Background = RedBrush;
            }
            if (isReady) {
                service.Name = ServiceName.Text;
                service.Code = Code.Text;
                service.Price = Convert.ToDecimal(Price.Text);
                service.Deadline = Convert.ToDecimal(Deadline.Text);
                service.Deviation = Convert.ToDecimal(Deviation.Text);
                service.StatusId = 1;
                _ = database.Services.Add(service);
                _ = database.SaveChanges();
                Close();
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e) {
            Close();
        }
    }
}