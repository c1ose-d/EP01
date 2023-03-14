using Database.Tables;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace UI.Pages {
    public partial class InsuranceCompaniesPage : Page {
        private class InsuranceCompaniesListItem : ListViewItem {
            public InsuranceCompany InsuranceCompany { get; set; }
            private Grid Grid { get; set; } = null!;
            private StackPanel? Head { get; set; } = null!;
            private StackPanel Body { get; set; } = null!;
            private StackPanel Foot { get; set; } = null!;
            private Label InsuranceCompanyName { get; set; } = null!;
            private Border Accent { get; set; } = null!;
            private Label Status { get; set; } = null!;
            private Label Inn { get; set; } = null!;
            private Border Spliter { get; set; } = null!;
            private Label Bik { get; set; } = null!;
            private Label Address { get; set; } = null!;
            private void CreateVisual() {
                Grid = new();
                Head = new() { Orientation = Orientation.Horizontal };
                Body = new() { Orientation = Orientation.Horizontal };
                Foot = new() { Orientation = Orientation.Horizontal };
                InsuranceCompanyName = new() { Style = HeadStyle };
                Accent = new() { Style = AccentStyle };
                Status = new() { Style = BodyStyle };
                Inn = new() { Style = BodyStyle };
                Spliter = new() { Style = SpliterStyle };
                Bik = new() { Style = BodyStyle };
                Address = new() { Style = FooterStyle };

                InsuranceCompanyName.Content = InsuranceCompany.Name;
                Status.Content = InsuranceCompany.Status.StatusName;
                Inn.Content = $"ИНН {InsuranceCompany.Inn}";
                Bik.Content = $"БИК {InsuranceCompany.Bik}";
                Address.Content = InsuranceCompany.Address;
            }

            public InsuranceCompaniesListItem(InsuranceCompany insuranceCompany) {
                InsuranceCompany = insuranceCompany;

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
                _ = Head.Children.Add(InsuranceCompanyName);
                Accent.Child = Status;
                _ = Head.Children.Add(Accent);
                _ = Body.Children.Add(Inn);
                _ = Body.Children.Add(Spliter);
                _ = Body.Children.Add(Bik);
                Grid.SetRow(Foot, 2);
                _ = Grid.Children.Add(Foot);
                _ = Foot.Children.Add(Address);
                Content = Grid;
            }
        }

        private object InsuranceCompanies { get; set; } = null!;
        private ObservableCollection<InsuranceCompaniesListItem> InsuranceCompaniesListItems { get; set; } = new();
        public async Task GetInsuranceCompanies() {
            try {
                using LaboratoryContext database = new();
                switch (OrderBy.SelectedIndex) {
                    case 0:
                        InsuranceCompanies = await database.InsuranceCompanies
                            .Include(i => i.Status)
                            .Where(i => EF.Functions.Like(i.Name, $"%{Where.Text}%"))
                            .ToListAsync();
                        break;
                    case 1:
                        InsuranceCompanies = await database.InsuranceCompanies
                            .Include(i => i.Status)
                            .Where(i => EF.Functions.Like(i.Name, $"%{Where.Text}%"))
                            .OrderBy(i => i.Name)
                            .ToListAsync();
                        break;
                    case 2:
                        InsuranceCompanies = await database.InsuranceCompanies
                            .Include(i => i.Status)
                            .Where(i => EF.Functions.Like(i.Name, $"%{Where.Text}%"))
                            .OrderByDescending(i => i.Name)
                            .ToListAsync();
                        break;
                }
                InsuranceCompaniesListItems.Clear();
                foreach (InsuranceCompany insuranceCompany in (List<InsuranceCompany>)InsuranceCompanies) {
                    InsuranceCompaniesListItems.Add(new InsuranceCompaniesListItem(insuranceCompany));
                }
                InsuranceCompaniesList.UpdateLayout();
            }
            catch { }
        }
        public async Task GetActualInsuranceCompanies() {
            try {
                using LaboratoryContext database = new();
                switch (OrderBy.SelectedIndex) {
                    case 0:
                        InsuranceCompanies = await database.InsuranceCompanies
                            .Include(i => i.Status)
                            .Where(i => EF.Functions.Like(i.Name, $"%{Where.Text}%"))
                            .Where(i => i.StatusId == 1)
                            .ToListAsync();
                        break;
                    case 1:
                        InsuranceCompanies = await database.InsuranceCompanies
                            .Include(i => i.Status)
                            .Where(i => EF.Functions.Like(i.Name, $"%{Where.Text}%"))
                            .Where(i => i.StatusId == 1)
                            .OrderBy(i => i.Name)
                            .ToListAsync();
                        break;
                    case 2:
                        InsuranceCompanies = await database.InsuranceCompanies
                            .Include(i => i.Status)
                            .Where(i => EF.Functions.Like(i.Name, $"%{Where.Text}%"))
                            .Where(i => i.StatusId == 1)
                            .OrderByDescending(i => i.Name)
                            .ToListAsync();
                        break;
                }
                InsuranceCompaniesListItems.Clear();
                foreach (InsuranceCompany insuranceCompany in (List<InsuranceCompany>)InsuranceCompanies) {
                    InsuranceCompaniesListItems.Add(new InsuranceCompaniesListItem(insuranceCompany));
                }
                InsuranceCompaniesList.UpdateLayout();
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
        private InsuranceCompaniesInsert? InsuranceCompaniesInsert { get; set; }
        private InsuranceCompaniesUpdate? InsuranceCompaniesUpdate { get; set; }
        private void InitializeAdditionalComponent() {
            MainPageFrame = MainPageFrame.GetInstance();
            HeadStyle = (Style)FindResource("HeadStyle");
            BodyStyle = (Style)FindResource("BodyStyle");
            FooterStyle = (Style)FindResource("FooterStyle");
            AccentStyle = (Style)FindResource("AccentStyle");
            SpliterStyle = (Style)FindResource("SpliterStyle");
            ListViewItemStyle = (Style)FindResource("ListViewItemStyle");
        }

        public InsuranceCompaniesPage(Button sender) {
            Sender = sender;
            InitializeComponent();
            InitializeAdditionalComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e) {
            InsuranceCompaniesList.ItemsSource = InsuranceCompaniesListItems;

            foreach (Button item in ((StackPanel)Sender.Parent).Children) {
                item.IsEnabled = true;
            }
            Sender.IsEnabled = false;

            if (User.Group == Group.Administrator) {
                await GetInsuranceCompanies();
            }
            else {
                await GetActualInsuranceCompanies();
            }
        }

        private async void Where_TextChanged(object sender, TextChangedEventArgs e) {
            if (User.Group == Group.Administrator) {
                await GetInsuranceCompanies();
            }
            else {
                await GetActualInsuranceCompanies();
            }
        }

        private async void OrderBy_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (User.Group == Group.Administrator) {
                await GetInsuranceCompanies();
            }
            else {
                await GetActualInsuranceCompanies();
            }
        }

        private void Insert_Click(object sender, RoutedEventArgs e) {
            InsuranceCompaniesInsert = new();
            InsuranceCompaniesInsert.Closed += InsuranceCompaniesInsert_Closed;
            if (MainPageFrame != null) {
                MainPageFrame.IsEnabled = false;
            }
            InsuranceCompaniesInsert.Show();
        }

        private async void InsuranceCompaniesInsert_Closed(object? sender, EventArgs e) {
            if (MainPageFrame != null) {
                MainPageFrame.IsEnabled = true;
            }
            SetDefault();
            if (User.Group == Group.Administrator) {
                await GetInsuranceCompanies();
            }
            else {
                await GetActualInsuranceCompanies();
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e) {
            try {
                InsuranceCompaniesUpdate = new(((InsuranceCompaniesListItem)InsuranceCompaniesList.SelectedItem).InsuranceCompany);
                InsuranceCompaniesUpdate.Closed += InsuranceCompaniesUpdate_Closed;
                if (MainPageFrame != null) {
                    MainPageFrame.IsEnabled = false;
                }
                InsuranceCompaniesUpdate.Show();
            }
            catch { }
        }

        private async void InsuranceCompaniesUpdate_Closed(object? sender, EventArgs e) {
            if (MainPageFrame != null) {
                MainPageFrame.IsEnabled = true;
            }
            SetDefault();
            if (User.Group == Group.Administrator) {
                await GetInsuranceCompanies();
            }
            else {
                await GetActualInsuranceCompanies();
            }
        }

        private async void Delete_Click(object sender, RoutedEventArgs e) {
            try {
                using LaboratoryContext database = new();
                InsuranceCompany insuranceCompany = ((InsuranceCompaniesListItem)InsuranceCompaniesList.SelectedItem).InsuranceCompany;
                InsuranceCompany def = database.InsuranceCompanies.Where(s => s.Id == insuranceCompany.Id).ToList()[0];
                def.StatusId = 3;
                _ = database.InsuranceCompanies.Update(def);
                _ = database.SaveChanges();
                SetDefault();
                if (User.Group == Group.Administrator) {
                    await GetInsuranceCompanies();
                }
                else {
                    await GetActualInsuranceCompanies();
                }
            }
            catch { }
        }
    }
}