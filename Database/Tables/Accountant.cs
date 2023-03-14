namespace Database.Tables {
    public class Accountant {
        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Name { get; set; } = null!;
        public DateTime? LastEnter { get; set; }

        public List<Order> Orders { get; set; } = new();
    }
}