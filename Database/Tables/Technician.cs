namespace Database.Tables {
    public class Technician {
        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Name { get; set; } = null!;
        public DateTime? LastEnter { get; set; }

        public List<Provision> Provision { get; set; } = new();
    }
}