<!-- @format -->

# 📊 PropertyTrace System - Documentación Completa

## 🎯 **Descripción**

El sistema **PropertyTrace** permite llevar un historial completo de transacciones, ventas, y eventos importantes relacionados con cada propiedad. Es una funcionalidad clave para el seguimiento de valuaciones, cambios de precio, y auditoría de transacciones.

## 🏗️ **Arquitectura del Sistema**

### **Entidades Principales**

#### **PropertyTrace Entity**

```csharp
public class PropertyTrace : BaseEntity
{
    public string IdProperty { get; set; } = string.Empty;  // Referencia a Property
    public DateTime DateSale { get; set; }                  // Fecha de la transacción
    public string Name { get; set; } = string.Empty;        // Descripción del evento
    public decimal Value { get; set; }                      // Valor de la transacción
    public decimal Tax { get; set; }                        // Impuestos asociados
}
```

### **DTOs Implementados**

#### **PropertyTraceResponseDto** (Para consultas)

```csharp
public class PropertyTraceResponseDto
{
    public string Id { get; set; }
    public string IdProperty { get; set; }
    public string DateSale { get; set; }     // Formato: "yyyy-MM-dd"
    public string Name { get; set; }
    public decimal Value { get; set; }
    public decimal Tax { get; set; }
    public DateTime CreatedAt { get; set; }
}
```

#### **PropertyTraceCreateDto** (Para crear traces)

```csharp
public class PropertyTraceCreateDto
{
    [Required]
    public string IdProperty { get; set; }

    [Required]
    public DateTime DateSale { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public decimal Value { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public decimal Tax { get; set; }
}
```

#### **PropertyTraceDto** (Para incluir en PropertyDto)

```csharp
public class PropertyTraceDto
{
    public string DateSale { get; set; }     // Formato: "yyyy-MM-dd"
    public string Name { get; set; }
    public decimal Value { get; set; }
    public decimal Tax { get; set; }
}
```

## 🔄 **Repository Pattern**

### **IPropertyTraceRepository**

```csharp
public interface IPropertyTraceRepository : IRepository<PropertyTrace>
{
    Task<List<PropertyTrace>> GetByPropertyIdAsync(string propertyId);
    Task DeleteByPropertyIdAsync(string propertyId);
}
```

### **Métodos Especializados**

- `GetByPropertyIdAsync(propertyId)` - Obtener todos los traces de una propiedad
- `DeleteByPropertyIdAsync(propertyId)` - Eliminar todos los traces de una propiedad

## 📡 **API Endpoints**

### **Endpoint Principal: `/api/PropertyTrace`**

#### **1. Crear PropertyTrace**

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

**Respuesta:**

```json
{
  "id": "trace_id",
  "idProperty": "property_id_here",
  "dateSale": "2024-01-15",
  "name": "Primera venta",
  "value": 450000000,
  "tax": 45000000,
  "createdAt": "2024-10-27T23:30:47.644Z"
}
```

#### **2. Obtener Todos los Traces**

```bash
GET /api/PropertyTrace
```

#### **3. Obtener Traces por Propiedad**

```bash
GET /api/PropertyTrace/property/{propertyId}
```

#### **4. Obtener Trace Específico**

```bash
GET /api/PropertyTrace/{id}
```

#### **5. Actualizar Trace**

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

#### **6. Eliminar Trace**

```bash
DELETE /api/PropertyTrace/{id}
```

#### **7. Eliminar Todos los Traces de una Propiedad**

```bash
DELETE /api/PropertyTrace/property/{propertyId}
```

## 🏠 **Integración con Properties**

### **Respuesta Automática en PropertyController**

Cuando consultas una propiedad con `GET /api/Property/{id}`, automáticamente se incluyen los traces:

```json
{
  "id": "property_id",
  "name": "Casa Moderna",
  "address": "Calle 123",
  "price": 500000000,
  "images": [...],
  "owner": {...},
  "traces": [
    {
      "dateSale": "2020-06-15",
      "name": "Primera venta",
      "value": 450000000,
      "tax": 45000000
    },
    {
      "dateSale": "2024-01-20",
      "name": "Venta actual",
      "value": 500000000,
      "tax": 50000000
    }
  ],
  "codigoInternal": "PROP001",
  "year": 2020,
  "createdAt": "2024-10-27T23:16:24.208Z"
}
```

## 💼 **Casos de Uso Empresariales**

### **1. Historial de Valuaciones**

```bash
# Registrar tasación inicial
POST /api/PropertyTrace
{
  "idProperty": "prop123",
  "dateSale": "2020-01-15",
  "name": "Tasación inicial",
  "value": 400000000,
  "tax": 0
}

# Registrar primera venta
POST /api/PropertyTrace
{
  "idProperty": "prop123",
  "dateSale": "2020-06-20",
  "name": "Primera venta",
  "value": 420000000,
  "tax": 42000000
}

# Registrar reventa
POST /api/PropertyTrace
{
  "idProperty": "prop123",
  "dateSale": "2024-03-10",
  "name": "Reventa",
  "value": 580000000,
  "tax": 58000000
}
```

