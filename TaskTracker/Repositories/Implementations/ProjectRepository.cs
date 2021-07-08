using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

using TaskTracker.Data.Models;
using TaskTracker.Data.Dto;
using TaskTracker.Data;
using TaskTracker.Enums;
using TaskModel = TaskTracker.Data.Models.Task;

namespace TaskTracker.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly TaskTrackerDbContext _taskTrackerDbContext;

        public ProjectRepository(TaskTrackerDbContext taskTrackerDbContext)
        {
            _taskTrackerDbContext = taskTrackerDbContext;
        }

        public async Task<IEnumerable<ProjectDtoResponse>> Get(string? queryAttribute, string? queryMethod, string? name, DateTime? date, int? priority)
        {
            var projects = await Filter(queryAttribute, queryMethod, name, date, priority);
            
            foreach (var project in projects)
            {
                project.Tasks = _taskTrackerDbContext.Projects.Find(project.Id).Tasks.Select(t => AplyTaskEntityToDto(t));
            }
            return projects;
        }

        public async Task<ProjectDtoResponse> Get(int Id)
        {
            var project = await _taskTrackerDbContext.Projects.FindAsync(Id);
            if (project!= null)
            {
                ProjectDtoResponse projectDto = AplyProjectEntityToDto(project);
                projectDto.Tasks = project.Tasks.Select(t => AplyTaskEntityToDto(t));
                return projectDto;
            }
            return null;
        }

        public async Task<int> Create(ProjectDtoRequest projectDtoRequest)
        {
            Project project = new();
            AplyDtoRequestToEntity(project, projectDtoRequest);
            project.Status = ProjectStatus.NotStarted;
            project.StartDate = DateTime.Now.Date;
            _taskTrackerDbContext.Projects.Add(project);
            await _taskTrackerDbContext.SaveChangesAsync();
            return project.Id;
        }
        public async Task<int?> Update(ProjectDtoUpdate updatedProject)
        {
            Project projectToUpdate = _taskTrackerDbContext.Projects.Find(updatedProject.Id);
            if (projectToUpdate != null)
            {
                AplyDtoUpdateToEntity(projectToUpdate, updatedProject);

                _taskTrackerDbContext.Projects.Update(projectToUpdate);
                await _taskTrackerDbContext.SaveChangesAsync();
                return projectToUpdate.Id;
            }
            return null;

        }

        public async Task Delete(int Id)
        {
            Project project = await _taskTrackerDbContext.Projects.FindAsync(Id);

            if (project != null)
            {
                _taskTrackerDbContext.Projects.Remove(project);
                await _taskTrackerDbContext.SaveChangesAsync();
            }
        }

        private static void AplyDtoRequestToEntity(Project project, ProjectDtoRequest projectDto)
        {
            project.Name = projectDto.Name;
            project.Priority = projectDto.Priority;
        }

        private static void AplyDtoUpdateToEntity(Project project, ProjectDtoUpdate projectDto)
        {
            project.Name = projectDto.Name;
            project.Priority = projectDto.Priority;
            project.Status = projectDto.Status;
            if (project.Status == (ProjectStatus)2)
            {
                project.CompletionDate = DateTime.Now.Date;
            } else
            {
                project.CompletionDate = null;
            }        
        }

        private static ProjectDtoResponse AplyProjectEntityToDto(Project project)
        {
            ProjectDtoResponse projectDto = new();
            projectDto.Id = project.Id;
            projectDto.Name = project.Name;
            projectDto.StartDate = project.StartDate;
            projectDto.CompletionDate = project.CompletionDate;
            projectDto.Status = project.Status.ToString();
            projectDto.Priority = project.Priority;
            return projectDto;
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

        /* Filter method: 
            queryAttribute - by which attribute the filter is performed;
            queryMethod - filtering method (for name: "exactValue" - full match search; 
                                                      default - template entry search;
                                            for date: "startAt" - search for dates later than {date}; 
                                                      "endAt" - search for dates earlier than {date};
                                                      default - search for same date;
                                            for priority: "higher" - search for priorities higher than {priority}; 
                                                          "lower" - search for priorities lower than {priority};
                                                          default - search for same priority;)
        */
        public async Task<IEnumerable<ProjectDtoResponse>> Filter(string? queryAttribute, string? queryMethod, string? name, DateTime? date, int? priority)
        {
            if (date.HasValue)
            {
                date = date.Value.Date;
            }

            IEnumerable<ProjectDtoResponse> projects = queryAttribute switch
            {
                "name" => queryMethod switch
                {
                    "exactValue" => await _taskTrackerDbContext.Projects.Where(p => p.Name == name).Select(p => AplyProjectEntityToDto(p)).ToListAsync(),
                    _ => await _taskTrackerDbContext.Projects.Where(p => EF.Functions.Like(p.Name, $"%{name}%")).Select(p => AplyProjectEntityToDto(p)).ToListAsync(),
                },
                "date" => queryMethod switch
                {
                    "startAt" => await _taskTrackerDbContext.Projects.Where(p => p.StartDate > date).Select(p => AplyProjectEntityToDto(p)).ToListAsync(),
                    "endAt" => await _taskTrackerDbContext.Projects.Where(p => p.StartDate < date).Select(p => AplyProjectEntityToDto(p)).ToListAsync(),
                    _ => await _taskTrackerDbContext.Projects.Where(p => p.StartDate == date).Select(p => AplyProjectEntityToDto(p)).ToListAsync(),
                },
                "priority" => queryMethod switch
                {
                    "higher" => await _taskTrackerDbContext.Projects.Where(p => p.Priority > priority).Select(p => AplyProjectEntityToDto(p)).ToListAsync(),
                    "lower" => await _taskTrackerDbContext.Projects.Where(p => p.Priority < priority).Select(p => AplyProjectEntityToDto(p)).ToListAsync(),
                    _ => await _taskTrackerDbContext.Projects.Where(p => p.Priority == priority).Select(p => AplyProjectEntityToDto(p)).ToListAsync(),
                },
                _ => await _taskTrackerDbContext.Projects.Select(p => AplyProjectEntityToDto(p)).ToListAsync(),
            };
            return projects;
        }

    }
}
