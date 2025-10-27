namespace ASP.MongoDb.API.Entities
{
  using MongoDB.Bson;
  using MongoDB.Bson.Serialization.Attributes;
  using System.ComponentModel.DataAnnotations;

  public class PropertyPlace
  {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    [BsonElement("IdProperty")]
    [BsonRepresentation(BsonType.ObjectId)]
    [Required]
    public string IdProperty { get; set; } = string.Empty;

    [BsonElement("Name")]
    [Required]
    public string Name { get; set; } = string.Empty; // e.g., "City", "State", "Country", "Neighborhood"

    [BsonElement("Value")]
    [Required]
    public string Value { get; set; } = string.Empty; // e.g., "Bogotá", "Cundinamarca", "Colombia"

    [BsonElement("PlaceType")]
    public string PlaceType { get; set; } = string.Empty; // e.g., "city", "state", "country"

    [BsonElement("Latitude")]
    public double? Latitude { get; set; }

    [BsonElement("Longitude")]
    public double? Longitude { get; set; }

    [BsonElement("CreatedAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("UpdatedAt")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
  }
}