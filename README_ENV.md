<!-- @format -->

# 🏠 Real Estate API - Backend

Una API completa para gestión de propiedades inmobiliarias construida con .NET 9, MongoDB y Cloudinary.

## 🚀 Características

- **CRUD completo** para propiedades y propietarios
- **Filtros avanzados** con paginación
- **Búsqueda** por múltiples criterios
- **Gestión de imágenes** con Cloudinary (subida, optimización, eliminación)
- **URLs responsivas** automáticas para diferentes dispositivos
- **Arquitectura limpia** con Repository Pattern
- **Validaciones robustas** y manejo de errores
- **Documentación automática** con Swagger/OpenAPI

## 🛠️ Tecnologías

- **.NET 9** - Framework principal
- **MongoDB** - Base de datos NoSQL
- **Cloudinary** - Gestión de imágenes en la nube
- **ASP.NET Core** - Web API
- **NUnit** - Testing framework
- **Swagger/OpenAPI** - Documentación de API

## 📋 Requisitos Previos

- .NET 9 SDK
- MongoDB Atlas account (o instancia local)
- Cloudinary account
- Visual Studio Code o Visual Studio

## ⚙️ Configuración

### 1. Clonar el proyecto

```bash
git clone <repository-url>
cd BackEnd/Back-Mongo
```

### 2. Configurar Variables de Entorno

Copia el archivo `.env.example` a `.env` y configura tus credenciales:

```bash
cp .env.example .env
```

Edita el archivo `.env` con tus valores reales:

```env
# MongoDB Configuration
MONGODB_CONNECTION_STRING=tu_connection_string_mongodb
MONGODB_DATABASE_NAME=tu_nombre_base_datos

# Cloudinary Configuration
CLOUDINARY_CLOUD_NAME=tu_cloud_name
CLOUDINARY_API_KEY=tu_api_key
CLOUDINARY_API_SECRET=tu_api_secret
CLOUDINARY_BASE_URL=https://res.cloudinary.com
CLOUDINARY_SECURE_URL=https://res.cloudinary.com

# Development Environment
ASPNETCORE_ENVIRONMENT=Development
ASPNETCORE_URLS=https://localhost:7007;http://localhost:5179
```

### 3. Obtener Credenciales de MongoDB Atlas

