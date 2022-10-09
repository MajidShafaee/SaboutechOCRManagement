namespace WebApp.QuartzJobs;

public class AddLegacyDataJob : IJob
{
    private readonly IProjectService _projectService;
    private readonly ILogger _logger;
    public AddLegacyDataJob(IProjectService projectService, ILogger<AddLegacyDataJob> logger)
    {
        _logger = logger;
        _projectService = projectService;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Starting AddLegacyDataJob ");
        var project =await _projectService.GetLegacyProject();
        if (project != null)
        {
            var dsexcelRecords = ExcleHelper.ReadExcleFile(project.ExclePath);
            if (dsexcelRecords != null && dsexcelRecords.Tables.Count > 0)
            {
                DataTable dtStudentRecords = dsexcelRecords.Tables[0];
                for (int i = 0; i < dtStudentRecords.Rows.Count; i++)
                {
                    var projectFile = new ProjectFile
                    {
                        PdfFileUrl = dtStudentRecords.Rows[i][0].ToString(),
                        CreatedAt = DateTime.Now,
                        Exported = true,
                        ProjectId = project.Id,
                    };
                    await _projectService.AddProjectFile(projectFile, project);
                   // Thread.Sleep(TimeSpan.FromSeconds(30));
                }
               
            }
            
        }
    
    }
}

