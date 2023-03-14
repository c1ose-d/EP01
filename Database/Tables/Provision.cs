namespace Database.Tables {
    public class Provision {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public int TechnicianId { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public int StatusId { get; set; }
        public int OrderId { get; set; }

        public Service Service { get; set; } = null!;
        public Technician Technician { get; set; } = null!;
        public Status Status { get; set; } = null!;
        public Order Order { get; set; } = null!;
    }
}