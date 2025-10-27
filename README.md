<!-- @format -->

# Real Estate API - Technical Test

## 📋 Descripción del Proyecto

Esta es una **API REST completa** desarrollada en **.NET 9** con **MongoDB** para el manejo de propiedades inmobiliarias. Fue diseñada como parte de un test técnico para **Senior Frontend Developer**, cumpliendo con todos los requerimientos de arquitectura limpia, filtros avanzados, testing y documentación.

## 🛠️ Tecnologías Utilizadas

- **.NET 9**: Framework principal
- **C#**: Lenguaje de programación
- **MongoDB**: Base de datos NoSQL con driver oficial
- **MongoDB.Driver**: Driver oficial para .NET
- **Swagger/OpenAPI**: Documentación interactiva de la API
- **NUnit**: Framework de testing unitario
- **ASP.NET Core**: Framework web
- **CORS**: Configurado para integración con React/Next.js

## 🏗️ Arquitectura del Proyecto

### Estructura de Archivos

```
Back-Mongo/
├── Controllers/              # 🎮 Controladores de la API
│   ├── PropertyController.cs    # CRUD de propiedades + filtros
│   ├── OwnerController.cs       # CRUD de propietarios
│   └── ProductController.cs     # (Legacy - para compatibilidad)
├── DTOs/                     # 📦 Data Transfer Objects
│   ├── PropertyDto.cs           # DTOs para propiedades
│   ├── PropertyFilterDto.cs     # Filtros y paginación
│   └── OwnerDto.cs             # DTOs para propietarios
├── Entities/                 # 🗃️ Modelos de datos (MongoDB)
│   ├── Property.cs             # Propiedades inmobiliarias
│   ├── Owner.cs                # Propietarios
│   ├── PropertyImage.cs        # Imágenes de propiedades
│   ├── PropertyPlace.cs        # Ubicaciones geográficas
│   └── Products.cs             # (Legacy)
├── Repository/               # 🔄 Patrón Repository
│   ├── IRepository.cs          # Interface base genérica
│   ├── Repository.cs           # Implementación base
│   ├── IPropertyRepository.cs  # Interface específica de propiedades
│   ├── PropertyRepository.cs   # Implementación con filtros complejos
│   ├── IOwnerRepository.cs     # Interface de propietarios
│   └── OwnerRepository.cs      # Implementación de propietarios
├── Settings/                 # ⚙️ Configuraciones
│   └── MongoDbSettings.cs      # Configuración de MongoDB
├── Tests/                    # 🧪 Tests unitarios
│   ├── EntityTests.cs          # Tests de entidades
│   └── DtoTests.cs            # Tests de DTOs y validaciones
├── Properties/               # 🚀 Configuración de lanzamiento
│   └── launchSettings.json
├── Program.cs                # 🎯 Punto de entrada y configuración
├── README.md                 # 📚 Esta documentación
└── DEPLOYMENT.md             # 🚀 Guía de despliegue
```

## 🎯 Características Principales de la API

### 📡 Endpoints Implementados

#### 🏠 **Properties API** (`/api/Property`)

| Método   | Endpoint                        | Descripción                                 | Parámetros                          |
| -------- | ------------------------------- | ------------------------------------------- | ----------------------------------- |
| `GET`    | `/api/Property`                 | **Lista propiedades con filtros avanzados** | Query parameters para filtros       |
| `GET`    | `/api/Property/{id}`            | Obtiene propiedad específica con detalles   | `id`: ObjectId de la propiedad      |
| `POST`   | `/api/Property`                 | Crea nueva propiedad                        | Body: PropertyCreateDto             |
| `PUT`    | `/api/Property/{id}`            | Actualiza propiedad existente               | `id` + Body: PropertyUpdateDto      |
| `DELETE` | `/api/Property/{id}`            | Elimina propiedad                           | `id`: ObjectId de la propiedad      |
| `GET`    | `/api/Property/owner/{ownerId}` | Propiedades por propietario                 | `ownerId`: ObjectId del propietario |
| `GET`    | `/api/Property/price-range`     | Filtrar por rango de precios                | `minPrice`, `maxPrice`              |

