using System.Collections.ObjectModel;

namespace UI.Pages {
    public partial class PatientsPage : Page {
        private class PatientsListItem : ListViewItem {
            public Patient Patient { get; set; }
            private Grid Grid { get; set; } = null!;
            private StackPanel? Head { get; set; } = null!;
            private StackPanel Body { get; set; } = null!;
            private StackPanel Foot { get; set; } = null!;
            private Label PatientName { get; set; } = null!;
            private Border Accent { get; set; } = null!;
            private Label Status { get; set; } = null!;
            private Label Birth { get; set; } = null!;
            private Border Spliter1 { get; set; } = null!;
            private Label Phone { get; set; } = null!;
            private Border Spliter2 { get; set; } = null!;
            private Label Email { get; set; } = null!;
            private Label InsuranceCompany { get; set; } = null!;
            private void CreateVisual() {
                Grid = new();
                Head = new() { Orientation = Orientation.Horizontal };
                Body = new() { Orientation = Orientation.Horizontal };
                Foot = new() { Orientation = Orientation.Horizontal };
                PatientName = new() { Style = HeadStyle };
                Accent = new() { Style = AccentStyle };
                Status = new() { Style = BodyStyle };
                Birth = new() { Style = BodyStyle };
                Spliter1 = new() { Style = SpliterStyle };
                Phone = new() { Style = BodyStyle };
                Spliter2 = new() { Style = SpliterStyle };
                Email = new() { Style = BodyStyle };
                InsuranceCompany = new() { Style = FooterStyle };

                PatientName.Content = Patient.Name;
                Status.Content = Patient.Status.StatusName;
                Birth.Content = Patient.Birth.ToLongDateString();
                Phone.Content = Patient.Phone;
                Email.Content = Patient.Email;
                InsuranceCompany.Content = Patient.InsuranceCompany.Name;
            }

            public PatientsListItem(Patient patient) {
                Patient = patient;

                Style = ListViewItemStyle;
                Loaded += InsuranceCompaniesListItem_Loaded;
            }

