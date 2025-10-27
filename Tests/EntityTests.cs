using NUnit.Framework;
using ASP.MongoDb.API.DTOs;
using ASP.MongoDb.API.Entities;

namespace ASP.MongoDb.API.Tests
{
  [TestFixture]
  public class PropertyEntityTests
  {
    [Test]
    public void Property_Creation_ShouldSetDefaultValues()
    {
      // Arrange & Act
      var property = new Property();

      // Assert
      Assert.That(property.Id, Is.EqualTo(string.Empty));
      Assert.That(property.Name, Is.EqualTo(string.Empty));
      Assert.That(property.Address, Is.EqualTo(string.Empty));
      Assert.That(property.Price, Is.EqualTo(0));
      Assert.That(property.CodigoInternal, Is.EqualTo(string.Empty));
      Assert.That(property.IdOwner, Is.EqualTo(string.Empty));
      Assert.That(property.CreatedAt, Is.Not.EqualTo(default(DateTime)));
      Assert.That(property.UpdatedAt, Is.Not.EqualTo(default(DateTime)));
      Assert.That(property.Images, Is.Not.Null);
      Assert.That(property.Places, Is.Not.Null);
    }

    [Test]
    public void Property_WithValidData_ShouldSetProperties()
    {
      // Arrange
      var name = "Beautiful House";
      var address = "123 Main St";
      var price = 250000m;
      var ownerId = "64a1b2c3d4e5f6789012345";
      var codigo = "PROP001";
      var year = 2020;

      // Act
      var property = new Property
      {
        Name = name,
        Address = address,
        Price = price,
        IdOwner = ownerId,
        CodigoInternal = codigo,
        Year = year
      };

      // Assert
      Assert.That(property.Name, Is.EqualTo(name));
      Assert.That(property.Address, Is.EqualTo(address));
      Assert.That(property.Price, Is.EqualTo(price));
      Assert.That(property.IdOwner, Is.EqualTo(ownerId));
      Assert.That(property.CodigoInternal, Is.EqualTo(codigo));
      Assert.That(property.Year, Is.EqualTo(year));
    }
  }

  [TestFixture]
  public class OwnerEntityTests
  {
    [Test]
    public void Owner_FullName_ShouldCombineFirstAndLastName()
    {
      // Arrange
      var owner = new Owner
      {
        Name = "John",
        LastName = "Doe"
      };

      // Act
      var fullName = owner.FullName;

      // Assert
      Assert.That(fullName, Is.EqualTo("John Doe"));
    }

    [Test]
    public void Owner_FullName_WithOnlyFirstName_ShouldReturnFirstName()
    {
      // Arrange
      var owner = new Owner
      {
        Name = "John",
        LastName = ""
      };

      // Act
      var fullName = owner.FullName;

      // Assert
      Assert.That(fullName, Is.EqualTo("John"));
    }

    [Test]
    public void Owner_FullName_WithEmptyNames_ShouldReturnEmpty()
    {
      // Arrange
      var owner = new Owner
      {
        Name = "",
        LastName = ""
      };

      // Act
      var fullName = owner.FullName;

      // Assert
      Assert.That(fullName, Is.EqualTo(""));
    }
  }
}