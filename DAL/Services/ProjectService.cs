using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Services
{
    public class ProjectService : IProjectService
    {
        private readonly AppDbContext _appDbContext;

        public ProjectService(AppDbContext appDbContext) => _appDbContext = appDbContext;

        public async Task<IList<Project>> GetAll() => await _appDbContext.Projects.AsNoTracking().ToListAsync();

        public async Task<Project> GetLegacyProject()
        {
            return await _appDbContext.Projects.Include(i => i.ProjectFiles).FirstOrDefaultAsync(c => c.DirectoryPath == "legacy");

        }

        public async Task AddProjectFile(ProjectFile file, Project project)
        {            
                project.ProjectFiles.Add(file);               
        }

        public async Task<IList<Project>> GetAllWithoutLgacy()
        {
            return await _appDbContext.Projects.Include(i => i.ProjectFiles).Where(c => c.DirectoryPath!="legacy" && c.ReadAllFiles==false).ToListAsync();
        }

        public async Task<int> ProjectFilesCount(int projectId)
        {
           return await _appDbContext.ProjectFiles.CountAsync(c=>c.ProjectId==projectId);
        }

        public async Task<bool> FileExist(string pdfFileUrl)
        {
            return await _appDbContext.ProjectFiles.AnyAsync(c=>c.PdfFileUrl== pdfFileUrl);
        }

        public async Task<IList<ProjectFile>> Get5FilesToOCR()
        {
           return await _appDbContext.ProjectFiles.Include(c=>c.Project).Include(c => c.FileOCR).Where(c =>c.ProjectId!=1 && c.Status==0 && c.FileOCR == null).Take(5).ToListAsync();
        }

        public async Task SaveASync()
        {
            await _appDbContext.SaveChangesAsync();
        }

        public async Task AddFileOCR(int fileId, string ocrText)
        {
            Console.WriteLine($"Saving File:{fileId}");
            try
            {
                using var appDbCntx = new AppDbContext();
                var file=await appDbCntx.ProjectFiles.FirstOrDefaultAsync(c => c.Id == fileId); 
                file.Status = 2;
                appDbCntx.Update(file);
                appDbCntx.FileOCRs.Add(new FileOCR
                {
                    CreatedAt = DateTime.Now,
                    OcrText = ocrText,
                    ProjectFileId = fileId,

                });
                Console.WriteLine($"FileId: {fileId} Status Updated to 2");
                await appDbCntx.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro Saving File:{fileId}, {ex.Message}");
            }
        }

        public async Task UpdateFileStatus(int fileId, int status)
        {
            try
            {
                using var appDbCntx = new AppDbContext();
                var file =await _appDbContext.ProjectFiles.FirstOrDefaultAsync(c=>c.Id==fileId);
                file.Status = status;
                appDbCntx.Update(file);
                await appDbCntx.SaveChangesAsync();
                Console.WriteLine($"FileId: {fileId} Status Updated to {status}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro Updating File Status to {status}: fileId:{fileId}, {ex.Message}");
            }
        }
    }
}
