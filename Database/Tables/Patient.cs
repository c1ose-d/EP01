namespace Database.Tables {
    public class Patient {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime Birth { get; set; }
        public string Series { get; set; } = null!;
        public string Number { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string InsurancePolicy { get; set; } = null!;
        public int InsuranceCompanyId { get; set; }
        public int StatusId { get; set; }

        public InsuranceCompany InsuranceCompany { get; set; } = null!;
        public Status Status { get; set; } = null!;

        public List<Order> Orders { get; set; } = new();
    }
}