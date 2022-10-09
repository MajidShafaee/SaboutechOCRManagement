using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ProjectFile
    {
        [Key]
        public int Id { get; set; }
        public Project Project { get; set; }
        public int ProjectId { get; set; }
        public FileOCR  FileOCR { get; set; }        
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        [DefaultValue(false)]
        public bool Exported { get; set; }=false;
        public string PdfFileUrl { get; set; }
    }
}
