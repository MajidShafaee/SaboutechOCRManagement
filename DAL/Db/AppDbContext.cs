

namespace DAL
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public AppDbContext()
        {
        }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectFile> ProjectFiles { get; set; }
        public DbSet<FileOCR> FileOCRs { get; set; }
        public DbSet<ExportProject> ExportProjects { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
              optionsBuilder.UseSqlServer("server=DESKTOP-SABOU\\SQLEXPRESS2014;Database=ocr_management;User Id=ocrmng;Password=ocrmng");
            //optionsBuilder.UseSqlServer("server=DESKTOP-QF0K74V\\MSSQLSERVER17;Database=SaboutechOcrManagement;User Id=ocrmng;Password=ocrmng");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            //modelBuilder.Entity<Project>().HasData(new Project()
            //{
            //    CreatedAt=DateTime.Now,
            //    DirectoryPath="legacy",
            //    ExclePath="legacy",                
            //});
        }

    }
}
