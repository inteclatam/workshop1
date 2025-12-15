# Customer Data Seeder

Seeder para generar datos de clientes falsos usando **Bogus** y el generador de IDs **Snowflake**.

## Características

- Genera 50 customers por defecto (configurable)
- Cada customer incluye:
  - ID único generado con Snowflake
  - Nombre y apellido generados con Bogus
  - Email basado en el nombre
  - Número de teléfono en formato `prefix+number`
  - Un contacto principal asociado

## Uso

### Endpoint de Desarrollo

En modo Development, hay endpoints disponibles para seed datos:

#### 1. Seed de Customers
```bash
POST /api/seed/customers?count=50
```

Parámetros:
- `count` (opcional): Número de customers a generar. Default: 50

Respuesta:
```json
{
  "message": "Successfully seeded 50 customers with contact information",
  "count": 50
}
```

#### 2. Seed de Contactos Adicionales
```bash
POST /api/seed/additional-contacts?maxAdditionalContacts=2
```

Parámetros:
- `maxAdditionalContacts` (opcional): Máximo de contactos adicionales por customer. Default: 2

Respuesta:
```json
{
  "message": "Successfully added additional contacts to existing customers",
  "maxContactsPerCustomer": 2
}
```

## Ejemplo con cURL

```bash
# Generar 50 customers
curl -X POST "http://localhost:5000/api/seed/customers?count=50"

# Agregar contactos adicionales
curl -X POST "http://localhost:5000/api/seed/additional-contacts?maxAdditionalContacts=2"
```

## Notas

- El seeder solo ejecuta si la base de datos está vacía (para el seed inicial de customers)
- Los IDs se generan usando el algoritmo Snowflake para garantizar unicidad distribuida
- Los emails y teléfonos son generados por Bogus con formato realista
- El formato del teléfono es: `prefix+number` (ej: "57+3001234567")
