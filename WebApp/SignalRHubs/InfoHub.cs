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
            await Clients.Caller.SendAsync(
                "GetInfoCount", await GetInfoCounts());
            await base.OnConnectedAsync();
        }
        public async Task GetProjectCount()
        {
            await Clients.Caller.SendAsync(
                "GetInfoCount", await GetInfoCounts());
        }
        async Task<(int,int,int,int)> GetInfoCounts()
        {
            var projectCount = await _projectService.GetProjectCount();
            var filesCount = await _projectService.GetProjectCount();
            var ocredCount = await _projectService.GetProjectCount();
            var remainCount = filesCount - ocredCount;
            return (projectCount, filesCount, ocredCount, remainCount);
        }
       
    }
}
