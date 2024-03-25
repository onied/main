using AutoMapper;
using Courses.Dtos;
using Courses.Models;

namespace Courses.Profiles.Resolvers;

public class AuthorNameResolver : IValueResolver<User, AuthorDto, string>
{
    public string Resolve(User source, AuthorDto destination, string destMember, ResolutionContext context)
    {
        return $"{source.FirstName} {source.LastName}";
    }
}
