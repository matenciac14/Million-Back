<!-- @format -->

# 📋 Estructura de Respuesta Actualizada

## ✅ **Respuesta del GET /api/Property/{id}**

Tu frontend ahora recibirá exactamente la estructura que esperas:

```json
{
  "id": "671234567890abcdef123999",
  "name": "Casa Moderna Premium",
  "address": "Carrera 15 #93-45, Zona Rosa",
  "price": 850000000,
  "images": [
    {
      "idPropertyImage": "img_001",
      "file": "https://res.cloudinary.com/demo/image/upload/v1234567890/fachada.jpg",
      "enabled": true,
      "isMain": true,
      "description": "Fachada principal"
    },
    {
      "idPropertyImage": "img_002",
      "file": "https://res.cloudinary.com/demo/image/upload/v1234567890/sala.jpg",
      "enabled": true,
      "isMain": false,
      "description": "Sala de estar"
    },
    {
      "idPropertyImage": "img_003",
      "file": "https://res.cloudinary.com/demo/image/upload/v1234567890/habitacion.jpg",
      "enabled": true,
      "isMain": false,
      "description": "Habitación principal"
    }
  ],
  "owner": {
    "name": "Juan Carlos Pérez García",
    "photo": "https://res.cloudinary.com/demo/image/upload/v1234567890/owner-photo.jpg",
    "phone": "+57 300 123 4567",
    "email": "juan.perez@email.com"
  },
  "traces": [
    {
      "dateSale": "2024-01-15",
      "name": "Venta inicial",
      "value": 850000000,
      "tax": 85000000
    },
    {
      "dateSale": "2023-12-01",
      "name": "Avalúo comercial",
      "value": 800000000,
      "tax": 80000000
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

## ✅ **Nuevas Entidades Creadas**

### **PropertyTrace**

```csharp
public class PropertyTrace
{
    public string Id { get; set; }
    public string IdProperty { get; set; }
    public DateTime DateSale { get; set; }
    public string Name { get; set; }
    public decimal Value { get; set; }
    public decimal Tax { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
```

### **Owner (Actualizado)**

```csharp
public class Owner
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Phone { get; set; }
    public string Photo { get; set; } // ✅ NUEVO CAMPO
    public DateTime? Birthday { get; set; }
    public string Email { get; set; }
    public string FullName => $"{Name} {LastName}".Trim();
}
```

## ✅ **DTOs Actualizados**

### **PropertyDto (Completamente nuevo)**

```csharp
public class PropertyDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public decimal Price { get; set; }
    public List<PropertyImageDto> Images { get; set; } = new();
    public PropertyOwnerDto Owner { get; set; } = new();
    public List<PropertyTraceDto> Traces { get; set; } = new();
    public string CodigoInternal { get; set; }
    public int? Year { get; set; }
    public DateTime CreatedAt { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Country { get; set; }
}
```

### **PropertyImageDto**

```csharp
public class PropertyImageDto
{
    public string IdPropertyImage { get; set; }
    public string File { get; set; }
    public bool Enabled { get; set; }
    public bool IsMain { get; set; }
    public string Description { get; set; }
}
```

### **PropertyOwnerDto**

```csharp
public class PropertyOwnerDto
{
    public string Name { get; set; }
    public string Photo { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
}
```

### **PropertyTraceDto**

```csharp
public class PropertyTraceDto
{
    public string DateSale { get; set; } // Format: "yyyy-MM-dd"
    public string Name { get; set; }
    public decimal Value { get; set; }
    public decimal Tax { get; set; }
}
```

## ✅ **Nuevos Repositorios**

### **IPropertyTraceRepository**

```csharp
public interface IPropertyTraceRepository : IRepository<PropertyTrace>
{
    Task<List<PropertyTrace>> GetByPropertyIdAsync(string propertyId);
    Task DeleteByPropertyIdAsync(string propertyId);
}
```

## ✅ **Endpoints Actualizados**

- `GET /api/Property/{id}` → Devuelve PropertyDto con images, owner y traces completos
- `GET /api/Property` → Lista de propiedades con la nueva estructura
- `GET /api/Property/owner/{ownerId}` → Propiedades por propietario con estructura completa
- `GET /api/Property/price-range` → Búsqueda por precio con estructura completa

## 🚀 **Compatibilidad**

Tu frontend ahora recibirá exactamente la estructura que esperas:

- ✅ `images` array con `idPropertyImage` y `file`
- ✅ `owner` object con `name` y `photo`
- ✅ `traces` array con `dateSale`, `name`, `value`, `tax`
- ✅ Todos los campos originales de Property mantienen su estructura

## 📝 **Próximos Pasos**

1. **Crear PropertyTrace**: Usar POST /api/PropertyTrace (endpoint a crear)
2. **Subir foto de Owner**: Usar endpoints de imagen
3. **Asociar múltiples imágenes**: Usar endpoints de PropertyImage ya existentes
