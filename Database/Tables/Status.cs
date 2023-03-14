namespace Database.Tables {
    public class Status {
        public int Id { get; set; }
        public string StatusName { get; set; } = null!;

        public List<InsuranceCompany> InsuranceCompanies { get; set; } = new();
        public List<Order> Orders { get; set; } = new();
        public List<Patient> Patients { get; set; } = new();
        public List<Provision> Provision { get; set; } = new();
        public List<Service> Services { get; set; } = new();
    }
}