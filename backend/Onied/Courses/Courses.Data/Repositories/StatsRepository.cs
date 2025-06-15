using ClickHouse.Client.ADO;
using ClickHouse.Client.Utility;
using Courses.Data.Abstractions;

namespace Courses.Data.Repositories;

public class StatsRepository(ClickHouseConnection connection) : IStatsRepository
{
    private const string LikesTableName = "likes";

    public async Task CreateTablesIfNotExistsAsync()
    {
        var query = $"CREATE TABLE IF NOT EXISTS {LikesTableName} (userId UUID, courseId Int32) ENGINE = MergeTree()";
        await using var command = connection.CreateCommand();
        command.CommandText = query;
        await command.ExecuteNonQueryAsync();
    }

    public async Task<int> GetCourseLikesAsync(int courseId)
    {
        var query = $"SELECT count() from {LikesTableName} WHERE courseId = {courseId}";
        await using var command = connection.CreateCommand();
        command.CommandText = query;
        var result = await command.ExecuteScalarAsync();
        return result == null ? 0 : (int)result;
    }

    public async Task LikeCourseAsync(int courseId, Guid userId)
    {
        var query = $"INSERT INTO {LikesTableName} (courseId, userId) VALUES (@courseId, @userId)";
        await using var command = connection.CreateCommand();
        command.CommandText = query;
        command.AddParameter("courseId", courseId);
        command.AddParameter("userId", userId);
        await command.ExecuteNonQueryAsync();
    }

    public async Task UnlikeCourseAsync(int courseId, Guid userId)
    {
        var query = $"DELETE FROM {LikesTableName} WHERE courseId = @courseId AND userId = @userId";
        await using var command = connection.CreateCommand();
        command.CommandText = query;
        command.AddParameter("courseId", courseId);
        command.AddParameter("userId", userId);
        await command.ExecuteNonQueryAsync();
    }

    public async Task<bool> IsCourseLikedAsync(int courseId, Guid userId)
    {
        var query = $"SELECT count() from {LikesTableName} WHERE courseId = @courseId and userId = @userId";
        await using var command = connection.CreateCommand();
        command.CommandText = query;
        command.AddParameter("courseId", courseId);
        command.AddParameter("userId", userId);
        var result = await command.ExecuteScalarAsync();
        return result != null && ((int)result) > 0;
    }
}
