using AutoMapper;
using Courses.Dtos;
using Courses.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Courses.Controllers;

[ApiController]
[Route("api/v1/[controller]/{id:guid}")]
public class AccountsController(IAccountsService accountsService) : ControllerBase
{
    [HttpGet]
    [Route("courses")]
    public async Task<IResult> GetCourses(Guid id)
    {
        return await accountsService.GetCourses(id);
    }
}
