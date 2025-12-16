# Fix para el Error de Timestamp con PostgreSQL

## Problema
El error `'timestamp with time zone' literal cannot be generated for Unspecified DateTime` ocurre porque las migraciones generadas contenían valores DateTime con `DateTimeKind.Unspecified`, pero PostgreSQL requiere timestamps UTC.

## Solución Aplicada

### 1. Actualización del DatabaseSeeder
Se modificó el archivo `src/Infrastructure/Configuration/DatabaseSeeder.cs` para que genere valores DateTime con UTC:

```csharp
var created = DateTime.SpecifyKind(
    faker.Date.Between(new DateTime(2023, 1, 1), DateTime.UtcNow),
    DateTimeKind.Utc);
```

### 2. Migraciones Eliminadas
Se eliminaron las migraciones con timestamps incorrectos:
- `20251216222556_UpdateFields.cs`
- `20251216222727_UpdateFields2.cs`

## Pasos para Regenerar las Migraciones

### Ejecuta los siguientes comandos en tu máquina local:

1. **Limpia la base de datos** (si existe):
   ```bash
   cd src
   dotnet ef database drop --context CustomersDbContext --force
   ```

2. **Crea una nueva migración inicial**:
   ```bash
   dotnet ef migrations add InitialCreate --context CustomersDbContext
   ```

3. **Aplica la migración**:
   ```bash
   dotnet ef database update --context CustomersDbContext
   ```

## Verificación

Después de aplicar los pasos anteriores:
- La migración debe crearse sin errores
- Los valores DateTime en la migración deben tener `DateTimeKind.Utc`
- La base de datos debe crearse correctamente con todos los datos de seeding
- El endpoint debe funcionar correctamente

## Archivos Modificados
- ✅ `src/Infrastructure/Configuration/DatabaseSeeder.cs` - Corregido para usar UTC
- ✅ Migraciones antiguas eliminadas
- ⏳ Nueva migración pendiente de generar (requiere dotnet en tu máquina)
