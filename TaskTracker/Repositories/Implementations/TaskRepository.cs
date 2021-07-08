using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

using TaskTracker.Data.Dto;
using TaskTracker.Data;
using TaskModel = TaskTracker.Data.Models.Task;

namespace TaskTracker.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskTrackerDbContext _taskTrackerDbContext;

        public TaskRepository(TaskTrackerDbContext taskTrackerDbContext)
        {
            _taskTrackerDbContext = taskTrackerDbContext;
        }

        public async Task<IEnumerable<TaskDtoResponse>> Get()
        {
            var tasks = await _taskTrackerDbContext.Tasks.Select(t => AplyTaskEntityToDto(t)).ToListAsync();
            return tasks;
        }

        public async Task<TaskDtoResponse> Get(int Id)
        {
            var task = await _taskTrackerDbContext.Tasks.FindAsync(Id);
            if (task != null)
            {
                TaskDtoResponse taskDto = AplyTaskEntityToDto(task);

                return taskDto;
            }

            return null;
        }

        public async Task<int> Create(TaskDtoRequest taskDtoRequest)
        {
            TaskModel task = new();
            AplyDtoRequestToEntity(task, taskDtoRequest);
            task.Status = Enums.TaskStatus.ToDo;
            _taskTrackerDbContext.Tasks.Add(task);
            await _taskTrackerDbContext.SaveChangesAsync();
            return task.Id;
        }
        public async Task<int?> Update(TaskDtoUpdate updatedTask)
        {
            TaskModel taskToUpdate = await _taskTrackerDbContext.Tasks.FindAsync(updatedTask.Id);
            if (taskToUpdate != null)
            {
                AplyDtoUpdateToEntity(taskToUpdate, updatedTask);

                _taskTrackerDbContext.Tasks.Update(taskToUpdate);
                await _taskTrackerDbContext.SaveChangesAsync();
                return taskToUpdate.Id;
            }
            return null;
        }

        public async Task Delete(int Id)
        {
            TaskModel task = await _taskTrackerDbContext.Tasks.FindAsync(Id);

            if (task != null)
            {
                _taskTrackerDbContext.Tasks.Remove(task);
                await _taskTrackerDbContext.SaveChangesAsync();
            }
        }

        private static void AplyDtoRequestToEntity(TaskModel task, TaskDtoRequest taskDto)
        {
            task.Name = taskDto.Name;
            task.Description = taskDto.Description;
            task.Priority = taskDto.Priority;
            task.ProjectId = taskDto.ProjectId;
        }

        private static void AplyDtoUpdateToEntity(TaskModel task, TaskDtoUpdate taskDto)
        {
            task.Name = taskDto.Name;
            task.Priority = taskDto.Priority;
            task.Description = taskDto.Description;
            task.Status = taskDto.Status;
        }

        private static TaskDtoResponse AplyTaskEntityToDto(TaskModel task)
        {
            TaskDtoResponse taskDto = new();
            taskDto.Id = task.Id;
            taskDto.Name = task.Name;
            taskDto.Description = task.Description;
            taskDto.Status = task.Status.ToString();
            taskDto.Priority = task.Priority;
            taskDto.ProjectId = task.ProjectId;
            return taskDto;
        }
    }
}
