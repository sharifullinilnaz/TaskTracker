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
    public class TasksController : Controller
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IProjectRepository _projectRepository;

        public TasksController(ITaskRepository taskRepository, IProjectRepository projectRepository)
        {
            _taskRepository = taskRepository;
            _projectRepository = projectRepository;
        }

        [HttpGet(Name = "GetAllTasks")]
        public async Task<ActionResult<IEnumerable<TaskDtoResponse>>> Get()
        {
            return new ObjectResult(await _taskRepository.Get());
        }

        [HttpGet("{Id}", Name = "GetTask")]
        public async Task<IActionResult> Get(int Id)
        {
            TaskDtoResponse task = await _taskRepository.Get(Id);

            if (task == null)
            {
                return NotFound();
            }

            return new ObjectResult(task);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TaskDtoRequest task)
        {
            var validationResultList = new List<ValidationResult>();
            bool taskDtoRequestValidate = Validator.TryValidateObject(task, new ValidationContext(task), validationResultList);
            if (!taskDtoRequestValidate)
            {
                return BadRequest(new { errorText = "Invalid form fields." });
            }
            var project = await _projectRepository.Get(task.ProjectId);
            if (project == null)
            {
                return BadRequest(new { errorText = "No project with this id found." });
            }
            int createdTaskId = await _taskRepository.Create(task);
            return CreatedAtRoute("GetTask", new { id = createdTaskId }, task);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] TaskDtoUpdate updatedTask)
        {
            if (updatedTask == null)
            {
                return BadRequest(new { errorText = "Invalid form fields." });
            }

            int? updatedTaskId =  await _taskRepository.Update(updatedTask);
            if (updatedTaskId != null)
            {
                return CreatedAtRoute("GetProject", new { id = updatedTaskId}, updatedTask);
            }
            return BadRequest(new { errorText = "No task with this id found." });
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(int Id)
        {
            var deletedTask = await _taskRepository.Get(Id);

            if (deletedTask == null)
            {
                return BadRequest(new { errorText = "No task with this id found." });
            }
            await _taskRepository.Delete(Id);

            return Ok(new { Success = true });
        }

    }
}