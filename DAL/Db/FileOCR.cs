
namespace DAL
{
    public class FileOCR
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string OcrText { get; set; }
        public int ProjectFileId { get; set; }
        public ProjectFile ProjectFile { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
