using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

using TaskTracker.Repositories;
using TaskTracker.Data.Dto;

namespace TaskTracker.Controllers
{
    [EnableCors("MyPolicy")]
    [Route("api/[controller]")]
    public class ProjectsController : Controller
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectsController(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        [HttpGet(Name = "GetAllProjects")]
        public async Task<ActionResult<IEnumerable<ProjectDtoResponse>>> Get(string? queryAttribute, string? queryMethod, string? name, DateTime? date, int? priority)
        {
            return new ObjectResult(await _projectRepository.Get(queryAttribute, queryMethod, name, date, priority));
        }

        [HttpGet("{id}", Name = "GetProject")]
        public async Task<IActionResult> Get(int id)
        {
            ProjectDtoResponse project = await _projectRepository.Get(id);

            if (project == null)
            {
                return NotFound();
            }

            return new ObjectResult(project);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProjectDtoRequest project)
        {
            var validationResultList = new List<ValidationResult>();
            bool projectDtoRequestValidate = Validator.TryValidateObject(project, new ValidationContext(project), validationResultList);
            if (!projectDtoRequestValidate)
            {
                return BadRequest(new { errorText = "Invalid form fields." });
            }
            int createdProjectId = await _projectRepository.Create(project);
            return CreatedAtRoute("GetProject", new { id = createdProjectId }, project);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ProjectDtoUpdate updatedProject)
        {
            if (updatedProject == null)
            {
                return BadRequest(new { errorText = "Invalid form fields." });
            }

            int? updatedProjectId = await _projectRepository.Update(updatedProject);
            if (updatedProjectId != null)
            {
                return CreatedAtRoute("GetProject", new { id = updatedProjectId}, updatedProject);
            }
            return BadRequest(new { errorText = "No project with this id found." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deletedProject = await _projectRepository.Get(id);

            if (deletedProject == null)
            {
                return BadRequest(new { errorText = "No project with this id found." });
            }
            await _projectRepository.Delete(id);

            return Ok(new { Success = true });
        }

    }
}