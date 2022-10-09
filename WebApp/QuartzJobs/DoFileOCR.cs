

namespace WebApp.QuartzJobs;

public class DoFileOCR : IJob
{
    private readonly IProjectService _projectService;
    private readonly ILogger _logger;
    public DoFileOCR(IProjectService projectService, ILogger<AddLegacyDataJob> logger)
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
            TesseractPersianOCR ocr = new TesseractPersianOCR(file.PdfFileUrl, new TesseractPersianOCR.LoggerCallback(LoggerCallback), new TesseractPersianOCR.DbCallback(DbCallback));
            Thread t1 = new Thread(new ThreadStart(ocr.DoOCR));
            t1.Start();
        }
    }

    public void LoggerCallback(string log)
    {
        _logger.LogError("{@appName}. error in doing OCR.{@errorMessage}", "OCR", log);
    }

    public void DbCallback(string ocrText)
    {
         _logger.LogError("{@appName}. error in doing OCR.{@errorMessage}", "OCR", ocrText);
    }
}

