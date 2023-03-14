namespace Database.Tables {
    public class Service {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
        public decimal Price { get; set; }
        public decimal Deadline { get; set; }
        public decimal Deviation { get; set; }
        public int StatusId { get; set; }

        public Status Status { get; set; } = null!;

        public List<Provision> Provision { get; set; } = new();
    }
}