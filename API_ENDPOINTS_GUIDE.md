<!-- @format -->

# 🚀 API Endpoints Guide - Sistema Completo

**Base URL**: `https://localhost:7007/api`

## 📋 **Resumen de Endpoints Disponibles**

| Controlador | Endpoints | Descripción |
|-------------|-----------|-------------|
| **Property** | 8 endpoints | Gestión completa de propiedades |
| **Owner** | 6 endpoints | Gestión de propietarios |
| **PropertyTrace** | 6 endpoints | Historial de transacciones |
| **Image** | 12 endpoints | Gestión de imágenes y Cloudinary |

---

## 🏠 **Property Controller** `/api/Property`

### **GET Endpoints**
```http
GET /api/Property                    # Listar todas las propiedades (paginado)
GET /api/Property/{id}               # Obtener propiedad por ID (con images, owner, traces)
GET /api/Property/owner/{ownerId}    # Propiedades por propietario
GET /api/Property/price-range        # Filtrar por rango de precio
```

### **POST Endpoints**
```http
POST /api/Property                   # Crear nueva propiedad
```

### **PUT Endpoints**
```http
PUT /api/Property/{id}               # Actualizar propiedad completa
```

### **DELETE Endpoints**
```http
DELETE /api/Property/{id}            # Eliminar propiedad (y dependencias)
```

### **Respuesta Completa de Property**
```json
{
  "id": "671234567890abcdef123999",
  "name": "Casa Moderna Premium",
  "address": "Carrera 15 #93-45",
  "price": 850000000,
  "images": [
    {
      "idPropertyImage": "img_001",
      "file": "https://res.cloudinary.com/demo/...",
      "enabled": true,
      "isMain": true,
      "description": "Fachada principal"
    }
  ],
  "owner": {
    "name": "Juan Carlos Pérez García",
    "photo": "https://res.cloudinary.com/...",
    "phone": "+57 300 123 4567",
    "email": "juan.perez@email.com"
  },
  "traces": [
    {
      "dateSale": "2024-01-15",
      "name": "Venta inicial",
      "value": 850000000,
      "tax": 85000000
    }
  ],
  "codigoInternal": "PROP_001",
  "year": 2024,
  "createdAt": "2025-10-27T15:30:00Z",
  "city": "Bogotá",
  "state": "Cundinamarca",
  "country": "Colombia"
}
```

---

## � **Owner Controller** `/api/Owner`

### **GET Endpoints**
```http
GET /api/Owner                       # Listar todos los propietarios
GET /api/Owner/{id}                  # Obtener propietario por ID
```

### **POST Endpoints**
```http
POST /api/Owner                      # Crear nuevo propietario
```

### **PUT Endpoints**
```http
PUT /api/Owner/{id}                  # Actualizar propietario
```

### **DELETE Endpoints**
```http
DELETE /api/Owner/{id}               # Eliminar propietario
```

### **Ejemplo de Owner**
```json
{
  "id": "671234567890abcdef123456",
  "name": "Juan Carlos",
  "lastName": "Pérez García",
  "phone": "+57 300 123 4567",
  "photo": "https://res.cloudinary.com/...",
  "birthday": "1980-05-15",
  "email": "juan.perez@email.com",
  "fullName": "Juan Carlos Pérez García",
  "createdAt": "2025-10-27T15:30:00Z"
}
```

---

## 📊 **PropertyTrace Controller** `/api/PropertyTrace`

### **GET Endpoints**
```http
GET /api/PropertyTrace                     # Listar todos los traces
GET /api/PropertyTrace/{id}                # Obtener trace por ID
GET /api/PropertyTrace/property/{propertyId} # Traces de una propiedad
```

### **POST Endpoints**
```http
POST /api/PropertyTrace                    # Crear nuevo trace
```

### **PUT Endpoints**
```http
PUT /api/PropertyTrace/{id}                # Actualizar trace
```

### **DELETE Endpoints**
```http
DELETE /api/PropertyTrace/{id}             # Eliminar trace específico
DELETE /api/PropertyTrace/property/{propertyId} # Eliminar todos los traces de una propiedad
```

### **Ejemplo de PropertyTrace**
```json
{
  "id": "671234567890abcdef123789",
  "idProperty": "671234567890abcdef123999",
  "dateSale": "2024-01-15T00:00:00Z",
  "name": "Venta inicial",
  "value": 850000000,
  "tax": 85000000,
  "createdAt": "2025-10-27T15:30:00Z"
}
```

---

## 🖼️ **Image Controller** `/api/Image`

### **Upload Endpoints**
```http
POST /api/Image/upload                     # Upload general (con/sin propertyId)
POST /api/Image/upload-multiple           # Upload múltiples imágenes
POST /api/Image/property/{propertyId}/upload # Upload específico para propiedad
```

