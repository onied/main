namespace Courses.Helpers;

public class Page<TEntity>
{
    public int CurrentPage { get; set; }
    public int PagesCount { get; set; }
    public int ElementsPerPage { get; set; }
    public IEnumerable<TEntity> Elements { get; set; } = null!;

    public static Page<TEntity> Prepare(PageQuery pageQuery, int elementsCount, out int offset)
    {
        var pagesCount = (elementsCount + pageQuery.ElementsOnPage - 1) / pageQuery.ElementsOnPage;
        var currentPage = Math.Max(1, Math.Min(pagesCount, pageQuery.Page));
        offset = (currentPage - 1) * pageQuery.ElementsOnPage;
        return new Page<TEntity>
        {
            CurrentPage = currentPage,
            ElementsPerPage = pageQuery.ElementsOnPage,
            PagesCount = pagesCount
        };
    }
}