namespace Database.Tables {
    public class InsuranceCompany {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Inn { get; set; } = null!;
        public string Bik { get; set; } = null!;
        public int StatusId { get; set; }

        public Status Status { get; set; } = null!;

        public List<Patient> Patients { get; set; } = new();
    }
}