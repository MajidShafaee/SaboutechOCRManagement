

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
        var filesToOCR=await _projectService.GetFilesToOCR(5); 
        foreach(var file in filesToOCR)
        {
           await _projectService.UpdateFileStatus(file.Id, 1);

            TesseractPersianOCR ocr = new TesseractPersianOCR($"{file.Project.DirectoryPath}\\{file.PdfFilePath}",file.PdfFilePath,file.Id, new TesseractPersianOCR.LoggerCallback(LoggerCallback), new TesseractPersianOCR.DbCallback(DbCallback), new TesseractPersianOCR.RunNewOCRCallback(RunNewOCR));
            Thread t1 = new Thread(new ThreadStart(ocr.DoOCR));
            t1.Start();
        }
    }

    public void LoggerCallback(string log, int fileId)
    {
        _projectService.UpdateFileStatus(fileId, 3);
        _logger.LogError($"Error in doing OCR. {log}");
    }

    public void DbCallback(int fileId, string ocrText)
    {
        _logger.LogError($"DbCallback invoked for fileId;{fileId}");
        _projectService.AddFileOCR(fileId, ocrText);
    }
    public void RunNewOCR()
    {
        var fileToOCR = _projectService.GetFilesToOCR(1).Result;
        if (fileToOCR.Count == 1)
        {
            var file = fileToOCR.First();
            _projectService.UpdateFileStatus(file.Id, 1).Wait();
            TesseractPersianOCR ocr = new TesseractPersianOCR($"{file.Project.DirectoryPath}\\{file.PdfFilePath}", file.PdfFilePath, file.Id, new TesseractPersianOCR.LoggerCallback(LoggerCallback), new TesseractPersianOCR.DbCallback(DbCallback), new TesseractPersianOCR.RunNewOCRCallback(RunNewOCR));
            Thread t1 = new Thread(new ThreadStart(ocr.DoOCR));
            t1.Start();

        }

    }
}

