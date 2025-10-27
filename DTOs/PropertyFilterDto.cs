namespace ASP.MongoDb.API.DTOs
{
  using ASP.MongoDb.API.Entities;

  public class PropertyFilterDto
  {
    public string? Name { get; set; }
    public string? Address { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? OwnerName { get; set; }
    public int? Year { get; set; }

    // Pagination
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;

    // Sorting
    public string? SortBy { get; set; } = "CreatedAt";
    public string? SortDirection { get; set; } = "desc"; // asc or desc
  }

  public class PropertySearchResultDto
  {
    public List<PropertyDto> Properties { get; set; } = new();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public bool HasNextPage { get; set; }
    public bool HasPreviousPage { get; set; }
  }

  public class PropertySearchResultRaw
  {
    public List<Property> Properties { get; set; } = new();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public bool HasNextPage { get; set; }
    public bool HasPreviousPage { get; set; }
  }
}