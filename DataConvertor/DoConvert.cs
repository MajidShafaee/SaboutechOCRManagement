using DAL;
using DAL.Services;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataConvertor
{
    public class DoConvert : IJob
    {
        private readonly IProjectService _projectService;
        private readonly ILogger _logger;

        public DoConvert(IProjectService projectService, ILogger<DoConvert> logger)
        {
            _logger = logger;
            _projectService = projectService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("Starting Exportig...");
            var exportProjects = await _projectService.GetReadyToExortProjects();
            foreach (var ep in exportProjects)
            {
                
                if(ep.FileIdStart==0 && ep.FileIdEnd==0)
                {
                    var exportFiles =await _projectService.GetFilesToExport(false);
                    await DoExport(exportFiles,ep.Id);
                }
                else
                {
                    var exportFiles = await _projectService.GetFilesToExport(ep.FileIdStart,ep.FileIdEnd);
                    await DoExport(exportFiles, ep.Id);
                }
            }
        }
        private async Task DoExport(List<ProjectFile> exportFiles, int epId)
        {
            File.Create(@$"E:\ExportedFiles\ocr_management_export_{epId}.json");
            using (var streamWriter = new StreamWriter(@$"C:\Temp\ocr_management_export_{epId}.json"))
            using (var jsonWriter = new JsonTextWriter(streamWriter) { Formatting = Newtonsoft.Json.Formatting.Indented, Indentation = 2, IndentChar = ' ' })
            {
                jsonWriter.WriteStartArray();
                foreach (var file in exportFiles)
                {
                    await jsonWriter.WriteStartObjectAsync();
                    await jsonWriter.WritePropertyNameAsync("FileId");
                    await jsonWriter.WriteValueAsync(file.Id);

                    await jsonWriter.WritePropertyNameAsync("JournalName");
                    await jsonWriter.WriteValueAsync(file.JournalName);
                     
                    await jsonWriter.WritePropertyNameAsync("ISSN");
                    await jsonWriter.WriteValueAsync(file.ISSN);
                     
                    await jsonWriter.WritePropertyNameAsync("AuthorsEn");
                    await jsonWriter.WriteValueAsync(file.AuthorsEn);
                     
                    await jsonWriter.WritePropertyNameAsync("AuthorsFa");
                    await jsonWriter.WriteValueAsync(file.AuthorsFa);
                     
                    await jsonWriter.WritePropertyNameAsync("Issue");
                    await jsonWriter.WriteValueAsync(file.Issue);
                     
                    await jsonWriter.WritePropertyNameAsync("PdfFileUrl");
                    await jsonWriter.WriteValueAsync(file.PdfFileUrl);
                     
                    await jsonWriter.WritePropertyNameAsync("TitleEn");
                    await jsonWriter.WriteValueAsync(file.TitleEn);
                     
                    await jsonWriter.WritePropertyNameAsync("TitleFa");
                    await jsonWriter.WriteValueAsync(file.TitleFa);
                     
                    await jsonWriter.WritePropertyNameAsync("Volume");
                    await jsonWriter.WriteValueAsync(file.Volume);
                     
                    await jsonWriter.WritePropertyNameAsync("Year");
                    await jsonWriter.WriteValueAsync(file.Year);
                     
                    await jsonWriter.WritePropertyNameAsync("OcrText");
                    await jsonWriter.WriteValueAsync(file.FileOCR.OcrText);
                     
                    await jsonWriter.WriteEndObjectAsync();

                    await _projectService.UpdateFileExportStatus(true, file.Id);
                }
            }
        }
    }
}
