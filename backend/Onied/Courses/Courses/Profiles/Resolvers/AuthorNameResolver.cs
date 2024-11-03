using AutoMapper;
using Courses.Data.Models;
using Courses.Dtos.Catalog.Response;

namespace Courses.Profiles.Resolvers;

public class AuthorNameResolver : IValueResolver<User, AuthorResponse, string>
{
    public string Resolve(User source, AuthorResponse destination, string destMember, ResolutionContext context)
    {
        return $"{source.FirstName} {source.LastName}";
    }
}
