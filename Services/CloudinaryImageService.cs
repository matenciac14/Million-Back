namespace ASP.MongoDb.API.Services
{
  using CloudinaryDotNet;
  using CloudinaryDotNet.Actions;
  using Microsoft.Extensions.Options;
  using ASP.MongoDb.API.Settings;

  public interface IImageService
  {
    Task<ImageUploadResult> UploadImageAsync(IFormFile file, string? folder = null);
    Task<ImageUploadResult> UploadImageAsync(byte[] imageData, string fileName, string? folder = null);
    Task<DeletionResult> DeleteImageAsync(string publicId);
    string GenerateImageUrl(string publicId, int? width = null, int? height = null, string? format = null);
    Task<List<ImageUploadResult>> UploadMultipleImagesAsync(IFormFileCollection files, string? folder = null);
  }

  public class CloudinaryImageService : IImageService
  {
    private readonly Cloudinary _cloudinary;
    private readonly CloudinarySettings _settings;

    public CloudinaryImageService(IOptions<CloudinarySettings> cloudinarySettings)
    {
      _settings = cloudinarySettings.Value;

      var account = new Account(
          _settings.CloudName,
          _settings.ApiKey,
          _settings.ApiSecret
      );

      _cloudinary = new Cloudinary(account);
    }

    public async Task<ImageUploadResult> UploadImageAsync(IFormFile file, string? folder = null)
    {
      if (file == null || file.Length == 0)
        throw new ArgumentException("File is required", nameof(file));

      // Validar tipo de archivo
      var allowedTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/gif", "image/webp" };
      if (!allowedTypes.Contains(file.ContentType?.ToLower()))
        throw new ArgumentException("Invalid file type. Only JPEG, PNG, GIF, and WebP are allowed.");

      // Validar tamaño (máximo 10MB)
      if (file.Length > 10 * 1024 * 1024)
        throw new ArgumentException("File size cannot exceed 10MB.");

      var uploadParams = new ImageUploadParams()
      {
        File = new FileDescription(file.FileName, file.OpenReadStream()),
        UseFilename = true,
        UniqueFilename = true,
        Overwrite = false,
        Folder = folder ?? "real-estate/properties",
        Transformation = new Transformation()
              .Quality("auto:best")
              .FetchFormat("auto")
              .Chain()
              .Width(1920).Height(1080).Crop("limit")
      };

      return await _cloudinary.UploadAsync(uploadParams);
    }

    public async Task<ImageUploadResult> UploadImageAsync(byte[] imageData, string fileName, string? folder = null)
    {
      if (imageData == null || imageData.Length == 0)
        throw new ArgumentException("Image data is required", nameof(imageData));

      var uploadParams = new ImageUploadParams()
      {
        File = new FileDescription(fileName, new MemoryStream(imageData)),
        UseFilename = true,
        UniqueFilename = true,
        Overwrite = false,
        Folder = folder ?? "real-estate/properties",
        Transformation = new Transformation()
              .Quality("auto:best")
              .FetchFormat("auto")
              .Chain()
              .Width(1920).Height(1080).Crop("limit")
      };

      return await _cloudinary.UploadAsync(uploadParams);
    }

    public async Task<DeletionResult> DeleteImageAsync(string publicId)
    {
      if (string.IsNullOrEmpty(publicId))
        throw new ArgumentException("Public ID is required", nameof(publicId));

      var deleteParams = new DeletionParams(publicId)
      {
        ResourceType = ResourceType.Image
      };

      return await _cloudinary.DestroyAsync(deleteParams);
    }

    public string GenerateImageUrl(string publicId, int? width = null, int? height = null, string? format = null)
    {
      if (string.IsNullOrEmpty(publicId))
        return string.Empty;

      var transformation = new Transformation()
          .Quality("auto:best")
          .FetchFormat(format ?? "auto");

      if (width.HasValue || height.HasValue)
      {
        transformation = transformation.Width(width).Height(height).Crop("fill").Gravity("auto");
      }

      return _cloudinary.Api.UrlImgUp.Transform(transformation).BuildUrl(publicId);
    }

    public async Task<List<ImageUploadResult>> UploadMultipleImagesAsync(IFormFileCollection files, string? folder = null)
    {
      var results = new List<ImageUploadResult>();

      foreach (var file in files)
      {
        try
        {
          var result = await UploadImageAsync(file, folder);
          results.Add(result);
        }
        catch (Exception ex)
        {
          // Log error but continue with other files
          Console.WriteLine($"Error uploading file {file.FileName}: {ex.Message}");

          // Add failed result
          results.Add(new ImageUploadResult
          {
            Error = new Error { Message = ex.Message }
          });
        }
      }

      return results;
    }
  }
}