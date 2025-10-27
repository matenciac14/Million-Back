<!-- @format -->

# 🚀 Real Estate API - Guía de Despliegue

## 📋 Resumen del Sistema

**API REST completa** para gestión inmobiliaria con **.NET 9**, **MongoDB** y **Cloudinary**. Sistema empresarial con gestión de propiedades, propietarios, imágenes optimizadas e historial de transacciones.

## 🛠️ Prerrequisitos de Producción

### Infraestructura Requerida

- **Servidor**: Compatible con .NET 9 (Linux/Windows/Azure/AWS)
- **Base de Datos**: MongoDB Atlas (recomendado) o instancia propia
- **CDN Imágenes**: Cuenta Cloudinary configurada
- **SSL**: Certificado HTTPS para producción
- **Memoria**: Mínimo 2GB RAM recomendado

### Software

- **.NET 9 Runtime** o superior
- **Nginx/IIS** como reverse proxy (opcional)
- **PM2** o **Systemd** para gestión de procesos (Linux)

## 🔧 Configuración de Entorno

### 1. Variables de Entorno de Producción

Crear archivo `.env` o configurar variables del sistema:

```env
# MongoDB Production
MONGODB_CONNECTION_STRING=mongodb+srv://prod_user:secure_password@cluster-prod.mongodb.net/
MONGODB_DATABASE_NAME=RealEstateProduction

# Cloudinary Production
CLOUDINARY_CLOUD_NAME=tu_production_cloud
CLOUDINARY_API_KEY=tu_production_key
CLOUDINARY_API_SECRET=tu_production_secret

# Production Settings
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=https://localhost:443;http://localhost:80
ASPNETCORE_FORWARDEDHEADERS_ENABLED=true

# Security
CORS_ALLOWED_ORIGINS=https://tu-frontend-production.com,https://tu-admin-panel.com
```

### 2. Configuración de Base de Datos

#### MongoDB Atlas (Recomendado para Producción)

```bash
# 1. Crear cluster en MongoDB Atlas
# 2. Configurar IP whitelist
# 3. Crear usuario de base de datos
# 4. Obtener connection string
```

#### Colecciones que se crean automáticamente

- `Properties` - Propiedades inmobiliarias
- `Owners` - Propietarios
- `PropertyImages` - Gestión de imágenes
- `PropertyTraces` - Historial de transacciones

  O configurar `appsettings.json` con tus credenciales:

  ```json
  {
    "MongoDbSettings": {
      "ConnectionString": "mongodb+srv://YOUR_USER:YOUR_PASSWORD@YOUR_CLUSTER.mongodb.net/",
      "DatabaseName": "YOUR_DATABASE_NAME"
    }
  }
  ```

4. **Restaurar dependencias**

   ```bash
   dotnet restore
   ```

5. **Compilar**

   ```bash
   dotnet build
   ```

6. **Ejecutar tests**

   ```bash
   dotnet test
   ```

7. **Ejecutar aplicación**
   ```bash
   dotnet run
   ```

### 3. Acceso a la API

- **Swagger UI**: http://localhost:5179/swagger (HTTP) o https://localhost:7007/swagger (HTTPS)
- **API Base URL**: http://localhost:5179/api o https://localhost:7007/api

### 4. Endpoints Principales

```
GET    /api/Property              # Listar propiedades con filtros
GET    /api/Property/{id}         # Obtener propiedad específica
POST   /api/Property              # Crear propiedad
PUT    /api/Property/{id}         # Actualizar propiedad
DELETE /api/Property/{id}         # Eliminar propiedad

GET    /api/Owner                 # Listar propietarios
POST   /api/Owner                 # Crear propietario
```

### 5. Ejemplo de Uso

1. **Crear un propietario**:

   ```bash
   curl -X POST https://localhost:7007/api/Owner \
     -H "Content-Type: application/json" \
     -d '{
       "name": "Juan",
       "lastName": "Pérez",
       "email": "juan@email.com",
       "phone": "+57 300 123 4567"
     }'
   ```

2. **Crear una propiedad**:

   ```bash
   curl -X POST https://localhost:7007/api/Property \
     -H "Content-Type: application/json" \
     -d '{
       "name": "Casa Moderna",
       "address": "Carrera 13 #85-32",
       "price": 450000000,
       "idOwner": "OWNER_ID_FROM_STEP_1",
       "city": "Bogotá",
       "country": "Colombia"
     }'
   ```

3. **Subir imagen a la propiedad**:

   ```bash
   curl -X POST "https://localhost:7007/api/Image/property/PROPERTY_ID/upload?description=Fachada&isMain=true" \
     -F "file=@imagen.jpg"
   ```

4. **Buscar propiedades**:
   ```
   GET https://localhost:7007/api/Property?name=casa&minPrice=200000000&maxPrice=500000000&city=bogotá
   ```

### 6. Datos de Base Incluidos

El proyecto está configurado para crear automáticamente las colecciones necesarias:

- Property
- Owner
- PropertyImage
- PropertyPlace

### 7. Solución de Problemas

- **Puerto ocupado**: Cambiar puerto en `launchSettings.json`
- **Error de MongoDB**: Verificar cadena de conexión y acceso a red
- **Error de dependencias**: Ejecutar `dotnet clean` y luego `dotnet restore`

### 8. Para Desarrollo Frontend

La API está configurada con CORS para permitir conexiones desde:

- http://localhost:3000
- http://localhost:3001
- https://localhost:3000

Usar la base URL de la API en tu aplicación React/Next.js.

### 9. Tests de la API

```bash
# Ejecutar todos los tests
dotnet test

# Ejecutar tests con detalles
dotnet test --verbosity normal

# Ejecutar tests específicos
dotnet test --filter "ClassName=PropertyEntityTests"
```

---

**¡La API está lista para usar!** 🚀
