namespace ASP.MongoDb.API.Controllers
{
  using ASP.MongoDb.API.Entities;
  using ASP.MongoDb.API.Repository;
  using ASP.MongoDb.API.DTOs;
  using Microsoft.AspNetCore.Mvc;

  [ApiController]
  [Route("api/[controller]")]
  public class OwnerController : ControllerBase
  {
    private readonly IOwnerRepository _ownerRepository;

    public OwnerController(IOwnerRepository ownerRepository)
    {
      _ownerRepository = ownerRepository;
    }

    /// <summary>
    /// Get all owners
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<OwnerDto>>> GetAllOwners()
    {
      try
      {
        var owners = await _ownerRepository.GetAllAsync();
        var ownerDtos = owners.Select(ConvertToDto).ToList();
        return Ok(ownerDtos);
      }
      catch (Exception ex)
      {
        return StatusCode(500, new { message = "Error retrieving owners", error = ex.Message });
      }
    }

    /// <summary>
    /// Get owner by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<OwnerDto>> GetOwnerById(string id)
    {
      try
      {
        var owner = await _ownerRepository.GetByIdAsync(id);
        if (owner == null)
        {
          return NotFound(new { message = "Owner not found" });
        }

        var ownerDto = ConvertToDto(owner);
        return Ok(ownerDto);
      }
      catch (Exception ex)
      {
        return StatusCode(500, new { message = "Error retrieving owner", error = ex.Message });
      }
    }

    /// <summary>
    /// Create new owner
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<OwnerDto>> CreateOwner([FromBody] OwnerCreateDto createDto)
    {
      try
      {
        // Check if email already exists
        if (!string.IsNullOrEmpty(createDto.Email))
        {
          var exists = await _ownerRepository.ExistsByEmailAsync(createDto.Email);
          if (exists)
          {
            return BadRequest(new { message = "Owner with this email already exists" });
          }
        }

        var owner = new Owner
        {
          Name = createDto.Name,
          LastName = createDto.LastName,
          Phone = createDto.Phone,
          Birthday = createDto.Birthday,
          Email = createDto.Email,
          CreatedAt = DateTime.UtcNow,
          UpdatedAt = DateTime.UtcNow
        };

        await _ownerRepository.CreateAsync(owner);

        var ownerDto = ConvertToDto(owner);
        return CreatedAtAction(nameof(GetOwnerById), new { id = owner.Id }, ownerDto);
      }
      catch (Exception ex)
      {
        return StatusCode(500, new { message = "Error creating owner", error = ex.Message });
      }
    }

    /// <summary>
    /// Update owner
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOwner(string id, [FromBody] OwnerUpdateDto updateDto)
    {
      try
      {
        var existingOwner = await _ownerRepository.GetByIdAsync(id);
        if (existingOwner == null)
        {
          return NotFound(new { message = "Owner not found" });
        }

        // Check if email is being changed and if new email already exists
        if (!string.IsNullOrEmpty(updateDto.Email) && updateDto.Email != existingOwner.Email)
        {
          var exists = await _ownerRepository.ExistsByEmailAsync(updateDto.Email);
          if (exists)
          {
            return BadRequest(new { message = "Owner with this email already exists" });
          }
        }

        // Update fields
        if (!string.IsNullOrEmpty(updateDto.Name))
          existingOwner.Name = updateDto.Name;

        if (updateDto.LastName != null)
          existingOwner.LastName = updateDto.LastName;

        if (updateDto.Phone != null)
          existingOwner.Phone = updateDto.Phone;

        if (updateDto.Birthday.HasValue)
          existingOwner.Birthday = updateDto.Birthday;

        if (!string.IsNullOrEmpty(updateDto.Email))
          existingOwner.Email = updateDto.Email;

        existingOwner.UpdatedAt = DateTime.UtcNow;

        await _ownerRepository.UpdateAsync(id, existingOwner);
        return NoContent();
      }
      catch (Exception ex)
      {
        return StatusCode(500, new { message = "Error updating owner", error = ex.Message });
      }
    }

    /// <summary>
    /// Delete owner
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOwner(string id)
    {
      try
      {
        var existingOwner = await _ownerRepository.GetByIdAsync(id);
        if (existingOwner == null)
        {
          return NotFound(new { message = "Owner not found" });
        }

        await _ownerRepository.DeleteAsync(id);
        return NoContent();
      }
      catch (Exception ex)
      {
        return StatusCode(500, new { message = "Error deleting owner", error = ex.Message });
      }
    }

    /// <summary>
    /// Search owners by name
    /// </summary>
    [HttpGet("search")]
    public async Task<ActionResult<List<OwnerDto>>> SearchOwners([FromQuery] string name)
    {
      try
      {
        if (string.IsNullOrEmpty(name))
        {
          return BadRequest(new { message = "Name parameter is required" });
        }

        var owners = await _ownerRepository.SearchByNameAsync(name);
        var ownerDtos = owners.Select(ConvertToDto).ToList();
        return Ok(ownerDtos);
      }
      catch (Exception ex)
      {
        return StatusCode(500, new { message = "Error searching owners", error = ex.Message });
      }
    }

    #region Private Helper Methods

    private OwnerDto ConvertToDto(Owner owner)
    {
      return new OwnerDto
      {
        Id = owner.Id,
        Name = owner.Name,
        LastName = owner.LastName,
        Phone = owner.Phone,
        Birthday = owner.Birthday,
        Email = owner.Email,
        FullName = owner.FullName,
        CreatedAt = owner.CreatedAt
      };
    }

    #endregion
  }
}