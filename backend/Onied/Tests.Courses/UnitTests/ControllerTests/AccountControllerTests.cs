using Courses;
using Courses.Controllers;
using Microsoft.Extensions.Logging;
using Moq;

namespace Tests.Courses.UnitTests.ControllerTests;

public class AccountControllerTests
{
    private readonly AppDbContext _context;
    private readonly Mock<ILogger<CatalogController>> _logger = new();
}
