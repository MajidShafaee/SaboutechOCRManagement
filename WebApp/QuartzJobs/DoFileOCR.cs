﻿namespace WebApp.QuartzJobs;

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
        //get 4 files to OCR
        //add file object to thread
        //set callback
        //update file status    
    }
}

