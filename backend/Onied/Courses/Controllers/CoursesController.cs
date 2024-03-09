using Microsoft.AspNetCore.Mvc;

namespace Courses.Controllers;

[ApiController]
[Route("[controller]")]
public class CoursesController : ControllerBase
{

    private readonly ILogger<CoursesController> _logger;

    public CoursesController(ILogger<CoursesController> logger)
    {
        _logger = logger;
    }
    
    [HttpGet]
    public string Get()
    {
        return "Under construction...";
    }
}