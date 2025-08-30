using BusinessObject.Models;
using DataAccessLayer.ReqDTO;
using DataAccessLayer.ResDTO;
using Repository.Implements;
using Service.Implements;

namespace Service
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectService(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository ?? throw new ArgumentNullException(nameof(projectRepository));
        }

        public async Task<IEnumerable<Project>> GetAllProjects()
        {
            try
            {
                return await _projectRepository.GetAllProjects();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ProjectGetResDTO> GetProjectById(int projectId)
        {
            if (projectId <= 0)
            {
                throw new ArgumentException("ID dự án không hợp lệ.");
            }

            try
            {
                var project = await _projectRepository.GetProjectById(projectId);
                if (project == null)
                {
                    throw new ArgumentException("Dự án không tồn tại.");
                }
                return project;
            }
            catch (ArgumentException ex)
            {
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ProjectResDTO> AddProject(CreateProjectReqDTO createProjectReqDTO)
        {
            if (createProjectReqDTO == null)
            {
                throw new ArgumentNullException(nameof(createProjectReqDTO), "Thông tin dự án không được để trống.");
            }

            try
            {
                return await _projectRepository.AddProject(createProjectReqDTO);
            }
            catch (ArgumentException ex)
            {
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ProjectResDTO> UpdateProject(int projectId, UpdateProjectReqDTO updateProjectReqDTO)
        {
            if (projectId <= 0)
            {
                throw new ArgumentException("ID dự án không hợp lệ.");
            }

            var existingProject = await _projectRepository.GetProjectById(projectId);
            if (existingProject == null)
            {
                throw new ArgumentException("Dự án không tồn tại.");
            }

            if (updateProjectReqDTO == null)
            {
                throw new ArgumentNullException("Thông tin dự án không được để trống.");
            }

            try
            {
                return await _projectRepository.UpdateProject(projectId, updateProjectReqDTO);
            }
            catch (ArgumentException ex)
            {
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async System.Threading.Tasks.Task DeleteProject(int projectId)
        {
            if (projectId <= 0)
            {
                throw new ArgumentException("ID dự án không hợp lệ.");
            }

            var project = await _projectRepository.GetProjectById(projectId);
            if (project == null)
            {
                throw new ArgumentException("Dự án không tồn tại.");
            }

            try
            {
                await _projectRepository.DeleteProject(projectId);
            }
            catch (ArgumentException ex)
            {
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
