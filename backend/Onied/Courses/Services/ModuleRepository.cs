using Courses.Models;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace Courses.Services;

public class ModuleRepository(AppDbContext dbContext) : IModuleRepository
{
    public Task<Module?> GetModuleAsync(int id) =>
        dbContext.Modules
            .FirstOrDefaultAsync(x => x.Id == id);

    public async Task AddModuleAsync(Module module)
    {
        await dbContext.Modules.AddAsync(module);
        await dbContext.SaveChangesAsync();
    }

    public async Task<int> AddModuleReturnIdAsync(Module module)
    {
        await dbContext.Modules.AddAsync(module);
        await dbContext.SaveChangesAsync();
        return module.Id;
    }

    public async Task UpdateModuleAsync(Module module)
    {
        dbContext.Modules.Update(module);
        await dbContext.SaveChangesAsync();
    }
}
