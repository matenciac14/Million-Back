<!-- @format -->

# 🏠 Real Estate API - Sistema Inmobiliario Empresarial

[![.NET](https://img.shields.io/badge/.NET-9.0-purple.svg)](https://dotnet.microsoft.com/)
[![MongoDB](https://img.shields.io/badge/MongoDB-7.0-green.svg)](https://www.mongodb.com/)
[![Cloudinary](https://img.shields.io/badge/Cloudinary-Images-blue.svg)](https://cloudinary.com/)
[![Status](https://img.shields.io/badge/Status-Production%20Ready-brightgreen.svg)]()

## 📋 **Descripción del Sistema**

**API REST empresarial completa** para gestión integral de propiedades inmobiliarias construida con **.NET 9**, **MongoDB** y **Cloudinary**. Sistema robusto que incluye gestión de propietarios, propiedades, imágenes optimizadas e historial completo de transacciones.

## 🚀 **Características Avanzadas**

### **🏠 Gestión de Propiedades**

- **CRUD completo** con validaciones empresariales
- **Filtros avanzados** por precio, ciudad, propietario, año
- **Búsqueda inteligente** con múltiples criterios
- **Paginación optimizada** para grandes volúmenes

### **📸 Sistema de Imágenes**

- **Upload automático** a Cloudinary con organización por carpetas
- **URLs responsivas** automáticas para múltiples dispositivos
- **Optimización automática** de imágenes (WebP, compresión)
- **Gestión por propiedad** con sistema de imagen principal

### **📊 PropertyTrace (Historial)**

- **Registro completo** de transacciones y ventas
- **Historial de valuaciones** con fechas precisas
- **Tracking de impuestos** y valores históricos
- **Análisis de rentabilidad** y evolución de precios

### **👤 Gestión de Propietarios**

- **Información detallada** con validaciones
- **Relaciones automáticas** con propiedades
- **Contacto y datos personales** estructurados

### **⚡ Arquitectura Empresarial**

- **Repository Pattern** para escalabilidad
- **Dependency Injection** configurado
- **Async/Await** para alto rendimiento
- **Error handling** robusto y consistente

## 🛠️ **Stack Tecnológico**

- **.NET 9** - Framework principal con ASP.NET Core
- **MongoDB** - Base de datos NoSQL escalable
- **Cloudinary** - CDN y gestión de imágenes en la nube
- **Repository Pattern** - Arquitectura de acceso a datos
- **Swagger/OpenAPI** - Documentación automática interactiva
- **NUnit** - Framework de testing empresarial

## 📋 **Requisitos del Sistema**

### **Desarrollo**

- **.NET 9 SDK** o superior
- **MongoDB Atlas** account (recomendado) o instancia local
- **Cloudinary** account con API keys
- **IDE**: Visual Studio 2022 / VS Code / Rider

### **Producción**

- **Servidor**: Compatible con .NET 9 (Linux/Windows/Docker)
- **Base de datos**: MongoDB Atlas (escalable)
- **CDN**: Cloudinary (gestión de imágenes)
- **SSL**: Certificado HTTPS requerido
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
- **Swagger UI**: https://localhost:7007/swagger (HTTPS) o http://localhost:5179/swagger (HTTP)

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
