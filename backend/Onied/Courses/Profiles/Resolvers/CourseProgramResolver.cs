using AutoMapper;
using Courses.Dtos;
using Courses.Models;

namespace Courses.Profiles.Resolvers;

public class CourseProgramResolver : IValueResolver<Course, PreviewDto, List<string>?>
{
    public List<string>? Resolve(Course source, PreviewDto destination, List<string>? destMember,
        ResolutionContext context)
    {
        return source.IsProgramVisible
            ? source.Modules.OrderBy(module => module.Id).Select(module => module.Title).ToList()
            : null;
    }
}