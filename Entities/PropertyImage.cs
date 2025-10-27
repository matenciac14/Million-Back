namespace ASP.MongoDb.API.Entities
{
  using MongoDB.Bson;
  using MongoDB.Bson.Serialization.Attributes;
  using System.ComponentModel.DataAnnotations;

  public class PropertyImage
  {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    [BsonElement("IdProperty")]
    [BsonRepresentation(BsonType.ObjectId)]
    [Required]
    public string IdProperty { get; set; } = string.Empty;

    [BsonElement("CloudinaryPublicId")]
    [Required]
    public string CloudinaryPublicId { get; set; } = string.Empty; // Cloudinary Public ID

    [BsonElement("CloudinaryUrl")]
    [Required]
    public string CloudinaryUrl { get; set; } = string.Empty; // Cloudinary Secure URL

    [BsonElement("OriginalFileName")]
    public string OriginalFileName { get; set; } = string.Empty;

    [BsonElement("Width")]
    public int Width { get; set; }

    [BsonElement("Height")]
    public int Height { get; set; }

    [BsonElement("Format")]
    public string Format { get; set; } = string.Empty;

    [BsonElement("Bytes")]
    public long Bytes { get; set; }

    [BsonElement("Enabled")]
    public bool Enabled { get; set; } = true;

    [BsonElement("IsMain")]
    public bool IsMain { get; set; } = false;

    [BsonElement("Description")]
    public string Description { get; set; } = string.Empty;

    [BsonElement("CreatedAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("UpdatedAt")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Computed properties for backward compatibility
    [BsonIgnore]
    public string Image => CloudinaryUrl; // For compatibility with existing code

    [BsonIgnore]
    public string ThumbnailUrl => GenerateImageUrl(150, 150);

    [BsonIgnore]
    public string MediumUrl => GenerateImageUrl(800, 600);

    [BsonIgnore]
    public string LargeUrl => GenerateImageUrl(1200, 900);

    // Helper method to generate different image sizes
    private string GenerateImageUrl(int? width = null, int? height = null)
    {
      if (string.IsNullOrEmpty(CloudinaryPublicId))
        return CloudinaryUrl;

      // Basic URL transformation (can be enhanced with Cloudinary service)
      var baseUrl = CloudinaryUrl.Split('/').Take(6).Aggregate((a, b) => $"{a}/{b}");
      var publicIdPart = CloudinaryUrl.Split('/').Last().Split('.')[0];

      if (width.HasValue && height.HasValue)
      {
        return $"{baseUrl}/w_{width},h_{height},c_fill,g_auto/{publicIdPart}.{Format}";
      }

      return CloudinaryUrl;
    }
  }
}