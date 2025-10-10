namespace Tixxp.Core.Utilities.Results.Concrete;

public class CompareDataResult
{
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
    public List<ColumnChange>? ChangedColumns { get; set; } = new();
}
