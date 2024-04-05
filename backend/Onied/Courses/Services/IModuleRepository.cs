using Courses.Models;
using Task = System.Threading.Tasks.Task;

namespace Courses.Services;

public interface IModuleRepository
{
    public Task<Module?> GetModuleAsync(int id);
    public Task AddModuleAsync(Module module);
    public Task<int> AddModuleReturnIdAsync(Module module);
    public Task UpdateModuleAsync(Module module);
    public Task<bool> RenameModuleAsync(int id, string title);
    public Task<bool> DeleteModuleAsync(int id);
}