### **2. Análisis de Rentabilidad**

Los traces permiten calcular:

- **ROI (Return on Investment)**
- **Apreciación del valor** en el tiempo
- **Historial de impuestos** pagados
- **Frecuencia de transacciones**

### **3. Auditoría y Compliance**

- Registro completo de todas las transacciones
- Fechas precisas de cada evento
- Valores históricos para declaraciones
- Rastreo de impuestos pagados

## 🔍 **Consultas Avanzadas**

### **Obtener Evolución de Precios**

```bash
# Traces ordenados por fecha para ver evolución
GET /api/PropertyTrace/property/{propertyId}
```

### **Análisis por Rangos de Fechas**

```bash
# En el frontend puedes filtrar por fechas
# Los traces incluyen DateSale para análisis temporal
```

### **Comparación de Propiedades**

```bash
# Obtener traces de múltiples propiedades
GET /api/PropertyTrace?propertyIds=prop1,prop2,prop3
```

## 📊 **Métricas y Analytics**

### **Datos que Proporciona el Sistema**

1. **Valor histórico** de cada propiedad
2. **Tendencias de precio** en el tiempo
3. **Impuestos acumulados** por propiedad
4. **Frecuencia de transacciones**
5. **ROI calculable** entre compra y venta

### **Integración con Business Intelligence**

Los datos están estructurados para fácil integración con:

- **Power BI**
- **Tableau**
- **Excel** reporting
- **Custom dashboards**

## ⚙️ **Configuración Técnica**

### **Dependency Injection (Program.cs)**

```csharp
// Repository ya registrado automáticamente
builder.Services.AddScoped<IPropertyTraceRepository, PropertyTraceRepository>();
```

### **MongoDB Collection**

- **Nombre**: `PropertyTraces`
- **Indexes**: Automáticos en `IdProperty` para optimizar consultas
- **Schema**: Flexible para futuras extensiones

### **Validaciones Implementadas**

- ✅ **IdProperty**: Debe existir la propiedad
- ✅ **DateSale**: Fecha válida requerida
- ✅ **Name**: Entre 1-100 caracteres
- ✅ **Value**: Mayor que 0
- ✅ **Tax**: Mayor o igual que 0

## 🚀 **Roadmap de Mejoras**

### **Funcionalidades Futuras**

- [ ] **Filtros por rango de fechas** en endpoints
- [ ] **Cálculo automático de ROI** en respuestas
- [ ] **Notificaciones** para cambios significativos de valor
- [ ] **Export a Excel/PDF** de historial de traces
- [ ] **Analytics dashboard** integrado
- [ ] **Alertas de compliance** fiscal

### **Optimizaciones Técnicas**

- [ ] **Caching** de traces por propiedad
- [ ] **Indexes compuestos** para consultas complejas
- [ ] **Batch operations** para múltiples traces
- [ ] **Soft delete** en lugar de delete físico

## 🎯 **Ejemplo Completo de Uso**

```bash
# 1. Crear propiedad
POST /api/Property
{
  "name": "Villa Campestre",
  "address": "Km 8 vía La Calera",
  "price": 450000000,
  "idOwner": "owner_id",
  "codigoInternal": "VC001"
}

# 2. Registrar tasación inicial
POST /api/PropertyTrace
{
  "idProperty": "property_id_from_step_1",
  "dateSale": "2018-08-10",
  "name": "Tasación construcción",
  "value": 400000000,
  "tax": 0
}

# 3. Registrar primera venta
POST /api/PropertyTrace
{
  "idProperty": "property_id_from_step_1",
  "dateSale": "2018-12-15",
  "name": "Primera venta",
  "value": 420000000,
  "tax": 42000000
}

# 4. Registrar venta actual
POST /api/PropertyTrace
{
  "idProperty": "property_id_from_step_1",
  "dateSale": "2024-01-20",
  "name": "Venta actual",
  "value": 450000000,
  "tax": 45000000
}

# 5. Consultar propiedad completa con historial
GET /api/Property/property_id_from_step_1
```

## 📈 **Beneficios del Sistema**

### **Para el Negocio**

- ✅ **Transparencia total** en transacciones
- ✅ **Análisis de rentabilidad** automatizado
- ✅ **Compliance fiscal** facilitado
- ✅ **Histórico completo** para clientes

### **Para Desarrolladores**

- ✅ **API consistente** con el resto del sistema
- ✅ **Repository pattern** mantenido
- ✅ **DTOs especializados** para cada caso
- ✅ **Validaciones robustas** implementadas

### **Para Usuarios Finales**

- ✅ **Historial visual** de cada propiedad
- ✅ **Datos precisos** para toma de decisiones
- ✅ **Transparencia** en evolución de precios
- ✅ **Información fiscal** organizada

**El sistema PropertyTrace es una pieza fundamental para un sistema inmobiliario empresarial completo** 🏆
