namespace ASP.MongoDb.API.DTOs
{
  using System.ComponentModel.DataAnnotations;

  public class OwnerDto
  {
    public string Id { get; set; } = string.Empty;

    [Required]
    public string Name { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public DateTime? Birthday { get; set; }

    public string Email { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
  }

  public class OwnerCreateDto
  {
    [Required]
    public string Name { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public DateTime? Birthday { get; set; }

    [EmailAddress]
    public string Email { get; set; } = string.Empty;
  }

  public class OwnerUpdateDto
  {
    public string? Name { get; set; }
    public string? LastName { get; set; }
    public string? Phone { get; set; }
    public DateTime? Birthday { get; set; }

    [EmailAddress]
    public string? Email { get; set; }
  }
}