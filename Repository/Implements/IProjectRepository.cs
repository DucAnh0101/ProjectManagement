using BusinessObject.Models;
using DataAccessLayer.ReqDTO;
using DataAccessLayer.ResDTO;
using Task = System.Threading.Tasks.Task;

namespace Repository.Implements
{
    public interface IProjectRepository
    {
        Task<IEnumerable<Project>> GetAllProjects();
        Task<ProjectGetResDTO> GetProjectById(int projectId);
        Task<ProjectResDTO> AddProject(CreateProjectReqDTO createProjectReqDTO);
        Task<ProjectResDTO> UpdateProject(int projectId, UpdateProjectReqDTO updateProjectReqDTO);
        Task DeleteProject(int projectId);
    }
}
