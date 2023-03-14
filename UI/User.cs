namespace UI {
    internal enum Group {
        Administrator,
        Accountant,
        Technician
    }

    internal class User {
        public static object Object { get; set; } = null!;
        public static Group Group { get; set; }
    }
}