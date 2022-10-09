namespace WebApp.QuartzJobs;

public class ReadProjectFile : IJob
{
    private readonly IProjectService _projectService;
    private readonly ILogger _logger;
    public ReadProjectFile(IProjectService projectService, ILogger<AddLegacyDataJob> logger)
    {
        _logger = logger;
        _projectService = projectService;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Starting ReadProjectFile ");
        var projects = await _projectService.GetAllWithoutLgacy();
        foreach (var project in projects)
        {
            var files = Directory.GetFiles(project.DirectoryPath);
            var projectFilesCount = await _projectService.ProjectFilesCount(project.Id);
            if (files.Length != projectFilesCount)
            {
                foreach (var file in files)
                {
                    if (!await _projectService.FileExist(file))
                    {
                        await _projectService.AddProjectFile(new ProjectFile
                        {
                            CreatedAt = DateTime.Now,
                            Exported = false,
                            PdfFileUrl ="",
                            ProjectId = project.Id,                            
                        }, project);
                    }
                }
            }
        }


    }
}

