namespace ASP.MongoDb.API.Entities
{
  using MongoDB.Bson;
  using MongoDB.Bson.Serialization.Attributes;
  using System.ComponentModel.DataAnnotations;

  public class PropertyTrace
  {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    [BsonElement("IdProperty")]
    [BsonRepresentation(BsonType.ObjectId)]
    [Required]
    public string IdProperty { get; set; } = string.Empty;

    [BsonElement("DateSale")]
    [Required]
    public DateTime DateSale { get; set; }

    [BsonElement("Name")]
    [Required]
    public string Name { get; set; } = string.Empty;

    [BsonElement("Value")]
    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "Value must be greater than 0")]
    public decimal Value { get; set; }

    [BsonElement("Tax")]
    [Range(0, double.MaxValue, ErrorMessage = "Tax must be greater than 0")]
    public decimal Tax { get; set; }

    [BsonElement("CreatedAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("UpdatedAt")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
  }
}