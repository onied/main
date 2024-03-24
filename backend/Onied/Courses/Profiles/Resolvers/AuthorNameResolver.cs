using AutoMapper;
using Courses.Dtos;
using Courses.Models;
using Courses.Models.Users;

namespace Courses.Profiles.Resolvers;

public class AuthorNameResolver : IValueResolver<Author, AuthorDto, string>
{
    public string Resolve(Author source, AuthorDto destination, string destMember, ResolutionContext context)
    {
        return $"{source.FirstName} {source.LastName}";
    }
}
