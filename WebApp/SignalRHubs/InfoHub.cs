using Microsoft.AspNetCore.SignalR;

namespace WebApp.SignalRHubs
{
    public class InfoHub: Hub
    {
        private readonly IProjectService _projectService;
        public InfoHub(IProjectService projectService)
        {
            _projectService = projectService;
        }
        public override async Task OnConnectedAsync()
        {
            (int projectCount, int filesCount, int ocredCount, int remainCount) = await GetInfoCounts();
            
            await Clients.Caller.SendAsync(
                "GetInfoCount", projectCount,filesCount,ocredCount,remainCount);
            await base.OnConnectedAsync();
        }
        public async Task GetProjectCount()
        {
            await Clients.Caller.SendAsync(
                "GetInfoCount", await GetInfoCounts());
        }
        public async Task<(int,int,int,int)> GetInfoCounts()
        {
            var projectCount = await _projectService.GetProjectCount();
            var filesCount = await _projectService.GetFilesCount();
            var ocredCount = await _projectService.GetOcredCount();
            var remainCount = filesCount - ocredCount;
            return (projectCount, filesCount, ocredCount, remainCount);
        }
       
    }
}
