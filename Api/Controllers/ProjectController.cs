using DataAccessLayer.ReqDTO;
using Microsoft.AspNetCore.Mvc;
using Service.Implements;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;
        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProjects()
        {
            var projects = await _projectService.GetAllProjects();
            return Ok(projects);
        }

        [HttpGet("{projectId}")]
        public async Task<IActionResult> GetProjectById(int projectId)
        {
            var project = await _projectService.GetProjectById(projectId);
            if (project == null)
            {
                return NotFound();
            }
            return Ok(project);
        }

        [HttpPost("create-project")]
        public async Task<IActionResult> AddProject([FromBody] CreateProjectReqDTO createProjectReqDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var project = await _projectService.AddProject(createProjectReqDTO);
            return Ok(project);
        }

        [HttpPut("update-project/{projectId}")]
        public async Task<IActionResult> UpdateProject(int projectId, [FromBody] UpdateProjectReqDTO updateProjectReqDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var project = await _projectService.UpdateProject(projectId, updateProjectReqDTO);
            if (project == null)
            {
                return NotFound();
            }
            return Ok(project);
        }

        [HttpDelete("delete-project/{projectId}")]
        public async Task<IActionResult> DeleteProject(int projectId)
        {
            await _projectService.DeleteProject(projectId);
            return NoContent();
        }
    }
}
