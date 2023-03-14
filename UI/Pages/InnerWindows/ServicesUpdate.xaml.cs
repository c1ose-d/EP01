namespace UI.Pages.InnerWindows {
    public partial class ServicesUpdate : Window {
        private Service Service { get; set; }
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

        public ServicesUpdate(Service service) {
            Service = service;
            InitializeComponent();
            InitializeAdditionalComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            ServiceName.Text = Service.Name;
            Code.Text = Service.Code;
            Price.Text = Service.Price.ToString();
            Deadline.Text = Service.Deadline.ToString();
            Deviation.Text = Service.Deviation.ToString();
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
            Service def = database.Services.Where(s => s.Id == Service.Id).ToList()[0];
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
                def.Name = ServiceName.Text;
                def.Code = Code.Text;
                def.Price = Convert.ToDecimal(Price.Text);
                def.Deadline = Convert.ToDecimal(Deadline.Text);
                def.Deviation = Convert.ToDecimal(Deviation.Text);
                _ = database.Services.Update(def);
                _ = database.SaveChanges();
                Close();
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e) {
            Close();
        }
    }
}