namespace ASP.MongoDb.API.Controllers
{
  using ASP.MongoDb.API.Entities;
  using ASP.MongoDb.API.Repository;
  using ASP.MongoDb.API.DTOs;
  using Microsoft.AspNetCore.Mvc;

  [ApiController]
  [Route("api/[controller]")]
  public class PropertyController : ControllerBase
  {
    private readonly IPropertyRepository _propertyRepository;
    private readonly IOwnerRepository _ownerRepository;

    public PropertyController(IPropertyRepository propertyRepository, IOwnerRepository ownerRepository)
    {
      _propertyRepository = propertyRepository;
      _ownerRepository = ownerRepository;
    }

    /// <summary>
    /// Get all properties with optional filters
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<PropertySearchResultDto>> GetProperties([FromQuery] PropertyFilterDto filter)
    {
      try
      {
        var result = await _propertyRepository.SearchPropertiesAsync(filter);
        return Ok(result);
      }
      catch (Exception ex)
      {
        return StatusCode(500, new { message = "Error retrieving properties", error = ex.Message });
      }
    }

    /// <summary>
    /// Get property by ID with all details
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<PropertyDto>> GetPropertyById(string id)
    {
      try
      {
        var property = await _propertyRepository.GetPropertyWithDetailsAsync(id);
        if (property == null)
        {
          return NotFound(new { message = "Property not found" });
        }

        var propertyDto = ConvertToDto(property);
        return Ok(propertyDto);
      }
      catch (Exception ex)
      {
        return StatusCode(500, new { message = "Error retrieving property", error = ex.Message });
      }
    }

    /// <summary>
    /// Create new property
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<PropertyDto>> CreateProperty([FromBody] PropertyCreateDto createDto)
    {
      try
      {
        // Validate owner exists
        var owner = await _ownerRepository.GetByIdAsync(createDto.IdOwner);
        if (owner == null)
        {
          return BadRequest(new { message = "Owner not found" });
        }

        // Check if codigo interno already exists
        if (!string.IsNullOrEmpty(createDto.CodigoInternal))
        {
          var exists = await _propertyRepository.ExistsByCodigoInternoAsync(createDto.CodigoInternal);
          if (exists)
          {
            return BadRequest(new { message = "Property with this codigo interno already exists" });
          }
        }

        var property = new Property
        {
          Name = createDto.Name,
          Address = createDto.Address,
          Price = createDto.Price,
          IdOwner = createDto.IdOwner,
          CodigoInternal = createDto.CodigoInternal,
          Year = createDto.Year,
          CreatedAt = DateTime.UtcNow,
          UpdatedAt = DateTime.UtcNow
        };

        await _propertyRepository.CreateAsync(property);

        // Create location places if provided
        await CreatePropertyPlacesAsync(property.Id, createDto);

        // Create image if provided
        if (!string.IsNullOrEmpty(createDto.Image))
        {
          await CreatePropertyImageAsync(property.Id, createDto.Image);
        }

        // Get the created property with details
        var createdProperty = await _propertyRepository.GetPropertyWithDetailsAsync(property.Id);
        var propertyDto = ConvertToDto(createdProperty!);

        return CreatedAtAction(nameof(GetPropertyById), new { id = property.Id }, propertyDto);
      }
      catch (Exception ex)
      {
        return StatusCode(500, new { message = "Error creating property", error = ex.Message });
      }
    }

    /// <summary>
    /// Update property
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProperty(string id, [FromBody] PropertyUpdateDto updateDto)
    {
      try
      {
        var existingProperty = await _propertyRepository.GetByIdAsync(id);
        if (existingProperty == null)
        {
          return NotFound(new { message = "Property not found" });
        }

        // Validate owner if provided
        if (!string.IsNullOrEmpty(updateDto.IdOwner))
        {
          var owner = await _ownerRepository.GetByIdAsync(updateDto.IdOwner);
          if (owner == null)
          {
            return BadRequest(new { message = "Owner not found" });
          }
        }

        // Update fields
        if (!string.IsNullOrEmpty(updateDto.Name))
          existingProperty.Name = updateDto.Name;

        if (!string.IsNullOrEmpty(updateDto.Address))
          existingProperty.Address = updateDto.Address;

        if (updateDto.Price.HasValue)
          existingProperty.Price = updateDto.Price.Value;

        if (!string.IsNullOrEmpty(updateDto.IdOwner))
          existingProperty.IdOwner = updateDto.IdOwner;

        if (!string.IsNullOrEmpty(updateDto.CodigoInternal))
          existingProperty.CodigoInternal = updateDto.CodigoInternal;

        if (updateDto.Year.HasValue)
          existingProperty.Year = updateDto.Year.Value;

        existingProperty.UpdatedAt = DateTime.UtcNow;

        await _propertyRepository.UpdateAsync(id, existingProperty);

        return NoContent();
      }
      catch (Exception ex)
      {
        return StatusCode(500, new { message = "Error updating property", error = ex.Message });
      }
    }

    /// <summary>
    /// Delete property
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProperty(string id)
    {
      try
      {
        var existingProperty = await _propertyRepository.GetByIdAsync(id);
        if (existingProperty == null)
        {
          return NotFound(new { message = "Property not found" });
        }

        await _propertyRepository.DeleteAsync(id);
        return NoContent();
      }
      catch (Exception ex)
      {
        return StatusCode(500, new { message = "Error deleting property", error = ex.Message });
      }
    }