            private void InsuranceCompaniesListItem_Loaded(object sender, RoutedEventArgs e) {
                CreateVisual();

                Grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(40) });
                Grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(30) });
                Grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(20) });
                Head = new() { Orientation = Orientation.Horizontal };
                _ = Grid.Children.Add(Head);
                Grid.SetRow(Body, 1);
                _ = Grid.Children.Add(Body);
                _ = Head.Children.Add(PatientName);
                Accent.Child = Status;
                _ = Head.Children.Add(Accent);
                _ = Body.Children.Add(Birth);
                _ = Body.Children.Add(Spliter1);
                _ = Body.Children.Add(Phone);
                _ = Body.Children.Add(Spliter2);
                _ = Body.Children.Add(Email);
                Grid.SetRow(Foot, 2);
                _ = Grid.Children.Add(Foot);
                _ = Foot.Children.Add(InsuranceCompany);
                Content = Grid;
            }
        }

        private object Patients { get; set; } = null!;
        private ObservableCollection<PatientsListItem> PatientsListItems { get; set; } = new();
        public async Task GetPatients() {
            try {
                using LaboratoryContext database = new();
                switch (OrderBy.SelectedIndex) {
                    case 0:
                        Patients = await database.Patients
                            .Include(p => p.Status)
                            .Include(p => p.InsuranceCompany)
                            .Where(p => EF.Functions.Like(p.Name, $"%{Where.Text}%"))
                            .ToListAsync();
                        break;
                    case 1:
                        Patients = await database.Patients
                            .Include(p => p.Status)
                            .Include(p => p.InsuranceCompany)
                            .Where(p => EF.Functions.Like(p.Name, $"%{Where.Text}%"))
                            .OrderBy(p => p.Name)
                            .ToListAsync();
                        break;
                    case 2:
                        Patients = await database.Patients
                            .Include(p => p.Status)
                            .Include(p => p.InsuranceCompany)
                            .Where(p => EF.Functions.Like(p.Name, $"%{Where.Text}%"))
                            .OrderByDescending(p => p.Name)
                            .ToListAsync();
                        break;
                }
                PatientsListItems.Clear();
                foreach (Patient patient in (List<Patient>)Patients) {
                    PatientsListItems.Add(new PatientsListItem(patient));
                }
                PatientsList.UpdateLayout();
            }
            catch { }
        }
        public async Task GetActualPatients() {
            try {
                using LaboratoryContext database = new();
                switch (OrderBy.SelectedIndex) {
                    case 0:
                        Patients = await database.Patients
                            .Include(p => p.Status)
                            .Include(p => p.InsuranceCompany)
                            .Where(p => EF.Functions.Like(p.Name, $"%{Where.Text}%"))
                            .Where(p => p.StatusId == 1)
                            .ToListAsync();
                        break;
                    case 1:
                        Patients = await database.Patients
                            .Include(p => p.Status)
                            .Include(p => p.InsuranceCompany)
                            .Where(p => EF.Functions.Like(p.Name, $"%{Where.Text}%"))
                            .Where(p => p.StatusId == 1)
                            .OrderBy(p => p.Name)
                            .ToListAsync();
                        break;
                    case 2:
                        Patients = await database.Patients
                            .Include(p => p.Status)
                            .Include(p => p.InsuranceCompany)
                            .Where(p => EF.Functions.Like(p.Name, $"%{Where.Text}%"))
                            .Where(p => p.StatusId == 1)
                            .OrderByDescending(p => p.Name)
                            .ToListAsync();
                        break;
                }
                PatientsListItems.Clear();
                foreach (Patient patient in (List<Patient>)Patients) {
                    PatientsListItems.Add(new PatientsListItem(patient));
                }
                PatientsList.UpdateLayout();
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
        private static Style ListViewItemStyle { get; set; } = null!;
        private PatientsInsert? PatientsInsert { get; set; }
        private PatientsUpdate? PatientsUpdate { get; set; }
        private void InitializeAdditionalComponent() {
            MainPageFrame = MainPageFrame.GetInstance();
            HeadStyle = (Style)FindResource("HeadStyle");
            BodyStyle = (Style)FindResource("BodyStyle");
            FooterStyle = (Style)FindResource("FooterStyle");
            AccentStyle = (Style)FindResource("AccentStyle");
            SpliterStyle = (Style)FindResource("SpliterStyle");
            ListViewItemStyle = (Style)FindResource("ListViewItemStyle");
        }

        public PatientsPage(Button sender) {
            Sender = sender;
            InitializeComponent();
            InitializeAdditionalComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e) {
            PatientsList.ItemsSource = PatientsListItems;

            foreach (Button item in ((StackPanel)Sender.Parent).Children) {
                item.IsEnabled = true;
            }
            Sender.IsEnabled = false;

            if (User.Group == Group.Administrator) {
                await GetPatients();
            }
            else {
                await GetActualPatients();
            }
        }

        private async void Where_TextChanged(object sender, TextChangedEventArgs e) {
            if (User.Group == Group.Administrator) {
                await GetPatients();
            }
            else {
                await GetActualPatients();
            }
        }

        private async void OrderBy_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (User.Group == Group.Administrator) {
                await GetPatients();
            }
            else {
                await GetActualPatients();
            }
        }

        private void Insert_Click(object sender, RoutedEventArgs e) {
            PatientsInsert = new();
            PatientsInsert.Closed += PatientsInsert_Closed;
            if (MainPageFrame != null) {
                MainPageFrame.IsEnabled = false;
            }
            PatientsInsert.Show();
        }

        private async void PatientsInsert_Closed(object? sender, EventArgs e) {
            if (MainPageFrame != null) {
                MainPageFrame.IsEnabled = true;
            }
            SetDefault();
            if (User.Group == Group.Administrator) {
                await GetPatients();
            }
            else {
                await GetActualPatients();
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e) {
            try {
                PatientsUpdate = new(((PatientsListItem)PatientsList.SelectedItem).Patient);
                PatientsUpdate.Closed += PatientsUpdate_Closed;
                if (MainPageFrame != null) {
                    MainPageFrame.IsEnabled = false;
                }
                PatientsUpdate.Show();
            }
            catch { }
        }

        private async void PatientsUpdate_Closed(object? sender, EventArgs e) {
            if (MainPageFrame != null) {
                MainPageFrame.IsEnabled = true;
            }
            SetDefault();
            if (User.Group == Group.Administrator) {
                await GetPatients();
            }
            else {
                await GetActualPatients();
            }
        }

        private async void Delete_Click(object sender, RoutedEventArgs e) {
            try {
                using LaboratoryContext database = new();
                Patient patient = ((PatientsListItem)PatientsList.SelectedItem).Patient;
                Patient def = database.Patients.Where(s => s.Id == patient.Id).ToList()[0];
                def.StatusId = 6;
                _ = database.Patients.Update(def);
                _ = database.SaveChanges();
                SetDefault();
                if (User.Group == Group.Administrator) {
                    await GetPatients();
                }
                else {
                    await GetActualPatients();
                }
            }
            catch { }
        }
    }
}