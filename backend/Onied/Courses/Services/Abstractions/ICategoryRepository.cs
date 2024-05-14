using Courses.Models;

namespace Courses.Services.Abstractions;

public interface ICategoryRepository
{
    public Task<List<Category>> GetAllCategoriesAsync();
    public Task<Category?> GetCategoryById(int id);
}
