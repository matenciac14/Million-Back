namespace ASP.MongoDb.API.Controllers
{
  using ASP.MongoDb.API.Services;
  using ASP.MongoDb.API.Repository;
  using ASP.MongoDb.API.Entities;
  using Microsoft.AspNetCore.Mvc;
  using CloudinaryDotNet.Actions;

  [ApiController]
  [Route("api/[controller]")]
  public class ImageController : ControllerBase
  {
    private readonly IImageService _imageService;
    private readonly IPropertyImageRepository _propertyImageRepository;

    public ImageController(IImageService imageService, IPropertyImageRepository propertyImageRepository)
    {
      _imageService = imageService;
      _propertyImageRepository = propertyImageRepository;
    }

    /// <summary>
    /// Upload a single image to Cloudinary and optionally associate with a property
    /// Usage:
    /// - For general upload: POST /api/Image/upload with file only
    /// - For property image: POST /api/Image/upload with file + propertyId + description + isMain
    /// </summary>
    [HttpPost("upload")]
    public async Task<ActionResult<object>> UploadImage(
        IFormFile file,
        [FromForm] string? propertyId = null,
        [FromForm] string? description = null,
        [FromForm] bool isMain = false,
        [FromQuery] string? folder = null)
    {
      try
      {
        if (file == null || file.Length == 0)
        {
          return BadRequest(new { message = "No file provided" });
        }

        // Determine folder based on whether it's a property image
        var uploadFolder = !string.IsNullOrEmpty(propertyId) ? "real-estate/properties" : (folder ?? "general");

        var result = await _imageService.UploadImageAsync(file, uploadFolder);

        if (result.Error != null)
        {
          return BadRequest(new
          {
            message = "Upload failed",
            error = result.Error.Message
          });
        }

        // If propertyId is provided, save to database as PropertyImage
        if (!string.IsNullOrEmpty(propertyId))
        {
          var propertyImage = new PropertyImage
          {
            IdProperty = propertyId,
            CloudinaryPublicId = result.PublicId,
            CloudinaryUrl = result.SecureUrl?.ToString() ?? "",
            OriginalFileName = file.FileName,
            Width = result.Width,
            Height = result.Height,
            Format = result.Format,
            Bytes = result.Bytes,
            Description = description ?? "",
            IsMain = isMain,
            Enabled = true
          };

          // Save to database
          await _propertyImageRepository.CreateAsync(propertyImage);

          // If this is main image, update it properly
          if (isMain)
          {
            await _propertyImageRepository.SetMainImageAsync(propertyId, propertyImage.Id);
          }

          return Ok(new
          {
            id = propertyImage.Id,
            propertyId = propertyImage.IdProperty,
            publicId = result.PublicId,
            url = result.SecureUrl?.ToString(),
            width = result.Width,
            height = result.Height,
            format = result.Format,
            bytes = result.Bytes,
            description = propertyImage.Description,
            isMain = propertyImage.IsMain,
            enabled = propertyImage.Enabled,
            createdAt = propertyImage.CreatedAt
          });
        }

        // Return simple Cloudinary response for general uploads
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

    #region Property Images Management

    /// <summary>
    /// Upload image and associate it with a property
    /// </summary>
    [HttpPost("property/{propertyId}/upload")]
    public async Task<ActionResult<object>> UploadPropertyImage(string propertyId, IFormFile file, [FromQuery] string? description = null, [FromQuery] bool isMain = false)
    {
      try
      {
        if (file == null || file.Length == 0)
        {
          return BadRequest(new { message = "No file provided" });
        }

        // Upload to Cloudinary
        var cloudinaryResult = await _imageService.UploadImageAsync(file, "real-estate/properties");

        if (cloudinaryResult.Error != null)
        {
          return BadRequest(new
          {
            message = "Upload failed",
            error = cloudinaryResult.Error.Message
          });
        }

        // Create PropertyImage entity
        var propertyImage = new PropertyImage
        {
          IdProperty = propertyId,
          CloudinaryPublicId = cloudinaryResult.PublicId,
          CloudinaryUrl = cloudinaryResult.SecureUrl?.ToString() ?? "",
          OriginalFileName = file.FileName,
          Width = cloudinaryResult.Width,
          Height = cloudinaryResult.Height,
          Format = cloudinaryResult.Format,
          Bytes = cloudinaryResult.Bytes,
          Description = description ?? "",
          IsMain = isMain,
          Enabled = true
        };

        // Save to database
        await _propertyImageRepository.CreateAsync(propertyImage);

        // If this is main image, update it properly
        if (isMain)
        {
          await _propertyImageRepository.SetMainImageAsync(propertyId, propertyImage.Id);
        }

        return Ok(new
        {
          id = propertyImage.Id,
          propertyId = propertyImage.IdProperty,
          publicId = propertyImage.CloudinaryPublicId,
          url = propertyImage.CloudinaryUrl,
          width = propertyImage.Width,
          height = propertyImage.Height,
          format = propertyImage.Format,
          bytes = propertyImage.Bytes,
          description = propertyImage.Description,
          isMain = propertyImage.IsMain,
          enabled = propertyImage.Enabled,
          createdAt = propertyImage.CreatedAt
        });
      }
      catch (Exception ex)
      {
        return StatusCode(500, new
        {
          message = "Error uploading property image",
          error = ex.Message
        });
      }
    }

    /// <summary>
    /// Get all images for a property
    /// </summary>
    [HttpGet("property/{propertyId}")]
    public async Task<ActionResult<object>> GetPropertyImages(string propertyId, [FromQuery] bool enabledOnly = true)
    {
      try
      {
        var images = enabledOnly
          ? await _propertyImageRepository.GetEnabledByPropertyIdAsync(propertyId)
          : await _propertyImageRepository.GetByPropertyIdAsync(propertyId);

        var result = images.Select(img => new
        {
          id = img.Id,
          propertyId = img.IdProperty,
          publicId = img.CloudinaryPublicId,
          url = img.CloudinaryUrl,
          width = img.Width,
          height = img.Height,
          format = img.Format,
          bytes = img.Bytes,
          description = img.Description,
          isMain = img.IsMain,
          enabled = img.Enabled,
          createdAt = img.CreatedAt,
          thumbnailUrl = img.ThumbnailUrl,
          mediumUrl = img.MediumUrl,
          largeUrl = img.LargeUrl
        });

        return Ok(new
        {
          propertyId = propertyId,
          images = result,
          totalCount = images.Count
        });
      }
      catch (Exception ex)
      {
        return StatusCode(500, new
        {
          message = "Error retrieving property images",
          error = ex.Message
        });
      }
    }

    /// <summary>
    /// Set an image as the main image for a property
    /// </summary>
    [HttpPut("property/{propertyId}/main/{imageId}")]
    public async Task<ActionResult<object>> SetMainImage(string propertyId, string imageId)
    {
      try
      {
        var success = await _propertyImageRepository.SetMainImageAsync(propertyId, imageId);

        if (!success)
        {
          return NotFound(new { message = "Image not found or could not be set as main" });
        }

        return Ok(new { message = "Main image updated successfully", propertyId, imageId });
      }
      catch (Exception ex)
      {
        return StatusCode(500, new
        {
          message = "Error setting main image",
          error = ex.Message
        });
      }
    }

    /// <summary>
    /// Delete a specific property image
    /// </summary>
    [HttpDelete("property/{propertyId}/image/{imageId}")]
    public async Task<ActionResult<object>> DeletePropertyImage(string propertyId, string imageId)
    {
      try
      {
        // Get the image to retrieve Cloudinary public ID
        var image = await _propertyImageRepository.GetByIdAsync(imageId);
        if (image == null || image.IdProperty != propertyId)
        {
          return NotFound(new { message = "Image not found" });
        }

        // Delete from Cloudinary
        var cloudinaryResult = await _imageService.DeleteImageAsync(image.CloudinaryPublicId);

        // Delete from database
        await _propertyImageRepository.DeleteAsync(imageId);

        return Ok(new
        {
          message = "Property image deleted successfully",
          propertyId = propertyId,
          imageId = imageId,
          cloudinaryResult = cloudinaryResult?.Result
        });
      }
      catch (Exception ex)
      {
        return StatusCode(500, new
        {
          message = "Error deleting property image",
          error = ex.Message
        });
      }
    }

    /// <summary>
    /// Get responsive URLs for a property image
    /// </summary>
    [HttpGet("property/{propertyId}/image/{imageId}/responsive")]
    public async Task<ActionResult<object>> GetPropertyImageResponsive(string propertyId, string imageId)
    {
      try
      {
        var image = await _propertyImageRepository.GetByIdAsync(imageId);
        if (image == null || image.IdProperty != propertyId)
        {
          return NotFound(new { message = "Image not found" });
        }

        var sizes = new Dictionary<string, object>
        {
          ["thumbnail"] = new
          {
            url = _imageService.GenerateImageUrl(image.CloudinaryPublicId, 150, 150),
            width = 150,
            height = 150
          },
          ["small"] = new
          {
            url = _imageService.GenerateImageUrl(image.CloudinaryPublicId, 400, 300),
            width = 400,
            height = 300
          },
          ["medium"] = new
          {
            url = _imageService.GenerateImageUrl(image.CloudinaryPublicId, 800, 600),
            width = 800,
            height = 600
          },
          ["large"] = new
          {
            url = _imageService.GenerateImageUrl(image.CloudinaryPublicId, 1200, 900),
            width = 1200,
            height = 900
          },
          ["original"] = new
          {
            url = _imageService.GenerateImageUrl(image.CloudinaryPublicId),
            width = image.Width,
            height = image.Height
          }
        };

        return Ok(new
        {
          propertyId = propertyId,
          imageId = imageId,
          publicId = image.CloudinaryPublicId,
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

    #endregion
  }
}