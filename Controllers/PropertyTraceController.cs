namespace ASP.MongoDb.API.Controllers
{
  using ASP.MongoDb.API.Entities;
  using ASP.MongoDb.API.Repository;
  using ASP.MongoDb.API.DTOs;
  using Microsoft.AspNetCore.Mvc;

  [ApiController]
  [Route("api/[controller]")]
  public class PropertyTraceController : ControllerBase
  {
    private readonly IPropertyTraceRepository _propertyTraceRepository;
    private readonly IPropertyRepository _propertyRepository;

    public PropertyTraceController(IPropertyTraceRepository propertyTraceRepository, IPropertyRepository propertyRepository)
    {
      _propertyTraceRepository = propertyTraceRepository;
      _propertyRepository = propertyRepository;
    }

    /// <summary>
    /// Get all traces for a specific property
    /// </summary>
    [HttpGet("property/{propertyId}")]
    public async Task<ActionResult<List<PropertyTraceResponseDto>>> GetTracesByProperty(string propertyId)
    {
      try
      {
        // Verify property exists
        var property = await _propertyRepository.GetByIdAsync(propertyId);
        if (property == null)
        {
          return NotFound(new { message = "Property not found" });
        }

        var traces = await _propertyTraceRepository.GetByPropertyIdAsync(propertyId);
        var traceDtos = traces.Select(ConvertToDto).ToList();

        return Ok(traceDtos);
      }
      catch (Exception ex)
      {
        return StatusCode(500, new { message = "Error retrieving property traces", error = ex.Message });
      }
    }

    /// <summary>
    /// Get all traces
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<PropertyTraceResponseDto>>> GetAllTraces()
    {
      try
      {
        var traces = await _propertyTraceRepository.GetAllAsync();
        var traceDtos = traces.Select(ConvertToDto).ToList();

        return Ok(traceDtos);
      }
      catch (Exception ex)
      {
        return StatusCode(500, new { message = "Error retrieving traces", error = ex.Message });
      }
    }

    /// <summary>
    /// Get trace by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<PropertyTraceResponseDto>> GetTraceById(string id)
    {
      try
      {
        var trace = await _propertyTraceRepository.GetByIdAsync(id);
        if (trace == null)
        {
          return NotFound(new { message = "Trace not found" });
        }

        var traceDto = ConvertToDto(trace);
        return Ok(traceDto);
      }
      catch (Exception ex)
      {
        return StatusCode(500, new { message = "Error retrieving trace", error = ex.Message });
      }
    }

    /// <summary>
    /// Create new property trace
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<PropertyTraceResponseDto>> CreateTrace([FromBody] PropertyTraceCreateDto createDto)
    {
      try
      {
        // Verify property exists
        var property = await _propertyRepository.GetByIdAsync(createDto.IdProperty);
        if (property == null)
        {
          return BadRequest(new { message = "Property not found" });
        }

        var trace = new PropertyTrace
        {
          IdProperty = createDto.IdProperty,
          DateSale = createDto.DateSale,
          Name = createDto.Name,
          Value = createDto.Value,
          Tax = createDto.Tax,
          CreatedAt = DateTime.UtcNow,
          UpdatedAt = DateTime.UtcNow
        };

        await _propertyTraceRepository.CreateAsync(trace);

        var traceDto = ConvertToDto(trace);
        return CreatedAtAction(nameof(GetTraceById), new { id = trace.Id }, traceDto);
      }
      catch (Exception ex)
      {
        return StatusCode(500, new { message = "Error creating trace", error = ex.Message });
      }
    }

    /// <summary>
    /// Update property trace
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTrace(string id, [FromBody] PropertyTraceUpdateDto updateDto)
    {
      try
      {
        var existingTrace = await _propertyTraceRepository.GetByIdAsync(id);
        if (existingTrace == null)
        {
          return NotFound(new { message = "Trace not found" });
        }

        // Update fields
        if (updateDto.DateSale.HasValue)
          existingTrace.DateSale = updateDto.DateSale.Value;

        if (!string.IsNullOrEmpty(updateDto.Name))
          existingTrace.Name = updateDto.Name;

        if (updateDto.Value.HasValue)
          existingTrace.Value = updateDto.Value.Value;

        if (updateDto.Tax.HasValue)
          existingTrace.Tax = updateDto.Tax.Value;

        existingTrace.UpdatedAt = DateTime.UtcNow;

        await _propertyTraceRepository.UpdateAsync(id, existingTrace);
        return NoContent();
      }
      catch (Exception ex)
      {
        return StatusCode(500, new { message = "Error updating trace", error = ex.Message });
      }
    }

    /// <summary>
    /// Delete property trace
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTrace(string id)
    {
      try
      {
        var existingTrace = await _propertyTraceRepository.GetByIdAsync(id);
        if (existingTrace == null)
        {
          return NotFound(new { message = "Trace not found" });
        }

        await _propertyTraceRepository.DeleteAsync(id);
        return NoContent();
      }
      catch (Exception ex)
      {
        return StatusCode(500, new { message = "Error deleting trace", error = ex.Message });
      }
    }

    /// <summary>
    /// Delete all traces for a property
    /// </summary>
    [HttpDelete("property/{propertyId}")]
    public async Task<IActionResult> DeleteTracesByProperty(string propertyId)
    {
      try
      {
        // Verify property exists
        var property = await _propertyRepository.GetByIdAsync(propertyId);
        if (property == null)
        {
          return NotFound(new { message = "Property not found" });
        }

        await _propertyTraceRepository.DeleteByPropertyIdAsync(propertyId);
        return NoContent();
      }
      catch (Exception ex)
      {
        return StatusCode(500, new { message = "Error deleting property traces", error = ex.Message });
      }
    }

    #region Private Helper Methods

    private PropertyTraceResponseDto ConvertToDto(PropertyTrace trace)
    {
      return new PropertyTraceResponseDto
      {
        Id = trace.Id,
        IdProperty = trace.IdProperty,
        DateSale = trace.DateSale.ToString("yyyy-MM-dd"),
        Name = trace.Name,
        Value = trace.Value,
        Tax = trace.Tax,
        CreatedAt = trace.CreatedAt
      };
    }

    #endregion
  }
}