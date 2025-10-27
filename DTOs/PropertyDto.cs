namespace ASP.MongoDb.API.DTOs
{
  using System.ComponentModel.DataAnnotations;

  // DTO for frontend response (matches frontend expectations)
  public class PropertyDto
  {
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public List<PropertyImageDto> Images { get; set; } = new();
    public PropertyOwnerDto Owner { get; set; } = new();
    public List<PropertyTraceDto> Traces { get; set; } = new();
    public string CodigoInternal { get; set; } = string.Empty;
    public int? Year { get; set; }
    public DateTime CreatedAt { get; set; }

    // Location information
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
  }

  // Image DTO for frontend
  public class PropertyImageDto
  {
    public string IdPropertyImage { get; set; } = string.Empty;
    public string File { get; set; } = string.Empty;
    public bool Enabled { get; set; } = true;
    public bool IsMain { get; set; } = false;
    public string Description { get; set; } = string.Empty;
  }

  // Owner DTO for frontend
  public class PropertyOwnerDto
  {
    public string Name { get; set; } = string.Empty;
    public string Photo { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
  }

  // Trace DTO for frontend
  public class PropertyTraceDto
  {
    public string DateSale { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public decimal Tax { get; set; }
  }

  public class PropertyCreateDto
  {
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Address { get; set; } = string.Empty;

    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
    public decimal Price { get; set; }

    [Required]
    public string IdOwner { get; set; } = string.Empty;

    public string CodigoInternal { get; set; } = string.Empty;

    public int? Year { get; set; }

    public string? Image { get; set; }

    // Location data
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
  }

  public class PropertyUpdateDto
  {
    public string? Name { get; set; }
    public string? Address { get; set; }
    public decimal? Price { get; set; }
    public string? IdOwner { get; set; }
    public string? CodigoInternal { get; set; }
    public int? Year { get; set; }
    public string? Image { get; set; }

    // Location data
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
  }
}