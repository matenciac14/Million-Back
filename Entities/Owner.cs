namespace ASP.MongoDb.API.Entities
{
  using MongoDB.Bson;
  using MongoDB.Bson.Serialization.Attributes;
  using System.ComponentModel.DataAnnotations;

  public class Owner
  {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    [BsonElement("Name")]
    [Required]
    public string Name { get; set; } = string.Empty;

    [BsonElement("LastName")]
    public string LastName { get; set; } = string.Empty;

    [BsonElement("Phone")]
    public string Phone { get; set; } = string.Empty;

    [BsonElement("Photo")]
    public string Photo { get; set; } = string.Empty;

    [BsonElement("Birthday")]
    public DateTime? Birthday { get; set; }

    [BsonElement("Email")]
    public string Email { get; set; } = string.Empty;

    [BsonElement("CreatedAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("UpdatedAt")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Full name property for convenience
    [BsonIgnore]
    public string FullName => $"{Name} {LastName}".Trim();
  }
}