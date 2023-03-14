namespace UI.Pages.InnerWindows {
    public partial class InsuranceCompaniesUpdate : Window {
        private InsuranceCompany InsuranceCompany { get; set; }
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

        public InsuranceCompaniesUpdate(InsuranceCompany insuranceCompany) {
            InsuranceCompany = insuranceCompany;
            InitializeComponent();
            InitializeAdditionalComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            InsuranceCompanyName.Text = InsuranceCompany.Name;
            Address.Text = InsuranceCompany.Address;
            Inn.Text = InsuranceCompany.Inn;
            Bik.Text = InsuranceCompany.Bik;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            DragMove();
        }

        private void Update_Click(object sender, RoutedEventArgs e) {
            InsuranceCompanyNameBorder.Background = Gray50Brush;
            AddressBorder.Background = Gray50Brush;
            InnBorder.Background = Gray50Brush;
            BikBorder.Background = Gray50Brush;
            bool isReady = true;
            using LaboratoryContext database = new();
            InsuranceCompany def = database.InsuranceCompanies.Where(i => i.Id == InsuranceCompany.Id).ToList()[0];
            if (InsuranceCompanyName.Text == "") {
                isReady = false;
                InsuranceCompanyNameBorder.Background = RedBrush;
            }
            if (Address.Text == "") {
                isReady = false;
                AddressBorder.Background = RedBrush;
            }
            if (Inn.Text == "" || Inn.Text.Length != 10) {
                isReady = false;
                InnBorder.Background = RedBrush;
            }
            if (Bik.Text == "" || Bik.Text.Length != 9) {
                isReady = false;
                BikBorder.Background = RedBrush;
            }
            if (isReady) {
                def.Name = InsuranceCompanyName.Text;
                def.Address = Address.Text;
                def.Inn = Inn.Text;
                def.Bik = Bik.Text;
                _ = database.InsuranceCompanies.Update(def);
                _ = database.SaveChanges();
                Close();
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e) {
            Close();
        }
    }
}