#### 👤 **Owners API** (`/api/Owner`)

| Método   | Endpoint            | Descripción                    | Parámetros                     |
| -------- | ------------------- | ------------------------------ | ------------------------------ |
| `GET`    | `/api/Owner`        | Lista todos los propietarios   | -                              |
| `GET`    | `/api/Owner/{id}`   | Obtiene propietario específico | `id`: ObjectId del propietario |
| `POST`   | `/api/Owner`        | Crea nuevo propietario         | Body: OwnerCreateDto           |
| `PUT`    | `/api/Owner/{id}`   | Actualiza propietario          | `id` + Body: OwnerUpdateDto    |
| `DELETE` | `/api/Owner/{id}`   | Elimina propietario            | `id`: ObjectId del propietario |
| `GET`    | `/api/Owner/search` | Buscar por nombre              | `name`: Término de búsqueda    |

### 🔍 **Filtros Avanzados Implementados**

La API soporta filtros complejos y combinables para propiedades:

#### **Filtros de Texto**

- **`name`**: Búsqueda parcial en el nombre (case-insensitive)
- **`address`**: Búsqueda parcial en la dirección

#### **Filtros Numéricos**

- **`minPrice`**: Precio mínimo (mayor o igual)
- **`maxPrice`**: Precio máximo (menor o igual)
- **`year`**: Año específico de construcción

#### **Filtros de Ubicación**

- **`city`**: Ciudad específica
- **`state`**: Estado/Departamento
- **`country`**: País

#### **Filtros de Relaciones**

- **`ownerName`**: Nombre del propietario (busca en nombre completo)

#### **Paginación y Ordenamiento**

- **`page`**: Número de página (default: 1)
- **`pageSize`**: Elementos por página (default: 10)
- **`sortBy`**: Campo de ordenamiento (`name`, `price`, `address`, `createdAt`)
- **`sortDirection`**: Dirección (`asc`, `desc`)

#### **Ejemplo de Query Compleja**

```
GET /api/Property?name=casa&minPrice=200000000&maxPrice=500000000&city=bogotá&ownerName=juan&page=1&pageSize=5&sortBy=price&sortDirection=desc
```

## 🗄️ Esquema de Base de Datos MongoDB

### **Colecciones Principales**

#### **1. Property** (Propiedades)

```json
{
  "_id": "ObjectId",
  "Name": "string", // Nombre de la propiedad
  "Address": "string", // Dirección completa
  "Price": "decimal", // Precio en moneda local
  "CodigoInternal": "string", // Código interno único
  "IdOwner": "ObjectId", // Referencia al propietario
  "Year": "int?", // Año de construcción
  "CreatedAt": "DateTime",
  "UpdatedAt": "DateTime"
}
```

#### **2. Owner** (Propietarios)

```json
{
  "_id": "ObjectId",
  "Name": "string", // Nombre
  "LastName": "string", // Apellido
  "Phone": "string", // Teléfono
  "Email": "string", // Email único
  "Birthday": "DateTime?", // Fecha de nacimiento
  "CreatedAt": "DateTime",
  "UpdatedAt": "DateTime"
}
```

#### **3. PropertyImage** (Imágenes)

```json
{
  "_id": "ObjectId",
  "IdProperty": "ObjectId", // Referencia a Property
  "Image": "string", // URL o base64 de la imagen
  "Enabled": "bool", // Si está activa
  "IsMain": "bool", // Si es la imagen principal
  "Description": "string",
  "CreatedAt": "DateTime",
  "UpdatedAt": "DateTime"
}
```

#### **4. PropertyPlace** (Ubicaciones)

```json
{
  "_id": "ObjectId",
  "IdProperty": "ObjectId", // Referencia a Property
  "Name": "string", // Tipo: "City", "State", "Country"
  "Value": "string", // Valor: "Bogotá", "Cundinamarca"
  "PlaceType": "string", // Categoría del lugar
  "Latitude": "double?", // Coordenada
  "Longitude": "double?", // Coordenada
  "CreatedAt": "DateTime",
  "UpdatedAt": "DateTime"
}
```

