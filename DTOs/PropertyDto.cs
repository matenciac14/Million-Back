namespace ASP.MongoDb.API.DTOs
{
  using System.ComponentModel.DataAnnotations;

  public class PropertyDto
  {
    public string Id { get; set; } = string.Empty;

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Address { get; set; } = string.Empty;

    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
    public decimal Price { get; set; }

    [Required]
    public string IdOwner { get; set; } = string.Empty;

    public string? Image { get; set; } // Just one image as required

    public string CodigoInternal { get; set; } = string.Empty;

    public int? Year { get; set; }

    public DateTime CreatedAt { get; set; }

    // Owner information
    public string OwnerName { get; set; } = string.Empty;
    public string OwnerPhone { get; set; } = string.Empty;

    // Location information
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
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