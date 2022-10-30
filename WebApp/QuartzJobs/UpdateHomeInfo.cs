using Microsoft.AspNetCore.SignalR;
using WebApp.SignalRHubs;

namespace WebApp.QuartzJobs;

public class UpdateHomeInfo : IJob
{
    private readonly IProjectService _projectService;
    private readonly ILogger _logger;
    IHubContext<InfoHub> _infoHub;
    public UpdateHomeInfo(IHubContext<InfoHub> infoHub,IProjectService projectService, ILogger<ReadProjectFile> logger)
    {
        _logger = logger;
        _projectService = projectService;
        _infoHub = infoHub;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var projectCount = await _projectService.GetProjectCount();
        var filesCount = await _projectService.GetFilesCount();
        var ocredCount = await _projectService.GetOcredCount();
        var remainCount = filesCount - ocredCount;
        await _infoHub.Clients.All.SendAsync("GetInfoCount", projectCount, filesCount, ocredCount, remainCount);
    }
}

