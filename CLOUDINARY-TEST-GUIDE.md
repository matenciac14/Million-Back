<!-- @format -->

# 📸 CLOUDINARY IMAGE UPLOAD - TEST GUIDE

## 🔧 **Paso 1: Configurar Credenciales**

Actualiza tu `appsettings.json` con las credenciales reales de Cloudinary:

```json
{
  "CloudinarySettings": {
    "CloudName": "tu-cloud-name-real",
    "ApiKey": "tu-api-key-real",
    "ApiSecret": "tu-api-secret-real",
    "BaseUrl": "https://res.cloudinary.com",
    "SecureUrl": "https://res.cloudinary.com"
  }
}
```

## 🚀 **Paso 2: Ejecutar Servidor**

```bash
cd /Users/miguelatencia/Desktop/TEST-Million/BackEnd/Back-Mongo
dotnet run
```

## 📤 **Paso 3: Test de Subida de Imagen**

### **Opción A: Con una imagen real**

```bash
# Reemplaza 'path/to/image.jpg' con una imagen real
curl -X POST "http://localhost:5179/api/Image/upload?folder=real-estate/test" \
  -F "file=@path/to/image.jpg" \
  -H "Content-Type: multipart/form-data"
```

### **Opción B: Crear imagen de prueba**

```bash
# Crear una imagen simple de 100x100 píxeles (requiere ImageMagick)
convert -size 100x100 xc:blue test-house.jpg

# Subir la imagen de prueba
curl -X POST "http://localhost:5179/api/Image/upload?folder=real-estate/test" \
  -F "file=@test-house.jpg" \
  -H "Content-Type: multipart/form-data"
```

### **Opción C: Descargar imagen de prueba**

```bash
# Descargar imagen de ejemplo
curl -o test-house.jpg "https://images.unsplash.com/photo-1570129477492-45c003edd2be?w=500&h=300"

# Subir la imagen
curl -X POST "http://localhost:5179/api/Image/upload?folder=real-estate/test" \
  -F "file=@test-house.jpg" \
  -H "Content-Type: multipart/form-data"
```

## 📋 **Respuesta Esperada**

Si todo funciona correctamente, deberías recibir:

```json
{
  "publicId": "real-estate/test/abc123",
  "url": "https://res.cloudinary.com/tu-cloud/image/upload/real-estate/test/abc123.jpg",
  "width": 500,
  "height": 300,
  "format": "jpg",
  "bytes": 45678,
  "createdAt": "2025-10-27T..."
}
```

## 🧪 **Tests Adicionales**

### **1. URLs Responsive**

```bash
curl "http://localhost:5179/api/Image/responsive/real-estate/test/abc123"
```

### **2. URL Optimizada**

```bash
curl "http://localhost:5179/api/Image/url/real-estate/test/abc123?width=400&height=300&format=webp"
```

### **3. Eliminar Imagen**

```bash
curl -X DELETE "http://localhost:5179/api/Image/real-estate/test/abc123"
```

## ❌ **Posibles Errores y Soluciones**

### **Error: "Invalid file type"**

- **Causa**: Archivo no es imagen válida
- **Solución**: Usar .jpg, .png, .gif, o .webp

### **Error: "File size cannot exceed 10MB"**

- **Causa**: Imagen muy grande
- **Solución**: Comprimir imagen o usar imagen más pequeña

### **Error: "CloudName/ApiKey/ApiSecret required"**

- **Causa**: Credenciales no configuradas
- **Solución**: Actualizar appsettings.json con credenciales reales

### **Error: "401 Unauthorized"**

- **Causa**: Credenciales incorrectas
- **Solución**: Verificar credenciales en dashboard de Cloudinary

## 🎯 **¿Listo para el Test?**

1. ✅ **Configura credenciales** en appsettings.json
2. ✅ **Ejecuta servidor**: `dotnet run`
3. ✅ **Prepara imagen** de prueba
4. ✅ **Ejecuta comando curl** de subida
5. ✅ **Verifica respuesta** JSON

¡Avísame cuando tengas las credenciales configuradas y podemos hacer el test! 🚀
