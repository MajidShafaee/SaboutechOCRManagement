



namespace DAL
{
    public class Project
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string DirectoryPath { get; set; }
        [Required]
        public string ExclePath { get; set; }
        public bool ReadAllFiles { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public ICollection<ProjectFile> ProjectFiles { get; set; }
        public ICollection<ExportProject> ExportProjects { get; set; }

    }
}
