namespace Database {
    public class LaboratoryContext : DbContext {
        public DbSet<Accountant> Accountant { get; set; }
        public DbSet<Administrator> Administrator { get; set; }
        public DbSet<InsuranceCompany> InsuranceCompanies { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Provision> Provision { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Technician> Technicians { get; set; }

        public LaboratoryContext() { }
        public LaboratoryContext(DbContextOptions<LaboratoryContext> dbContextOptions) : base(dbContextOptions) { }
        protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptionsBuilder) {
            _ = dbContextOptionsBuilder.UseSqlServer("Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=Laboratory;Data Source=DESKTOP-SGD0ON5\\SQLEXPRESS;Trust Server Certificate = true");
        }
    }
}