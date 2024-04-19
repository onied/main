using Courses.Helpers;

namespace Courses.Dtos;

public class CatalogGetQueriesDto : PageQuery
{
    /// <summary>
    ///     Search query.
    /// </summary>
    public string Q { get; set; } = "";

    public string? Sort { get; set; }

    public int? CategoryId { get; set; }

    public int? PriceFrom { get; set; }
    public int? PriceTo { get; set; }

    public int? TimeFrom { get; set; }
    public int? TimeTo { get; set; }

    public bool CertificatesOnly { get; set; } = false;
    public bool ActiveOnly { get; set; } = false;
}
