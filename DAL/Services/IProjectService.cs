using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Services
{
    public interface IProjectService
    {
        public Task<IList<Project>> GetAll();
        public Task<IList<Project>> GetAllWithoutLgacy();
        public Task<Project> GetLegacyProject();
        public Task AddProjectFile(ProjectFile file, Project project);
        public Task<int> ProjectFilesCount(int projectId);
    }
}
