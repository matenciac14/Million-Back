namespace ASP.MongoDb.API.Controllers
{
  using ASP.MongoDb.API.Services;
  using Microsoft.AspNetCore.Mvc;
  using CloudinaryDotNet.Actions;

  [ApiController]
  [Route("api/[controller]")]
  public class ImageController : ControllerBase
  {
    private readonly IImageService _imageService;

    public ImageController(IImageService imageService)
    {
      _imageService = imageService;
    }

    /// <summary>
    /// Upload a single image to Cloudinary
    /// </summary>
    [HttpPost("upload")]
    public async Task<ActionResult<object>> UploadImage(IFormFile file, [FromQuery] string? folder = null)
    {
      try
      {
        if (file == null || file.Length == 0)
        {
          return BadRequest(new { message = "No file provided" });
        }

        var result = await _imageService.UploadImageAsync(file, folder);

        if (result.Error != null)
        {
          return BadRequest(new
          {
            message = "Upload failed",
            error = result.Error.Message
          });
        }

        return Ok(new
        {
          publicId = result.PublicId,
          url = result.SecureUrl?.ToString(),
          width = result.Width,
          height = result.Height,
          format = result.Format,
          bytes = result.Bytes,
          createdAt = result.CreatedAt
        });
      }
      catch (Exception ex)
      {
        return StatusCode(500, new
        {
          message = "Error uploading image",
          error = ex.Message
        });
      }
    }

    /// <summary>
    /// Upload multiple images to Cloudinary
    /// </summary>
    [HttpPost("upload-multiple")]
    public async Task<ActionResult<object>> UploadMultipleImages(IFormFileCollection files, [FromQuery] string? folder = null)
    {
      try
      {
        if (files == null || files.Count == 0)
        {
          return BadRequest(new { message = "No files provided" });
        }

        var results = await _imageService.UploadMultipleImagesAsync(files, folder);

        var response = results.Select(result => new
        {
          publicId = result.PublicId,
          url = result.SecureUrl?.ToString(),
          width = result.Width,
          height = result.Height,
          format = result.Format,
          bytes = result.Bytes,
          error = result.Error?.Message,
          success = result.Error == null
        }).ToList();

        return Ok(new
        {
          totalFiles = files.Count,
          successfulUploads = response.Count(r => r.success),
          failedUploads = response.Count(r => !r.success),
          results = response
        });
      }
      catch (Exception ex)
      {
        return StatusCode(500, new
        {
          message = "Error uploading images",
          error = ex.Message
        });
      }
    }

    /// <summary>
    /// Delete an image from Cloudinary
    /// </summary>
    [HttpDelete("{publicId}")]
    public async Task<ActionResult<object>> DeleteImage(string publicId)
    {
      try
      {
        if (string.IsNullOrEmpty(publicId))
        {
          return BadRequest(new { message = "Public ID is required" });
        }

        var result = await _imageService.DeleteImageAsync(publicId);

        if (result.Error != null)
        {
          return BadRequest(new
          {
            message = "Delete failed",
            error = result.Error.Message
          });
        }

        return Ok(new
        {
          message = "Image deleted successfully",
          publicId = publicId,
          result = result.Result
        });
      }
      catch (Exception ex)
      {
        return StatusCode(500, new
        {
          message = "Error deleting image",
          error = ex.Message
        });
      }
    }

    /// <summary>
    /// Generate optimized image URL with transformations
    /// </summary>
    [HttpGet("url/{publicId}")]
    public ActionResult<object> GenerateImageUrl(
        string publicId,
        [FromQuery] int? width = null,
        [FromQuery] int? height = null,
        [FromQuery] string? format = null)
    {
      try
      {
        if (string.IsNullOrEmpty(publicId))
        {
          return BadRequest(new { message = "Public ID is required" });
        }

        var url = _imageService.GenerateImageUrl(publicId, width, height, format);

        return Ok(new
        {
          publicId = publicId,
          url = url,
          transformations = new
          {
            width = width,
            height = height,
            format = format ?? "auto"
          }
        });
      }
      catch (Exception ex)
      {
        return StatusCode(500, new
        {
          message = "Error generating image URL",
          error = ex.Message
        });
      }
    }

    /// <summary>
    /// Get image info and multiple sizes for responsive display
    /// </summary>
    [HttpGet("responsive/{publicId}")]
    public ActionResult<object> GetResponsiveImageUrls(string publicId)
    {
      try
      {
        if (string.IsNullOrEmpty(publicId))
        {
          return BadRequest(new { message = "Public ID is required" });
        }

        var sizes = new Dictionary<string, object>
        {
          ["thumbnail"] = new
          {
            url = _imageService.GenerateImageUrl(publicId, 150, 150),
            width = 150,
            height = 150
          },
          ["small"] = new
          {
            url = _imageService.GenerateImageUrl(publicId, 400, 300),
            width = 400,
            height = 300
          },
          ["medium"] = new
          {
            url = _imageService.GenerateImageUrl(publicId, 800, 600),
            width = 800,
            height = 600
          },
          ["large"] = new
          {
            url = _imageService.GenerateImageUrl(publicId, 1200, 900),
            width = 1200,
            height = 900
          },
          ["original"] = new
          {
            url = _imageService.GenerateImageUrl(publicId),
            width = "auto",
            height = "auto"
          }
        };

        return Ok(new
        {
          publicId = publicId,
          sizes = sizes
        });
      }
      catch (Exception ex)
      {
        return StatusCode(500, new
        {
          message = "Error generating responsive URLs",
          error = ex.Message
        });
      }
    }
  }
}