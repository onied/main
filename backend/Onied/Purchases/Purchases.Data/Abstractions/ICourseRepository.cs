using Purchases.Data.Models;

namespace Purchases.Data.Abstractions;

public interface ICourseRepository
{
    public Task<Course?> GetAsync(int id);
    public Task AddAsync(Course course);
    public Task UpdateAsync(Course course);
    public Task RemoveAsync(Course course);
}
