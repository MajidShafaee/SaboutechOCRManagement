using DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataConvertor
{
    public class FileToExport
    {
        public int Id { get; set; }
        public string PdfFileUrl { get; set; }
        public string PdfFilePath { get; set; }
        public string JournalName { get; set; }
        public string Year { get; set; }
        public string TitleFa { get; set; }
        public string TitleEn { get; set; }
        public string ISSN { get; set; }
        public string AuthorsFa { get; set; }
        public string AuthorsEn { get; set; }
        public string Volume { get; set; }
        public string Issue { get; set; }
        public string OcrText { get; set; }

    }
}
