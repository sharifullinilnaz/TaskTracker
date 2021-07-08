using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using TaskTracker.Data.Dto;

namespace TaskTracker.Repositories
{
    public interface IProjectRepository
    {
        Task<IEnumerable<ProjectDtoResponse>> Get(string? queryAttribute, string? queryMethod, string? name, DateTime? date, int? priority);

        Task<ProjectDtoResponse> Get(int id);

        Task<int> Create(ProjectDtoRequest projectDto);

        Task<int?> Update(ProjectDtoUpdate projectDto);

        Task Delete(int id);
    }
}