### **Property Image Management**
```http
GET /api/Image/property/{propertyId}       # Obtener imágenes de propiedad
PUT /api/Image/property/{propertyId}/main/{imageId} # Establecer imagen principal
DELETE /api/Image/property/{propertyId}/image/{imageId} # Eliminar imagen específica
GET /api/Image/property/{propertyId}/image/{imageId}/responsive # URLs responsivas
```

### **General Image Operations**
```http
DELETE /api/Image/{publicId}               # Eliminar imagen por publicId
GET /api/Image/url/{publicId}             # Generar URL con transformaciones
GET /api/Image/responsive/{publicId}       # URLs responsivas
```

### **Parámetros de Upload**
```http
POST /api/Image/upload
Content-Type: multipart/form-data

file: [archivo]
propertyId: "671234..." (opcional)
description: "Descripción" (opcional)
isMain: true/false (opcional)
folder: "custom-folder" (opcional)
```

### **Respuesta de Upload**
```json
{
  "id": "img_123456",
  "propertyId": "671234567890abcdef123999",
  "publicId": "real-estate/properties/imagen_abc123",
  "url": "https://res.cloudinary.com/miguedev/image/upload/v1234567890/...",
  "width": 1920,
  "height": 1080,
  "format": "jpg",
  "bytes": 245678,
  "description": "Fachada principal",
  "isMain": true,
  "enabled": true,
  "createdAt": "2025-10-27T15:30:00Z"
}
```

---

## � **Ejemplos de Uso Completo**

### **1. Crear Propietario con Foto**
```bash
# 1. Crear propietario
curl -k -X POST https://localhost:7007/api/Owner \
  -H "Content-Type: application/json" \
  -d '{
    "name": "María Elena",
    "lastName": "González Ruiz",
    "phone": "+57 315 555 1234",
    "email": "maria.gonzalez@email.com",
    "birthday": "1985-03-20"
  }'

# 2. Subir foto del propietario (usar ID del paso 1)
curl -k -X POST https://localhost:7007/api/Image/upload \
  -F "file=@owner-photo.jpg" \
  -F "folder=owners"
```

### **2. Crear Propiedad Completa**
```bash
# 1. Crear propiedad
curl -k -X POST https://localhost:7007/api/Property \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Apartamento Lujoso Centro",
    "address": "Carrera 11 #85-32, Zona Rosa",
    "price": 580000000,
    "codigoInternal": "APART_001",
    "year": 2023,
    "city": "Bogotá",
    "state": "Cundinamarca",
    "country": "Colombia",
    "idOwner": "OWNER_ID_DEL_PASO_ANTERIOR"
  }'

# 2. Subir imágenes de la propiedad
curl -k -X POST https://localhost:7007/api/Image/property/PROPERTY_ID/upload \
  -F "file=@fachada.jpg" \
  -F "description=Fachada del edificio" \
  -F "isMain=true"

curl -k -X POST https://localhost:7007/api/Image/property/PROPERTY_ID/upload \
  -F "file=@sala.jpg" \
  -F "description=Sala de estar" \
  -F "isMain=false"

# 3. Agregar historial de transacciones
curl -k -X POST https://localhost:7007/api/PropertyTrace \
  -H "Content-Type: application/json" \
  -d '{
    "idProperty": "PROPERTY_ID",
    "dateSale": "2024-01-15",
    "name": "Venta inicial",
    "value": 580000000,
    "tax": 58000000
  }'
```

### **3. Obtener Propiedad Completa**
```bash
curl -k -X GET https://localhost:7007/api/Property/PROPERTY_ID
```

---

## ⚡ **Características Importantes**

### **🔗 Relaciones Automáticas**
- Al obtener una Property, automáticamente incluye Owner, Images y Traces
- Al eliminar una Property, se eliminan automáticamente sus Images y Traces relacionados

### **🖼️ Integración con Cloudinary**
- Upload automático a Cloudinary con URLs optimizadas
- Generación de URLs responsivas (thumbnail, small, medium, large)
- Eliminación automática de Cloudinary al eliminar de DB

### **📄 Paginación**
- GET /api/Property soporta paginación: `?page=1&pageSize=10`
- Respuesta incluye: `totalCount`, `totalPages`, `hasNextPage`, `hasPreviousPage`

### **🔍 Filtros Avanzados**
- Búsqueda por propietario: `/api/Property/owner/{ownerId}`
- Filtro por precio: `/api/Property/price-range?minPrice=100000&maxPrice=500000`

---

## 🎯 **Estado Actual del Sistema**

✅ **Sistema Completamente Funcional**
- 4 Controladores operativos
- 32 Endpoints totales
- Base de datos limpia y lista para usar
- Integración completa con Cloudinary
- Documentación completa actualizada
- Repository Pattern implementado
- DTOs optimizados para Frontend

🚀 **Listo para Producción**

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
