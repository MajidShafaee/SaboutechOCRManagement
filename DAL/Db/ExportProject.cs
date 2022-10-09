



namespace DAL
{
    public class ExportProject
    {
        [Key]
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        [Required]
        public int FileIdStart { get; set; }
        [Required]
        public int FileIdEnd { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
