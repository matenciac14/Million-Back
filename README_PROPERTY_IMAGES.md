<!-- @format -->

# 📸 Property Image Management - Documentation Update

## New Property Image Endpoints (Missing from main README.md)

### 🔧 PropertyImageRepository Implementation

The system now includes a dedicated repository for managing property images:

```csharp
public interface IPropertyImageRepository : IRepository<PropertyImage>
{
    Task<List<PropertyImage>> GetByPropertyIdAsync(string propertyId);
    Task<List<PropertyImage>> GetEnabledByPropertyIdAsync(string propertyId);
    Task<bool> SetMainImageAsync(string propertyId, string imageId);
    Task DeleteByPropertyIdAsync(string propertyId);
}
```

### 📡 New Property Image Endpoints

#### Upload Image to Property

```
POST /api/Image/property/{propertyId}/upload
Query Parameters:
- description: string (optional)
- isMain: boolean (optional, default: false)
Body: multipart/form-data with file
```

#### Get Property Images

```
GET /api/Image/property/{propertyId}
Query Parameters:
- enabledOnly: boolean (optional, default: true)
```

#### Set Main Property Image

```
PUT /api/Image/property/{propertyId}/main/{imageId}
```

#### Delete Property Image

```
DELETE /api/Image/property/{propertyId}/image/{imageId}
```

#### Get Responsive URLs for Property Image

```
GET /api/Image/property/{propertyId}/image/{imageId}/responsive
```

### 🗄️ Updated Database Schema

#### PropertyImage Entity (Enhanced)

```json
{
  "_id": "ObjectId",
  "IdProperty": "ObjectId",
  "CloudinaryPublicId": "string",
  "CloudinaryUrl": "string",
  "OriginalFileName": "string",
  "Width": "int",
  "Height": "int",
  "Format": "string",
  "Bytes": "long",
  "Enabled": "bool",
  "IsMain": "bool",
  "Description": "string",
  "CreatedAt": "DateTime",
  "UpdatedAt": "DateTime"
}
```

### 🔗 Property-Image Relationship

- **One-to-Many**: One Property can have multiple PropertyImages
- **Main Image**: Each property can have one designated main image
- **Cloudinary Integration**: All images stored in Cloudinary with responsive URLs
- **Responsive URLs**: Automatic generation of thumbnail, medium, and large sizes

### 📋 Example Usage

#### Complete Property with Multiple Images Workflow

```bash
# 1. Create property
POST /api/Property
{
  "name": "Modern Apartment",
  "address": "Main St 123",
  "price": 500000000,
  "idOwner": "64a1b2c3d4e5f6789012345"
}

# 2. Upload main image
POST /api/Image/property/PROPERTY_ID/upload?description=Facade&isMain=true
[multipart file data]

# 3. Upload additional images
POST /api/Image/property/PROPERTY_ID/upload?description=Living Room
[multipart file data]

POST /api/Image/property/PROPERTY_ID/upload?description=Kitchen
[multipart file data]

# 4. Get all property images
GET /api/Image/property/PROPERTY_ID

# Response includes responsive URLs:
{
  "images": [
    {
      "id": "...",
      "description": "Facade",
      "isMain": true,
      "thumbnailUrl": "https://res.cloudinary.com/.../w_150,h_150,c_fill,g_auto/...",
      "mediumUrl": "https://res.cloudinary.com/.../w_800,h_600,c_fill,g_auto/...",
      "largeUrl": "https://res.cloudinary.com/.../w_1200,h_900,c_fill,g_auto/..."
    }
  ]
}
```

### 🏗️ Architecture Update

```
Repository/
├── IPropertyImageRepository.cs    # NEW: Property images interface
├── PropertyImageRepository.cs     # NEW: Property images implementation
├── IPropertyRepository.cs         # Enhanced with image relationships
└── PropertyRepository.cs          # Enhanced with image loading

Controllers/
├── ImageController.cs             # Enhanced with property-specific endpoints
└── PropertyController.cs          # Integration with image system

Entities/
├── PropertyImage.cs               # Enhanced with Cloudinary fields
└── Property.cs                    # Navigation property for images
```

This documentation should be integrated into the main README.md file to provide complete coverage of the property image management system.
