<!-- @format -->

# � Real Estate API - Guía de Configuración Completa

## � **Configuración Post-Clonado**

### **1. Clonar y Navegar**

```bash
git clone https://github.com/matenciac14/Million-Back.git
cd Million-Back/Back-Mongo
```

### **2. Configuración de Variables de Entorno**

#### **Crear archivo .env**

```bash
# Crear archivo de configuración
touch .env

# Editar con tus credenciales
nano .env  # o usar VS Code: code .env
```

#### **Contenido del archivo .env**

```env
# ===========================================
# MONGODB CONFIGURATION
# ===========================================
MONGODB_CONNECTION_STRING=mongodb+srv://username:password@cluster.mongodb.net/
MONGODB_DATABASE_NAME=RealEstateProduction

# ===========================================
# CLOUDINARY CONFIGURATION (REQUIRED)
# ===========================================
CLOUDINARY_CLOUD_NAME=tu_cloud_name
CLOUDINARY_API_KEY=tu_api_key
CLOUDINARY_API_SECRET=tu_api_secret

# ===========================================
# ENVIRONMENT SETTINGS
# ===========================================
ASPNETCORE_ENVIRONMENT=Development
ASPNETCORE_URLS=https://localhost:7007;http://localhost:5179

# ===========================================
# SECURITY (PRODUCTION)
# ===========================================
CORS_ALLOWED_ORIGINS=http://localhost:3000,https://tu-frontend.com
```

### **3. Configurar MongoDB Atlas (Recomendado)**

#### **Paso a Paso:**

1. **Crear cuenta** en [MongoDB Atlas](https://www.mongodb.com/cloud/atlas)
2. **Crear cluster** gratuito (M0 Sandbox)
3. **Configurar Security:**
   - **Database Access**: Crear usuario con permisos `readWrite`
   - **Network Access**: Agregar tu IP o `0.0.0.0/0` para desarrollo
4. **Obtener Connection String:**
   - Click en **"Connect"** → **"Connect your application"**
   - Copiar URL y reemplazar `<password>` con tu password
   - Pegar en `MONGODB_CONNECTION_STRING`

#### **Estructura de Base de Datos:**

```
RealEstateProduction/
├── Properties          # Propiedades inmobiliarias
├── Owners             # Propietarios
├── PropertyImages     # Imágenes de propiedades
└── PropertyTraces     # Historial de transacciones
```

### **4. Configurar Cloudinary (OBLIGATORIO para imágenes)**

#### **Crear cuenta Cloudinary:**

1. **Registro** en [Cloudinary](https://cloudinary.com/)
2. **Dashboard** → **Account Details**
3. **Copiar credenciales:**
   - `Cloud Name`
   - `API Key`
   - `API Secret`

#### **Configuración de Carpetas:**

Las imágenes se organizan automáticamente en:

```
cloudinary_account/
└── real-estate/
    └── properties/
        ├── fachada-casa1_abc123.jpg
        ├── interior-casa1_def456.jpg
        └── jardin-casa1_ghi789.jpg
```

### 4. Configurar Cloudinary

1. Ve a [Cloudinary](https://cloudinary.com/)
2. Ve al Dashboard
3. Copia `Cloud Name`, `API Key`, y `API Secret` al archivo `.env`

### 5. Ejecutar el Proyecto

```bash
dotnet restore
dotnet run
```

## 🔗 URLs de la API

- **HTTP**: http://localhost:5179
- **HTTPS**: https://localhost:7007
- **Swagger**: https://localhost:7007/swagger (HTTPS) o http://localhost:5179/swagger (HTTP)

## ⚠️ IMPORTANTE

**NUNCA hagas commit del archivo `.env`** - contiene credenciales sensibles.

## 🧪 Testing

```bash
# Tests unitarios
dotnet test

# Tests de API
# Usa el archivo api-tests-images.http incluido
```
