<!-- @format -->

# Real Estate API - Deployment Guide

## Pasos para Ejecutar el Proyecto

### 1. Prerrequisitos

- .NET 9 SDK instalado
- Acceso a MongoDB (local o Atlas)
- Puerto 7000-8000 disponible

### 2. Configuración Rápida

1. **Descargar y extraer el proyecto**
2. **Navegar al directorio**

   ```bash
   cd BackEnd/Back-Mongo
   ```

3. **Verificar configuración de MongoDB** en `appsettings.json`:

   ```json
   {
     "MongoDbSettings": {
       "ConnectionString": "mongodb+srv://miguelatenciacanoles_db_user:ARCHwFxH7onJ7He8@cluster-millio.njqqpng.mongodb.net/",
       "DatabaseName": "Million-TestDB"
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

- **Swagger UI**: http://localhost:5000 o https://localhost:7xxx
- **API Base URL**: https://localhost:7xxx/api

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
   curl -X POST https://localhost:7xxx/api/Owner \
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
   curl -X POST https://localhost:7xxx/api/Property \
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

3. **Buscar propiedades**:
   ```
   GET https://localhost:7xxx/api/Property?name=casa&minPrice=200000000&maxPrice=500000000&city=bogotá
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
