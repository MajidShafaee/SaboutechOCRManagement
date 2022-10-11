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
                var dsexcelRecords = ExcleHelper.ReadExcleFile(project.ExclePath);
                if (dsexcelRecords != null && dsexcelRecords.Tables.Count > 0)
                {
                    DataTable dtStudentRecords = dsexcelRecords.Tables[0];
                    for (int i = 1; i < dtStudentRecords.Rows.Count; i++)
                    {
                        var lng = dtStudentRecords.Rows[i][10].ToString();                        
                        if (lng == "fa")
                        {
                            var projectFile = new ProjectFile
                            {
                                PdfFileUrl = dtStudentRecords.Rows[i][0].ToString(),
                                PdfFilePath = dtStudentRecords.Rows[i][1].ToString(),
                                JournalName = dtStudentRecords.Rows[i][2].ToString(),
                                Volume = dtStudentRecords.Rows[i][3].ToString(),
                                Issue = dtStudentRecords.Rows[i][4].ToString(),
                                Year = dtStudentRecords.Rows[i][5].ToString(),
                                TitleFa = dtStudentRecords.Rows[i][6].ToString(),
                                TitleEn = dtStudentRecords.Rows[i][7].ToString(),
                                ISSN = dtStudentRecords.Rows[i][11].ToString(),
                                AuthorsFa = dtStudentRecords.Rows[i][12].ToString(),
                                AuthorsEn = dtStudentRecords.Rows[i][13].ToString(),
                                CreatedAt = DateTime.Now,
                                Exported = false,
                                ProjectId = project.Id,
                            };
                            if (!await _projectService.FileExist(projectFile.PdfFileUrl))
                            {
                                await _projectService.AddProjectFile(projectFile, project);

                            }
                            
                        }                        
                    }
                }
            }
        }
    }
}

