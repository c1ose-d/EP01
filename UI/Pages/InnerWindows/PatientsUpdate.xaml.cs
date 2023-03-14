namespace UI.Pages.InnerWindows {
    public partial class PatientsUpdate : Window {
        private Patient Patient { get; set; }
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

        public PatientsUpdate(Patient patient) {
            Patient = patient;
            InitializeComponent();
            InitializeAdditionalComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            PatientName.Text = Patient.Name;
            Birth.Text = Patient.Birth.ToShortDateString();
            Series.Text = Patient.Series;
            Number.Text = Patient.Number;
            Phone.Text = Patient.Phone;
            Email.Text = Patient.Email;
            InsurancePolicy.Text = Patient.InsurancePolicy;
            _ = InsuranceCompany.Items.Add(new ComboBoxItem() { Content = Patient.InsuranceCompany.Name, DataContext = Patient.InsuranceCompanyId });
            InsuranceCompany.SelectedIndex = 0;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            DragMove();
        }

        private async void InsuranceCompany_DropDownOpened(object sender, EventArgs e) {
            using LaboratoryContext database = new();
            List<InsuranceCompany> insuranceCompanies = await database.InsuranceCompanies
                .Include(i => i.Status)
                .Where(i => EF.Functions.Like(i.Name, $"%{InsuranceCompany.Text}%"))
                .Where(i => i.Id != Patient.InsuranceCompanyId)
                .Where(i => i.Status.Id == 1)
                .ToListAsync();

            InsuranceCompany.Items.Clear();
            _ = InsuranceCompany.Items.Add(new ComboBoxItem() { Content = Patient.InsuranceCompany.Name, DataContext = Patient.InsuranceCompanyId });
            InsuranceCompany.SelectedIndex = 0;
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
            Patient def = database.Patients.Where(i => i.Id == Patient.Id).ToList()[0];
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
            if (InsurancePolicy.Text == "" || InsurancePolicy.Text.Length < 16 || InsuranceCompany.SelectedIndex == -1) {
                isReady = false;
                InsuranceBorder.Background = RedBrush;
            }
            if (isReady) {
                def.Name = PatientName.Text;
                def.Birth = Convert.ToDateTime($"{Birth.Text} 00:00:00");
                def.Series = Series.Text;
                def.Number = Number.Text;
                def.Phone = Phone.Text;
                def.Email = Email.Text;
                def.InsurancePolicy = InsurancePolicy.Text;
                def.InsuranceCompanyId = Convert.ToInt32(((ComboBoxItem)InsuranceCompany.SelectedItem).DataContext);
                _ = database.Patients.Update(def);
                _ = database.SaveChanges();
                Close();
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e) {
            Close();
        }
    }
}