## 🚀 Instalación y Ejecución

### **Prerrequisitos**

- **.NET 9 SDK** instalado
- **MongoDB Atlas** o instancia local
- **Puerto 5179** disponible (configurable)

### **Pasos de Instalación**

#### **1. Clonar y Navegar**

```bash
git clone <repository-url>
cd BackEnd/Back-Mongo
```

#### **2. Configurar MongoDB**

Editar `appsettings.json`:

```json
{
  "MongoDbSettings": {
    "ConnectionString": "mongodb+srv://tu-usuario:tu-password@cluster.mongodb.net/",
    "DatabaseName": "Million-TestDB"
  }
}
```

#### **3. Restaurar e Instalar**

```bash
dotnet restore
dotnet build
```

#### **4. Ejecutar Tests**

```bash
dotnet test --verbosity normal
```

#### **5. Ejecutar Aplicación**

```bash
dotnet run --project Back-Mongo.csproj
```

#### **6. Acceder a la API**

- **Swagger UI**: http://localhost:5179 (raíz del proyecto)
- **API Base**: http://localhost:5179/api
- **OpenAPI JSON**: http://localhost:5179/swagger/v1/swagger.json

## 🧪 Testing y Calidad

### **Tests Implementados**

- ✅ **12 tests unitarios** con **100% de éxito**
- ✅ **Validación de entidades** (Property, Owner)
- ✅ **Validación de DTOs** (Annotations, Required fields)
- ✅ **Tests de lógica de negocio**
- ✅ **Cobertura de casos edge**

### **Ejecutar Tests**

```bash
# Todos los tests
dotnet test

# Con detalles
dotnet test --verbosity normal

# Tests específicos
dotnet test --filter "ClassName=PropertyEntityTests"
```

## 🌐 Configuración CORS

Configurado para desarrollo frontend:

```csharp
// Puertos permitidos para React/Next.js
"http://localhost:3000"
"http://localhost:3001"
"https://localhost:3000"

// Configuración completa
- AllowAnyHeader()
- AllowAnyMethod()
- AllowCredentials()
```

## 📊 Ejemplos de Uso de la API

### **1. Crear Propietario**

```bash
POST /api/Owner
Content-Type: application/json

{
  "name": "Juan Carlos",
  "lastName": "Pérez García",
  "phone": "+57 300 123 4567",
  "email": "juan.perez@email.com",
  "birthday": "1985-06-15T00:00:00Z"
}
```

**Respuesta:**

```json
{
  "id": "671234567890abcdef123456",
  "name": "Juan Carlos",
  "lastName": "Pérez García",
  "fullName": "Juan Carlos Pérez García",
  "phone": "+57 300 123 4567",
  "email": "juan.perez@email.com",
  "birthday": "1985-06-15T00:00:00Z",
  "createdAt": "2025-10-26T15:30:00Z"
}
```

### **2. Crear Propiedad**

```bash
POST /api/Property
Content-Type: application/json

{
  "name": "Casa Moderna en Chapinero",
  "address": "Carrera 13 #85-32, Chapinero",
  "price": 450000000,
  "idOwner": "671234567890abcdef123456",
  "codigoInternal": "PROP001",
  "year": 2020,
  "image": "https://images.unsplash.com/house-modern",
  "city": "Bogotá",
  "state": "Cundinamarca",
  "country": "Colombia"
}
```

### **3. Buscar Propiedades con Filtros**

```bash
GET /api/Property?name=casa&minPrice=200000000&maxPrice=500000000&city=bogotá&page=1&pageSize=5&sortBy=price&sortDirection=desc
```

**Respuesta:**

