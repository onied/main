using AutoMapper;
using Courses.Data.Models;
using Courses.Dtos.Course.Response;

namespace Courses.Profiles.Resolvers;

public class CourseProgramResolver : IValueResolver<Course, PreviewResponse, List<string>?>
{
    public List<string>? Resolve(Course source, PreviewResponse destination, List<string>? destMember,
        ResolutionContext context)
    {
        return source.IsProgramVisible
            ? source.Modules.OrderBy(module => module.Id).Select(module => module.Title).ToList()
            : null;
    }
}
