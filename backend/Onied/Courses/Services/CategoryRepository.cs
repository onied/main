using Courses.Models;
using Courses.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Courses.Services;

public class CategoryRepository(AppDbContext context) : ICategoryRepository
{
    public Task<List<Category>> GetAllCategoriesAsync()
    {
        return context.Categories.AsNoTracking().ToListAsync();
    }

    public Task<Category?> GetCategoryById(int id)
    {
        return context.Categories.AsNoTracking().SingleOrDefaultAsync(category => category.Id == id);
    }
}
