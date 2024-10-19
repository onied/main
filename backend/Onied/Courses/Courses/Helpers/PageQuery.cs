namespace Courses.Helpers;

public class PageQuery
{
    private int _elementsOnPage = 20;

    public int ElementsOnPage
    {
        get => _elementsOnPage;
        set => _elementsOnPage = Math.Max(1, Math.Min(250, value));
    }

    public int Page { get; set; } = 1;

    public int Offset => (Page - 1) * ElementsOnPage;
}
