namespace ASP.MongoDb.API.Entities
{
  using MongoDB.Bson;
  using MongoDB.Bson.Serialization.Attributes;
  using System.ComponentModel.DataAnnotations;

  public class Property
  {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    [BsonElement("Name")]
    [Required]
    public string Name { get; set; } = string.Empty;

    [BsonElement("Address")]
    [Required]
    public string Address { get; set; } = string.Empty;

    [BsonElement("Price")]
    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
    public decimal Price { get; set; }

    [BsonElement("CodigoInternal")]
    public string CodigoInternal { get; set; } = string.Empty;

    [BsonElement("IdOwner")]
    [BsonRepresentation(BsonType.ObjectId)]
    [Required]
    public string IdOwner { get; set; } = string.Empty;

    [BsonElement("Year")]
    public int? Year { get; set; }

    [BsonElement("CreatedAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("UpdatedAt")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties (not stored in DB, used for joins)
    [BsonIgnore]
    public Owner? Owner { get; set; }

    [BsonIgnore]
    public List<PropertyImage> Images { get; set; } = new();

    [BsonIgnore]
    public List<PropertyPlace> Places { get; set; } = new();
  }
}