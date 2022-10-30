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
        public Task<int> GetProjectCount();
        public Task<int> GetFilesCount();
        public Task<int> GetOcredCount();
        public Task<IList<Project>> GetAllWithoutLgacy();
        public Task<Project> GetLegacyProject();
        public Task AddProjectFile(ProjectFile file, Project project);
        public Task<int> ProjectFilesCount(int projectId);
        public Task<bool> FileExist(string fileName);
        public Task<IList<ProjectFile>> GetFilesToOCR(int count);
        
        public Task AddFileOCR(int fileId, string ocrText);

        public Task UpdateFileStatus(int fileId, int status);

        public Task SaveASync();
    }
}