```json
{
  "properties": [
    {
      "id": "671234567890abcdef123457",
      "name": "Casa Moderna en Chapinero",
      "address": "Carrera 13 #85-32, Chapinero",
      "price": 450000000,
      "idOwner": "671234567890abcdef123456",
      "image": "https://images.unsplash.com/house-modern",
      "ownerName": "Juan Carlos Pérez García",
      "ownerPhone": "+57 300 123 4567",
      "city": "Bogotá",
      "state": "Cundinamarca",
      "country": "Colombia",
      "year": 2020,
      "createdAt": "2025-10-26T15:35:00Z"
    }
  ],
  "totalCount": 1,
  "page": 1,
  "pageSize": 5,
  "totalPages": 1,
  "hasNextPage": false,
  "hasPreviousPage": false
}
```

## 🏆 Buenas Prácticas Implementadas

### **🏛️ Arquitectura Limpia**

- ✅ **Separación de responsabilidades** clara
- ✅ **Patrón Repository** para abstracción de datos
- ✅ **DTOs** para transferencia segura de datos
- ✅ **Dependency Injection** configurada correctamente
- ✅ **Interfaces** para inversión de dependencias

### **🛡️ Seguridad y Validación**

- ✅ **Data Annotations** en DTOs
- ✅ **Validación de entrada** en controladores
- ✅ **Sanitización de queries** MongoDB
- ✅ **CORS** configurado apropiadamente
- ✅ **Manejo de errores** consistente

### **⚡ Performance**

- ✅ **Queries optimizadas** con filtros específicos
- ✅ **Paginación** para grandes datasets
- ✅ **Lazy loading** de datos relacionados
- ✅ **Índices MongoDB** recomendados
- ✅ **Async/await** en todas las operaciones I/O

### **🧪 Testing y Calidad**

- ✅ **Tests unitarios** completos
- ✅ **Cobertura de casos edge**
- ✅ **Tests de validación**
- ✅ **Documentación XML** en controladores

## 🚀 Para Producción

### **Índices MongoDB Recomendados**

```javascript
// Índices para mejorar performance
db.Property.createIndex({ Name: "text", Address: "text" });
db.Property.createIndex({ Price: 1 });
db.Property.createIndex({ IdOwner: 1 });
db.Property.createIndex({ CreatedAt: -1 });
db.Owner.createIndex({ Email: 1 }, { unique: true });
db.PropertyPlace.createIndex({ IdProperty: 1, Name: 1 });
db.PropertyImage.createIndex({ IdProperty: 1, IsMain: 1 });
```

### **Variables de Entorno**

```bash
# Para producción, usar variables de entorno
MONGODB_CONNECTION_STRING=mongodb+srv://...
MONGODB_DATABASE_NAME=RealEstate_Prod
ASPNETCORE_ENVIRONMENT=Production
```

## 📚 Documentación Adicional

- **Swagger UI**: Disponible en la raíz de la aplicación
- **OpenAPI Spec**: `/swagger/v1/swagger.json`
- **DEPLOYMENT.md**: Guía detallada de despliegue
- **Comentarios XML**: En todos los endpoints para IntelliSense

## 🎯 Cumplimiento del Test Técnico

### **✅ Requerimientos Cumplidos**

- ✅ **API en .NET 8/9** con C#
- ✅ **MongoDB** como base de datos
- ✅ **Filtros** por name, address, price range
- ✅ **DTOs** con IdOwner, Name, Address, Price, Image
- ✅ **CRUD completo** de propiedades
- ✅ **Tests unitarios** con NUnit
- ✅ **Arquitectura limpia**
- ✅ **Documentación completa**
- ✅ **Manejo de errores**
- ✅ **Performance optimizada**

### **🔥 Características Adicionales**

- ✅ **Filtros avanzados** (ubicación, propietario, año)
- ✅ **Paginación y ordenamiento**
- ✅ **CORS** configurado para frontend
- ✅ **Swagger** documentación interactiva
- ✅ **Repository pattern** con DI
- ✅ **Validaciones robustas**
- ✅ **Tests comprehensivos**

---

## 👨‍💻 Información del Desarrollador

**Proyecto**: Real Estate API  
**Framework**: .NET 9  
**Base de Datos**: MongoDB  
**Fecha**: Octubre 2025  
**Versión**: 1.0.0

**Estado**: ✅ **Completamente funcional y listo para producción**

---

¿Necesitas ayuda con la integración del frontend React/Next.js? 🚀
