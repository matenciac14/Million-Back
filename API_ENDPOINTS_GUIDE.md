<!-- @format -->

# 🚀 API Endpoints Documentation

## 📝 Complete Property Management System

### 👤 **OWNER ENDPOINTS**

#### Create Owner

```bash
POST /api/Owner
Content-Type: application/json

{
  "name": "Juan",
  "lastName": "Pérez",
  "email": "juan@email.com",
  "phone": "+57 300 123 4567"
}
```

#### Get All Owners

```bash
GET /api/Owner
```

#### Get Owner by ID

```bash
GET /api/Owner/{id}
```

#### Update Owner

```bash
PUT /api/Owner/{id}
Content-Type: application/json

{
  "name": "Juan Carlos",
  "lastName": "Pérez López",
  "email": "juan.carlos@email.com",
  "phone": "+57 300 123 4567"
}
```

#### Delete Owner

```bash
DELETE /api/Owner/{id}
```

---

### 🏠 **PROPERTY ENDPOINTS**

#### Create Property

```bash
POST /api/Property
Content-Type: application/json

{
  "name": "Casa Moderna",
  "address": "Calle 123 #45-67",
  "price": 500000000,
  "idOwner": "owner_id_here",
  "codigoInternal": "CM001",
  "year": 2024,
  "city": "Bogotá",
  "state": "Cundinamarca",
  "country": "Colombia"
}
```

#### Get All Properties (with filters)

```bash
GET /api/Property
GET /api/Property?city=Bogotá
GET /api/Property?minPrice=100000000&maxPrice=500000000
GET /api/Property?ownerId=owner_id_here
```

#### Get Property by ID (Complete with Images, Owner, Traces)

```bash
GET /api/Property/{id}

# Returns:
{
  "id": "property_id",
  "name": "Casa Moderna",
  "address": "Calle 123 #45-67",
  "price": 500000000,
  "images": [
    {
      "idPropertyImage": "img_id",
      "file": "https://cloudinary.com/url",
      "enabled": true,
      "isMain": true,
      "description": "Fachada principal"
    }
  ],
  "owner": {
    "name": "Juan Pérez",
    "phone": "+57 300 123 4567",
    "email": "juan@email.com",
    "photo": ""
  },
  "traces": [
    {
      "dateSale": "2024-01-15",
      "name": "Primera venta",
      "value": 450000000,
      "tax": 45000000
    }
  ],
  "city": "Bogotá",
  "state": "Cundinamarca",
  "country": "Colombia"
}
```

#### Update Property

```bash
PUT /api/Property/{id}
Content-Type: application/json

{
  "name": "Casa Moderna Renovada",
  "price": 550000000
}
```

#### Delete Property

```bash
DELETE /api/Property/{id}
```

---

### 📸 **IMAGE ENDPOINTS**

#### Upload Image (Universal - works for both general and property images)

```bash
# For Property Image (RECOMMENDED):
POST /api/Image/upload
Content-Type: multipart/form-data

FormData:
- file: image.jpg
- propertyId: property_id_here
- description: "Fachada principal"
- isMain: true

# For General Image:
POST /api/Image/upload
Content-Type: multipart/form-data

FormData:
- file: image.jpg
```

#### Upload Property Image (Alternative specific endpoint)

```bash
POST /api/Image/property/{propertyId}/upload
Content-Type: multipart/form-data

FormData:
- file: image.jpg
- description: "Fachada principal"
- isMain: true
```

#### Get Property Images

```bash
GET /api/Image/property/{propertyId}
GET /api/Image/property/{propertyId}?enabledOnly=false
```

#### Set Main Image

```bash
PUT /api/Image/property/{propertyId}/main/{imageId}
```

#### Delete Property Image

```bash
DELETE /api/Image/property/{propertyId}/image/{imageId}
```

#### Get Responsive Image URLs

```bash
GET /api/Image/property/{propertyId}/image/{imageId}/responsive

# Returns multiple sizes: thumbnail, small, medium, large, original
```

#### Upload Multiple Images

```bash
POST /api/Image/upload-multiple
Content-Type: multipart/form-data

FormData:
- files[]: image1.jpg
- files[]: image2.jpg
- files[]: image3.jpg
```

#### Delete Image from Cloudinary

```bash
DELETE /api/Image/{publicId}
```

#### Generate Image URL with transformations

```bash
GET /api/Image/url/{publicId}?width=800&height=600&format=webp
```

---

### 📊 **PROPERTY TRACE ENDPOINTS**

#### Create Property Trace

```bash
POST /api/PropertyTrace
Content-Type: application/json

{
  "idProperty": "property_id_here",
  "dateSale": "2024-01-15",
  "name": "Primera venta",
  "value": 450000000,
  "tax": 45000000
}
```

#### Get All Traces

```bash
GET /api/PropertyTrace
```

#### Get Traces by Property

```bash
GET /api/PropertyTrace/property/{propertyId}
```

#### Get Trace by ID

```bash
GET /api/PropertyTrace/{id}
```

#### Update Trace

```bash
PUT /api/PropertyTrace/{id}
Content-Type: application/json

{
  "dateSale": "2024-02-15",
  "name": "Venta actualizada",
  "value": 480000000,
  "tax": 48000000
}
```