    /// <summary>
    /// Get properties by owner
    /// </summary>
    [HttpGet("owner/{ownerId}")]
    public async Task<ActionResult<List<PropertyDto>>> GetPropertiesByOwner(string ownerId)
    {
      try
      {
        var properties = await _propertyRepository.GetPropertiesByOwnerAsync(ownerId);
        var propertyDtos = properties.Select(ConvertToDto).ToList();
        return Ok(propertyDtos);
      }
      catch (Exception ex)
      {
        return StatusCode(500, new { message = "Error retrieving properties by owner", error = ex.Message });
      }
    }

    /// <summary>
    /// Get properties by price range
    /// </summary>
    [HttpGet("price-range")]
    public async Task<ActionResult<List<PropertyDto>>> GetPropertiesByPriceRange(
        [FromQuery] decimal minPrice,
        [FromQuery] decimal maxPrice)
    {
      try
      {
        if (minPrice < 0 || maxPrice < 0 || minPrice > maxPrice)
        {
          return BadRequest(new { message = "Invalid price range" });
        }

        var properties = await _propertyRepository.GetPropertiesByPriceRangeAsync(minPrice, maxPrice);
        var propertyDtos = properties.Select(ConvertToDto).ToList();
        return Ok(propertyDtos);
      }
      catch (Exception ex)
      {
        return StatusCode(500, new { message = "Error retrieving properties by price range", error = ex.Message });
      }
    }

    #region Private Helper Methods

    private PropertyDto ConvertToDto(Property property)
    {
      var mainImage = property.Images?.FirstOrDefault(i => i.IsMain && i.Enabled)?.Image
                    ?? property.Images?.FirstOrDefault(i => i.Enabled)?.Image;

      var city = property.Places?.FirstOrDefault(p => p.Name.Equals("City", StringComparison.OrdinalIgnoreCase))?.Value ?? "";
      var state = property.Places?.FirstOrDefault(p => p.Name.Equals("State", StringComparison.OrdinalIgnoreCase))?.Value ?? "";
      var country = property.Places?.FirstOrDefault(p => p.Name.Equals("Country", StringComparison.OrdinalIgnoreCase))?.Value ?? "";

      return new PropertyDto
      {
        Id = property.Id,
        Name = property.Name,
        Address = property.Address,
        Price = property.Price,
        IdOwner = property.IdOwner,
        Image = mainImage,
        CodigoInternal = property.CodigoInternal,
        Year = property.Year,
        CreatedAt = property.CreatedAt,
        OwnerName = property.Owner?.FullName ?? "",
        OwnerPhone = property.Owner?.Phone ?? "",
        City = city,
        State = state,
        Country = country
      };
    }

    private async Task CreatePropertyPlacesAsync(string propertyId, PropertyCreateDto createDto)
    {
      var places = new List<PropertyPlace>();

      if (!string.IsNullOrEmpty(createDto.City))
      {
        places.Add(new PropertyPlace
        {
          IdProperty = propertyId,
          Name = "City",
          Value = createDto.City,
          PlaceType = "city",
          CreatedAt = DateTime.UtcNow,
          UpdatedAt = DateTime.UtcNow
        });
      }

      if (!string.IsNullOrEmpty(createDto.State))
      {
        places.Add(new PropertyPlace
        {
          IdProperty = propertyId,
          Name = "State",
          Value = createDto.State,
          PlaceType = "state",
          CreatedAt = DateTime.UtcNow,
          UpdatedAt = DateTime.UtcNow
        });
      }

      if (!string.IsNullOrEmpty(createDto.Country))
      {
        places.Add(new PropertyPlace
        {
          IdProperty = propertyId,
          Name = "Country",
          Value = createDto.Country,
          PlaceType = "country",
          CreatedAt = DateTime.UtcNow,
          UpdatedAt = DateTime.UtcNow
        });
      }

      foreach (var place in places)
      {
        // For simplicity, we're not creating a separate repository for PropertyPlace
        // In a real application, you might want to create a dedicated repository
      }
    }

    private async Task CreatePropertyImageAsync(string propertyId, string imageUrl)
    {
      var image = new PropertyImage
      {
        IdProperty = propertyId,
        CloudinaryUrl = imageUrl, // Usar CloudinaryUrl en lugar de Image
        CloudinaryPublicId = ExtractPublicIdFromUrl(imageUrl),
        OriginalFileName = "uploaded-image",
        Enabled = true,
        IsMain = true,
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
      };

      // For simplicity, we're not creating a separate repository for PropertyImage
      // In a real application, you might want to create a dedicated repository
    }

    private string ExtractPublicIdFromUrl(string cloudinaryUrl)
    {
      if (string.IsNullOrEmpty(cloudinaryUrl))
        return string.Empty;

      try
      {
        // Extract public ID from Cloudinary URL
        // Example: https://res.cloudinary.com/demo/image/upload/sample.jpg
        var parts = cloudinaryUrl.Split('/');
        var fileName = parts.Last();
        return fileName.Split('.')[0]; // Remove extension
      }
      catch
      {
        return string.Empty;
      }
    }

    #endregion
  }
}