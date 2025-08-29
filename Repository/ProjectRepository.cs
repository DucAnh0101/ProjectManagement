using BusinessObject.Models;
using DataAccessLayer.ReqDTO;
using DataAccessLayer.ResDTO;
using Microsoft.EntityFrameworkCore;
using Repository.Implements;

namespace Repository
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ProjectManagementContext _context;

        public ProjectRepository(ProjectManagementContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Project>> GetAllProjects()
        {
            try
            {
                return await _context.Projects
                    .Where(p => p.IsActive)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách dự án.", ex);
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
                var project = await _context.Projects
                    .Where(p => p.ProjectId == projectId && p.IsActive)
                    .Select(p => new ProjectGetResDTO
                    {
                        ProjectId = p.ProjectId,
                        ProjectCode = p.ProjectCode,
                        ProjectName = p.ProjectName,
                        CustomerId = p.CustomerId,
                        ProjectType = p.ProjectType,
                        StartDate = p.StartDate,
                        EndDate = p.EndDate,
                        Objective = p.Objective,
                        Description = p.Description,
                        Status = p.Status,
                        IsActive = p.IsActive,
                        Budget = p.Budget,
                        ActualProgress = p.ActualProgress
                    })
                    .FirstOrDefaultAsync();

                if (project == null)
                {
                    throw new ArgumentException("Dự án không tồn tại hoặc đã bị xóa.");
                }

                return project;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy thông tin dự án.", ex);
            }
        }

        public async Task<ProjectResDTO> AddProject(CreateProjectReqDTO createProjectReqDTO)
        {
            if (createProjectReqDTO == null)
            {
                throw new ArgumentNullException(nameof(createProjectReqDTO), "Thông tin dự án không được để trống.");
            }

            if (string.IsNullOrWhiteSpace(createProjectReqDTO.ProjectName) ||
                string.IsNullOrWhiteSpace(createProjectReqDTO.ProjectType) ||
                string.IsNullOrWhiteSpace(createProjectReqDTO.Status))
            {
                throw new ArgumentException("Tên dự án, loại dự án và trạng thái không được để trống.");
            }

            if (createProjectReqDTO.CustomerId <= 0)
            {
                throw new ArgumentException("ID khách hàng không hợp lệ.");
            }

            var existingCustomer = await _context.Customers.FindAsync(createProjectReqDTO.CustomerId);
            if (existingCustomer == null)
            {
                throw new ArgumentException("Khách hàng không tồn tại.");
            }

            var projectCode = GenerateProjectCode(createProjectReqDTO.ProjectName);
            var existingProjectCode = await _context.Projects
                .AnyAsync(p => p.ProjectCode.ToLower() == projectCode.ToLower());
            if (existingProjectCode)
            {
                throw new ArgumentException("Mã dự án đã tồn tại, vui lòng chọn tên dự án khác.");
            }

            var project = new Project
            {
                ProjectCode = projectCode,
                ProjectName = createProjectReqDTO.ProjectName,
                CustomerId = createProjectReqDTO.CustomerId,
                ProjectType = createProjectReqDTO.ProjectType,
                StartDate = createProjectReqDTO.StartDate,
                EndDate = createProjectReqDTO.EndDate,
                Objective = createProjectReqDTO.Objective,
                Description = createProjectReqDTO.Description,
                Status = createProjectReqDTO.Status,
                IsActive = true,
                Budget = createProjectReqDTO.Budget
            };

            try
            {
                _context.Projects.Add(project);
                await _context.SaveChangesAsync();

                return new ProjectResDTO
                {
                    ProjectCode = project.ProjectCode,
                    ProjectName = project.ProjectName,
                    CustomerId = project.CustomerId,
                    ProjectType = project.ProjectType,
                    StartDate = project.StartDate,
                    EndDate = project.EndDate,
                    Objective = project.Objective,
                    Description = project.Description,
                    Status = project.Status,
                    IsActive = project.IsActive,
                    Budget = project.Budget
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tạo dự án.", ex);
            }
        }

        public async Task<ProjectResDTO> UpdateProject(int projectId, UpdateProjectReqDTO updateProjectReqDTO)
        {
            if (projectId <= 0)
            {
                throw new ArgumentException("ID dự án không hợp lệ.");
            }

            if (updateProjectReqDTO == null)
            {
                throw new ArgumentNullException(nameof(updateProjectReqDTO), "Thông tin dự án không được để trống.");
            }

            var project = await _context.Projects
                .Where(p => p.ProjectId == projectId && p.IsActive)
                .FirstOrDefaultAsync();
            if (project == null)
            {
                throw new ArgumentException("Dự án không tồn tại hoặc đã bị xóa.");
            }

            if (!string.IsNullOrWhiteSpace(updateProjectReqDTO.ProjectName))
            {
                var newProjectCode = GenerateProjectCode(updateProjectReqDTO.ProjectName);
                var existingProjectCode = await _context.Projects
                    .AnyAsync(p => p.ProjectCode.ToLower() == newProjectCode.ToLower() && p.ProjectId != projectId);
                if (existingProjectCode)
                {
                    throw new ArgumentException("Mã dự án đã tồn tại, vui lòng chọn tên dự án khác.");
                }
                project.ProjectCode = newProjectCode;
                project.ProjectName = updateProjectReqDTO.ProjectName;
            }

            if (updateProjectReqDTO.CustomerId > 0)
            {
                var existingCustomer = await _context.Customers.FindAsync(updateProjectReqDTO.CustomerId);
                if (existingCustomer == null)
                {
                    throw new ArgumentException("Khách hàng không tồn tại.");
                }
                project.CustomerId = updateProjectReqDTO.CustomerId;
            }

            if (!string.IsNullOrWhiteSpace(updateProjectReqDTO.ProjectType))
            {
                project.ProjectType = updateProjectReqDTO.ProjectType;
            }

            if (!string.IsNullOrWhiteSpace(updateProjectReqDTO.Status))
            {
                project.Status = updateProjectReqDTO.Status;
            }

            if (updateProjectReqDTO.EndDate.HasValue)
            {
                project.EndDate = updateProjectReqDTO.EndDate;
            }

            if (updateProjectReqDTO.Objective != null)
            {
                project.Objective = updateProjectReqDTO.Objective;
            }

            if (updateProjectReqDTO.Description != null)
            {
                project.Description = updateProjectReqDTO.Description;
            }

            if (updateProjectReqDTO.Budget.HasValue)
            {
                project.Budget = updateProjectReqDTO.Budget;
            }

            if (updateProjectReqDTO.ActualProgress.HasValue)
            {
                project.ActualProgress = updateProjectReqDTO.ActualProgress;
            }

            try
            {
                await _context.SaveChangesAsync();

                return new ProjectResDTO
                {
                    ProjectCode = project.ProjectCode,
                    ProjectName = project.ProjectName,
                    CustomerId = project.CustomerId,
                    ProjectType = project.ProjectType,
                    StartDate = project.StartDate,
                    EndDate = project.EndDate,
                    Objective = project.Objective,
                    Description = project.Description,
                    Status = project.Status,
                    IsActive = project.IsActive,
                    Budget = project.Budget,
                    ActualProgress = project.ActualProgress
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật dự án.", ex);
            }
        }

        public async System.Threading.Tasks.Task DeleteProject(int projectId)
        {
            if (projectId <= 0)
            {
                throw new ArgumentException("ID dự án không hợp lệ.");
            }

            var project = await _context.Projects
                .Where(p => p.ProjectId == projectId && p.IsActive)
                .FirstOrDefaultAsync();
            if (project == null)
            {
                throw new ArgumentException("Dự án không tồn tại hoặc đã bị xóa.");
            }

            try
            {
                project.IsActive = false;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xóa dự án.", ex);
            }
        }

        private string GenerateProjectCode(string projectName)
        {
            if (string.IsNullOrWhiteSpace(projectName))
            {
                throw new ArgumentException("Tên dự án không được để trống.");
            }

            var prefix = projectName.Length > 3 ? projectName.Substring(0, 3).ToUpper() : projectName.ToUpper();
            var random = new Random().Next(1000, 9999);
            return $"{prefix}-{random}";
        }
    }
}
