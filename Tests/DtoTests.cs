using NUnit.Framework;
using ASP.MongoDb.API.DTOs;
using System.ComponentModel.DataAnnotations;

namespace ASP.MongoDb.API.Tests
{
  [TestFixture]
  public class PropertyDtoTests
  {
    [Test]
    public void PropertyCreateDto_WithValidData_ShouldPassValidation()
    {
      // Arrange
      var dto = new PropertyCreateDto
      {
        Name = "Beautiful House",
        Address = "123 Main St",
        Price = 250000m,
        IdOwner = "64a1b2c3d4e5f6789012345"
      };

      // Act
      var validationResults = ValidateModel(dto);

      // Assert
      Assert.That(validationResults, Is.Empty);
    }

    [Test]
    public void PropertyCreateDto_WithNegativePrice_ShouldFailValidation()
    {
      // Arrange
      var dto = new PropertyCreateDto
      {
        Name = "Beautiful House",
        Address = "123 Main St",
        Price = -1000m,
        IdOwner = "64a1b2c3d4e5f6789012345"
      };

      // Act
      var validationResults = ValidateModel(dto);

      // Assert
      Assert.That(validationResults, Is.Not.Empty);
      Assert.That(validationResults.Any(v => v.MemberNames.Contains("Price")), Is.True);
    }

    [Test]
    public void PropertyCreateDto_WithEmptyRequiredFields_ShouldFailValidation()
    {
      // Arrange
      var dto = new PropertyCreateDto
      {
        Name = "",
        Address = "",
        Price = 250000m,
        IdOwner = ""
      };

      // Act
      var validationResults = ValidateModel(dto);

      // Assert
      Assert.That(validationResults, Is.Not.Empty);
      Assert.That(validationResults.Count, Is.EqualTo(3)); // Name, Address, IdOwner
    }

    [Test]
    public void PropertyFilterDto_DefaultValues_ShouldSetCorrectDefaults()
    {
      // Arrange & Act
      var filter = new PropertyFilterDto();

      // Assert
      Assert.That(filter.Page, Is.EqualTo(1));
      Assert.That(filter.PageSize, Is.EqualTo(10));
      Assert.That(filter.SortBy, Is.EqualTo("CreatedAt"));
      Assert.That(filter.SortDirection, Is.EqualTo("desc"));
    }

    [Test]
    public void PropertySearchResultDto_InitialState_ShouldHaveEmptyProperties()
    {
      // Arrange & Act
      var result = new PropertySearchResultDto();

      // Assert
      Assert.That(result.Properties, Is.Not.Null);
      Assert.That(result.Properties, Is.Empty);
      Assert.That(result.TotalCount, Is.EqualTo(0));
      Assert.That(result.Page, Is.EqualTo(0));
      Assert.That(result.PageSize, Is.EqualTo(0));
      Assert.That(result.TotalPages, Is.EqualTo(0));
      Assert.That(result.HasNextPage, Is.False);
      Assert.That(result.HasPreviousPage, Is.False);
    }

    [Test]
    public void OwnerCreateDto_WithValidEmail_ShouldPassValidation()
    {
      // Arrange
      var dto = new OwnerCreateDto
      {
        Name = "John",
        Email = "john@example.com"
      };

      // Act
      var validationResults = ValidateModel(dto);

      // Assert
      Assert.That(validationResults, Is.Empty);
    }

    [Test]
    public void OwnerCreateDto_WithInvalidEmail_ShouldFailValidation()
    {
      // Arrange
      var dto = new OwnerCreateDto
      {
        Name = "John",
        Email = "invalid-email"
      };

      // Act
      var validationResults = ValidateModel(dto);

      // Assert
      Assert.That(validationResults, Is.Not.Empty);
      Assert.That(validationResults.Any(v => v.MemberNames.Contains("Email")), Is.True);
    }

    private List<ValidationResult> ValidateModel(object model)
    {
      var validationResults = new List<ValidationResult>();
      var context = new ValidationContext(model, null, null);
      Validator.TryValidateObject(model, context, validationResults, true);
      return validationResults;
    }
  }
}