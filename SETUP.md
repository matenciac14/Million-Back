# 📝 Instrucciones de Configuración Post-Clonado

## 🔧 Configuración Inicial

### 1. Clonar el Repositorio
```bash
git clone https://github.com/matenciac14/Million-Back.git
cd Million-Back
```

### 2. Configurar Variables de Entorno
```bash
# Copia el archivo de ejemplo
cp .env.example .env

# Edita el archivo .env con tus credenciales reales
nano .env  # o el editor de tu preferencia
```

### 3. Configurar MongoDB Atlas
1. Ve a [MongoDB Atlas](https://www.mongodb.com/cloud/atlas)
2. Crea un cluster gratuito
3. Configura un usuario de base de datos
4. Whitelist tu IP
5. Copia el connection string a `MONGODB_CONNECTION_STRING`

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
- **Swagger**: https://localhost:7007/swagger

## ⚠️ IMPORTANTE
**NUNCA hagas commit del archivo `.env`** - contiene credenciales sensibles.

## 🧪 Testing
```bash
# Tests unitarios
dotnet test

# Tests de API
# Usa el archivo api-tests-images.http incluido
```