#### Delete Trace

```bash
DELETE /api/PropertyTrace/{id}
```

#### Delete All Traces for Property

```bash
DELETE /api/PropertyTrace/property/{propertyId}
```

---

## 🎯 **COMPLETE WORKFLOW EXAMPLE**

### Step 1: Create Owner

```bash
curl -k -X POST https://localhost:7007/api/Owner \
  -H "Content-Type: application/json" \
  -d '{
    "name": "María",
    "lastName": "González",
    "email": "maria@email.com",
    "phone": "+57 300 555 1234"
  }'
```

### Step 2: Create Property

```bash
curl -k -X POST https://localhost:7007/api/Property \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Villa Moderna",
    "address": "Calle Principal #123",
    "price": 600000000,
    "idOwner": "OWNER_ID_FROM_STEP_1",
    "codigoInternal": "VM001",
    "year": 2024,
    "city": "Medellín",
    "state": "Antioquia",
    "country": "Colombia"
  }'
```

### Step 3: Upload Property Images

```bash
# Main image
curl -k -X POST https://localhost:7007/api/Image/upload \
  -F "file=@fachada.jpg" \
  -F "propertyId=PROPERTY_ID_FROM_STEP_2" \
  -F "description=Fachada principal" \
  -F "isMain=true"

# Additional images
curl -k -X POST https://localhost:7007/api/Image/upload \
  -F "file=@interior.jpg" \
  -F "propertyId=PROPERTY_ID_FROM_STEP_2" \
  -F "description=Interior elegante" \
  -F "isMain=false"
```

### Step 4: Add Property Trace

```bash
curl -k -X POST https://localhost:7007/api/PropertyTrace \
  -H "Content-Type: application/json" \
  -d '{
    "idProperty": "PROPERTY_ID_FROM_STEP_2",
    "dateSale": "2024-01-15",
    "name": "Venta inicial",
    "value": 580000000,
    "tax": 58000000
  }'
```

### Step 5: Get Complete Property Info

```bash
curl -k -X GET https://localhost:7007/api/Property/PROPERTY_ID_FROM_STEP_2
```

---

## 🔧 **TESTING COMMANDS**

### Test Server Connection

```bash
curl -k -X GET https://localhost:7007/api/Owner
```

### Upload Test Image

```bash
curl -k -X POST https://localhost:7007/api/Image/upload \
  -F "file=@test-image.jpg" \
  -F "propertyId=YOUR_PROPERTY_ID" \
  -F "description=Test image" \
  -F "isMain=true"
```

### Check Property with All Data

```bash
curl -k -X GET https://localhost:7007/api/Property/YOUR_PROPERTY_ID
```

---

## 📋 **RESPONSE FORMATS**

### Property Response (Complete)

```json
{
  "id": "67890abc123def456789",
  "name": "Villa Moderna",
  "address": "Calle Principal #123",
  "price": 600000000,
  "images": [
    {
      "idPropertyImage": "img123",
      "file": "https://res.cloudinary.com/miguedev/image/upload/v123/fachada.jpg",
      "enabled": true,
      "isMain": true,
      "description": "Fachada principal"
    }
  ],
  "owner": {
    "name": "María González",
    "phone": "+57 300 555 1234",
    "email": "maria@email.com",
    "photo": ""
  },
  "traces": [
    {
      "dateSale": "2024-01-15",
      "name": "Venta inicial",
      "value": 580000000,
      "tax": 58000000
    }
  ],
  "codigoInternal": "VM001",
  "year": 2024,
  "createdAt": "2024-10-27T23:00:00.000Z",
  "city": "Medellín",
  "state": "Antioquia",
  "country": "Colombia"
}
```

### Image Upload Response

```json
{
  "id": "img123",
  "propertyId": "prop123",
  "publicId": "real-estate/properties/fachada_abc123",
  "url": "https://res.cloudinary.com/miguedev/image/upload/v123/fachada.jpg",
  "width": 800,
  "height": 600,
  "format": "jpg",
  "bytes": 125000,
  "description": "Fachada principal",
  "isMain": true,
  "enabled": true,
  "createdAt": "2024-10-27T23:00:00.000Z"
}
```

---

## ⚠️ **IMPORTANT NOTES**

1. **Use HTTPS**: All endpoints use `https://localhost:7007`
2. **Use `-k` flag**: For curl commands to ignore SSL certificate warnings
3. **Main Image**: Only one image per property can be `isMain: true`
4. **File Upload**: Use `multipart/form-data` for image uploads
5. **Property ID**: Required for associating images with properties
6. **Cloudinary**: Images are stored in `real-estate/properties` folder
7. **MongoDB**: Property images are stored in `PropertyImages` collection
8. **Responsive URLs**: Available for all uploaded images

## 🎯 **RECOMMENDED ENDPOINT FOR PROPERTY IMAGES**

**Use this for property image uploads:**

```bash
POST /api/Image/upload
-F "file=@image.jpg"
-F "propertyId=YOUR_PROPERTY_ID"
-F "description=Description here"
-F "isMain=true/false"
```

This single endpoint handles both Cloudinary upload AND database storage! 🚀
