namespace Database.Tables {
    public class Order {
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public int? AccountantId { get; set; }
        public int? PatientId { get; set; }
        public decimal Deadline { get; set; }
        public decimal Sum { get; set; }
        public int? StatusId { get; set; }

        public Accountant Accountant { get; set; } = null!;
        public Patient Patient { get; set; } = null!;
        public Status Status { get; set; } = null!;

        public List<Provision> Provision { get; set; } = new();
    }
}