1. Crea una cuenta en [MongoDB Atlas](https://www.mongodb.com/cloud/atlas)
2. Crea un cluster gratuito
3. Configura un usuario de base de datos
4. Whitelist tu IP
5. Obtén el connection string

### 4. Obtener Credenciales de Cloudinary

1. Crea una cuenta en [Cloudinary](https://cloudinary.com/)
2. Ve al Dashboard
3. Copia: Cloud Name, API Key, y API Secret

### 5. Instalar Dependencias

```bash
dotnet restore
```

### 6. Ejecutar la Aplicación

```bash
dotnet run
```

La API estará disponible en:

- **HTTP**: http://localhost:5179
- **HTTPS**: https://localhost:7007
- **Swagger UI**: https://localhost:7007/swagger

## 🧪 Testing

### Ejecutar Tests Unitarios

```bash
dotnet test
```

### Tests de API con archivo HTTP

Usa el archivo `api-tests-images.http` incluido para probar todos los endpoints.

## 📚 Endpoints Principales

### 👥 Propietarios (Owners)

```
GET    /api/Owner              # Obtener todos los propietarios
POST   /api/Owner              # Crear propietario
GET    /api/Owner/{id}         # Obtener propietario por ID
PUT    /api/Owner/{id}         # Actualizar propietario
DELETE /api/Owner/{id}         # Eliminar propietario
GET    /api/Owner/search       # Buscar propietarios
```

### 🏠 Propiedades (Properties)

```
GET    /api/Property           # Obtener propiedades (con filtros)
POST   /api/Property           # Crear propiedad
GET    /api/Property/{id}      # Obtener propiedad por ID
PUT    /api/Property/{id}      # Actualizar propiedad
DELETE /api/Property/{id}      # Eliminar propiedad
GET    /api/Property/price-range # Filtrar por rango de precios
GET    /api/Property/owner/{id}  # Propiedades por propietario
```

### 📸 Imágenes (Images)

```
POST   /api/Image/upload            # Subir imagen individual
POST   /api/Image/upload-multiple   # Subir múltiples imágenes
GET    /api/Image/url/{publicId}    # URL optimizada
GET    /api/Image/responsive/{publicId} # URLs responsivas
DELETE /api/Image/{publicId}        # Eliminar imagen
```

## 🔍 Ejemplos de Uso

### Crear Propietario

```json
POST /api/Owner
{
  "name": "Juan Carlos",
  "lastName": "Pérez García",
  "phone": "+57 301 555 1234",
  "email": "juan.perez@email.com",
  "birthday": "1985-03-15T00:00:00Z"
}
```

### Crear Propiedad

```json
POST /api/Property
{
  "name": "Apartamento Moderno Chapinero",
  "address": "Carrera 13 #85-32, Chapinero",
  "price": 650000000,
  "idOwner": "64a1b2c3d4e5f6789012345",
  "codigoInternal": "APT_CHA_001",
  "year": 2022,
  "image": "https://res.cloudinary.com/miguedev/image/upload/v123/property.jpg",
  "city": "Bogotá",
  "state": "Cundinamarca",
  "country": "Colombia"
}
```

### Filtrar Propiedades

```
GET /api/Property?name=apartamento&minPrice=500000000&maxPrice=800000000&city=bogotá&page=1&pageSize=10&sortBy=price&sortDirection=desc
```

### Subir Imagen

```bash
curl -X POST "https://localhost:7007/api/Image/upload" \
  -H "Content-Type: multipart/form-data" \
  -F "file=@imagen.jpg"
```

## 🏗️ Arquitectura

```
├── Controllers/          # Controladores de API
├── Services/            # Lógica de negocio
├── Repository/          # Acceso a datos
├── Entities/           # Modelos de datos
├── DTOs/               # Objetos de transferencia
├── Settings/           # Configuraciones
└── Tests/              # Tests unitarios
```

## 🔐 Seguridad

- **Variables de entorno** para credenciales sensibles
- **Validación** de tipos de archivo en uploads
- **Límites de tamaño** para imágenes (10MB)
- **CORS** configurado para desarrollo
- **HTTPS** habilitado por defecto

## 📝 Variables de Entorno

| Variable                    | Descripción                    | Ejemplo                                        |
| --------------------------- | ------------------------------ | ---------------------------------------------- |
| `MONGODB_CONNECTION_STRING` | Connection string de MongoDB   | `mongodb+srv://user:pass@cluster.mongodb.net/` |
| `MONGODB_DATABASE_NAME`     | Nombre de la base de datos     | `RealEstateDB`                                 |
| `CLOUDINARY_CLOUD_NAME`     | Nombre del cloud de Cloudinary | `my-cloud`                                     |
| `CLOUDINARY_API_KEY`        | API Key de Cloudinary          | `123456789012345`                              |
| `CLOUDINARY_API_SECRET`     | API Secret de Cloudinary       | `abcdef123456789`                              |

## 🚫 Archivo .gitignore

El archivo `.env` está incluido en `.gitignore` para proteger tus credenciales. **NUNCA** hagas commit del archivo `.env` real.

## 🆘 Soporte

Si tienes problemas:

1. Verifica que todas las variables de entorno están configuradas
2. Asegúrate de que MongoDB Atlas tiene tu IP en whitelist
3. Confirma que las credenciales de Cloudinary son correctas
4. Revisa los logs de la aplicación para errores específicos

---

**Desarrollado para prueba técnica Senior Frontend Developer** 🚀
