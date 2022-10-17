﻿

namespace WebApp.QuartzJobs;

public class DoFileOCR : IJob
{
    private readonly IProjectService _projectService;
    private readonly ILogger _logger;
    public DoFileOCR(IProjectService projectService, ILogger<DoFileOCR> logger)
    {
        _logger = logger;
        _projectService = projectService;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Starting ReadProjectExcleFile ");
        var filesToOCR=await _projectService.Get5FilesToOCR(); 
        foreach(var file in filesToOCR)
        {
            file.Status = 1;
            await _projectService.SaveASync();

            TesseractPersianOCR ocr = new TesseractPersianOCR($"{file.Project.DirectoryPath}\\{file.PdfFilePath}",file.PdfFilePath,file.Id, new TesseractPersianOCR.LoggerCallback(LoggerCallback), new TesseractPersianOCR.DbCallback(DbCallback));
            Thread t1 = new Thread(new ThreadStart(ocr.DoOCR));
            t1.Start();
        }
    }

    public void LoggerCallback(string log)
    {
        _logger.LogError("{@appName}. error in doing OCR.{@errorMessage}", "OCR", log);
    }

    public void DbCallback(int fileId, string ocrText)
    {
        _logger.LogError($"DbCallback invoked for fileId;{fileId}");
        _projectService.AddFileOCR(fileId, ocrText);
    }
}

