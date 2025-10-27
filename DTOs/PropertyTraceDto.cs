namespace ASP.MongoDb.API.DTOs
{
  using System.ComponentModel.DataAnnotations;

  /// <summary>
  /// DTO for PropertyTrace responses (complete with all fields)
  /// </summary>
  public class PropertyTraceResponseDto
  {
    public string Id { get; set; } = string.Empty;
    public string IdProperty { get; set; } = string.Empty;
    public string DateSale { get; set; } = string.Empty; // Formatted as yyyy-MM-dd
    public string Name { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public decimal Tax { get; set; }
    public DateTime CreatedAt { get; set; }
  }

  /// <summary>
  /// DTO for creating PropertyTrace
  /// </summary>
  public class PropertyTraceCreateDto
  {
    [Required]
    public string IdProperty { get; set; } = string.Empty;

    [Required]
    public DateTime DateSale { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 100 characters")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "Value must be greater than 0")]
    public decimal Value { get; set; }

    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "Tax must be greater than or equal to 0")]
    public decimal Tax { get; set; }
  }

  /// <summary>
  /// DTO for updating PropertyTrace
  /// </summary>
  public class PropertyTraceUpdateDto
  {
    public DateTime? DateSale { get; set; }

    [StringLength(100, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 100 characters")]
    public string? Name { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Value must be greater than 0")]
    public decimal? Value { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Tax must be greater than or equal to 0")]
    public decimal? Tax { get; set; }
  }
}