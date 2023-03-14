namespace UI.Pages.InnerWindows {
    public partial class PatientsInsert : Window {
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

        public PatientsInsert() {
            InitializeComponent();
            InitializeAdditionalComponent();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            DragMove();
        }

        private async void InsuranceCompany_DropDownOpened(object sender, EventArgs e) {
            using LaboratoryContext database = new();
            List<InsuranceCompany> insuranceCompanies = await database.InsuranceCompanies
                .Include(i => i.Status)
                .Where(i => EF.Functions.Like(i.Name, $"%{InsuranceCompany.Text}%"))
                .Where(i => i.Status.Id == 1)
                .ToListAsync();

            InsuranceCompany.Items.Clear();
            foreach (InsuranceCompany insuranceCompany in insuranceCompanies) {
                _ = InsuranceCompany.Items.Add(new ComboBoxItem() { Content = insuranceCompany.Name, DataContext = insuranceCompany.Id });
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e) {
            PatientNameBorder.Background = Gray50Brush;
            BirthBorder.Background = Gray50Brush;
            PassportBorder.Background = Gray50Brush;
            ContactBorder.Background = Gray50Brush;
            InsuranceBorder.Background = Gray50Brush;
            bool isReady = true;
            using LaboratoryContext database = new();
            Patient patient = new();
            if (PatientName.Text == "") {
                isReady = false;
                PatientNameBorder.Background = RedBrush;
            }
            if (Birth.Text == "" || !DateOnly.TryParse(Birth.Text, out DateOnly birth)) {
                isReady = false;
                BirthBorder.Background = RedBrush;
            }
            if (Series.Text == "" || Series.Text.Length < 4 || Number.Text == "" || Number.Text.Length < 6) {
                isReady = false;
                PassportBorder.Background = RedBrush;
            }
            if (Phone.Text == "" || Email.Text == "") {
                isReady = false;
                ContactBorder.Background = RedBrush;
            }
            if (InsurancePolicy.Text=="" || InsurancePolicy.Text.Length < 16 || InsuranceCompany.SelectedIndex == -1) {
                isReady = false;
                InsuranceBorder.Background = RedBrush;
            }
            if (isReady) {
                patient.Name = PatientName.Text;
                patient.Birth = Convert.ToDateTime($"{Birth.Text} 00:00:00");
                patient.Series = Series.Text;
                patient.Number = Number.Text;
                patient.Phone = Phone.Text;
                patient.Email = Email.Text;
                patient.InsurancePolicy = InsurancePolicy.Text;
                patient.InsuranceCompanyId = Convert.ToInt32(((ComboBoxItem)InsuranceCompany.SelectedItem).DataContext);
                patient.StatusId = 1;
                _ = database.Patients.Add(patient);
                _ = database.SaveChanges();
                Close();
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e) {
            Close();
        }
    }
}
