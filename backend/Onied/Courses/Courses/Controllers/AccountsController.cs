using Courses.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Courses.Controllers;

[ApiController]
[Route("api/v1/[controller]/{id:guid}")]
public class AccountsController(ISender sender) : ControllerBase
{
    [HttpGet]
    [Route("courses")]
    public async Task<IResult> GetCourses(Guid id)
    {
        return await sender.Send(new GetCoursesQuery(id));
    }
}
