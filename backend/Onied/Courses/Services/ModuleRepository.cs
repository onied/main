﻿using Courses.Models;
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
        module.Index = await dbContext.Modules.Where(m => m.CourseId == module.CourseId).CountAsync();
        await dbContext.Modules.AddAsync(module);
        await dbContext.SaveChangesAsync();
    }

    public async Task<int> AddModuleReturnIdAsync(Module module)
    {
        module.Index = await dbContext.Modules.Where(m => m.CourseId == module.CourseId).CountAsync();
        await dbContext.Modules.AddAsync(module);
        await dbContext.SaveChangesAsync();
        return module.Id;
    }

    public async Task UpdateModuleAsync(Module module)
    {
        dbContext.Modules.Update(module);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteModuleAsync(int id)
    {
        var removedModule = await dbContext.Modules.FirstOrDefaultAsync(m => m.Id == id);
        if (removedModule != null)
        {
            dbContext.Modules.Remove(removedModule);
            var modules = dbContext.Modules
                .Where(m => m.CourseId == removedModule.CourseId && m.Id != id)
                .OrderBy(m => m.Index);
            var newIndex = 0;
            foreach (var module in modules)
            {
                module.Index = newIndex++;
            }
            await dbContext.SaveChangesAsync();
        }
    }
}
