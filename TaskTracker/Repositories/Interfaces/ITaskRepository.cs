using System.Collections.Generic;
using System.Threading.Tasks;

using TaskTracker.Data.Dto;

namespace TaskTracker.Repositories
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskDtoResponse>> Get();

        Task<TaskDtoResponse> Get(int id);

        Task<int> Create(TaskDtoRequest taskDto);

        Task<int?> Update(TaskDtoUpdate taskDto);

        Task Delete(int id);

    }
}
