# Instrucciones para Recrear las Migraciones

## Problema Resuelto
Se corrigió el error en el seeder de datos que causaba incompatibilidad de tipos al intentar sembrar datos de `ContactInformation`. El seeder ahora usa correctamente los tipos de dominio:

- `CustomerId` en lugar de `long`
- `CustomerName` en lugar de `FirstName` y `LastName` separados
- `EMailAddress` para el email
- `PhoneNumber` para el número de teléfono

## Pasos para Aplicar los Cambios

### 1. Recrear las Migraciones

Las migraciones anteriores fueron eliminadas porque contenían datos de seeding incompatibles. Debes recrearlas:

```bash
# Desde la raíz del proyecto
dotnet ef migrations add InitialCreate --project src/Infrastructure --startup-project src/Api
```

### 2. Aplicar las Migraciones

```bash
dotnet ef database update --project src/Infrastructure --startup-project src/Api
```

### 3. Verificar que el Seeding Funciona

Una vez aplicadas las migraciones, la base de datos debería contener:
- 50 clientes con nombres generados aleatoriamente
- Cada cliente tiene al menos 1 contacto principal
- Algunos clientes tienen contactos adicionales (0-2 por cliente)

### 4. Ejecutar la Aplicación

```bash
dotnet run --project src/Api
```

## Cambios Realizados

### Archivo: `src/Infrastructure/Configuration/DatabaseSeeder.cs`

**Antes:**
```csharp
customerSeeds.Add(new
{
    Id = customerId,              // ❌ long en lugar de CustomerId
    FirstName = firstName,        // ❌ campos separados
    LastName = lastName,          // ❌ en lugar de CustomerName
    // ...
});

contactSeeds.Add(new
{
    // ...
    Email = primaryEmail,         // ❌ string en lugar de EMailAddress
    PhoneNumber = primaryPhoneNumber,  // ❌ string
    PhonePrefix = primaryPhonePrefix,  // ❌ string
    PhoneValue = $"{primaryPhonePrefix}+{primaryPhoneNumber}", // ❌ string
    CustomerId = customerId       // ❌ long en lugar de CustomerId
});
```

**Después:**
```csharp
customerSeeds.Add(new
{
    Id = new CustomerId(customerId),           // ✅ Tipo de dominio
    Name = new CustomerName(firstName, lastName), // ✅ Value Object
    // ...
});

contactSeeds.Add(new
{
    // ...
    Email = new EMailAddress(primaryEmail),    // ✅ Value Object
    PhoneNumber = new PhoneNumber(primaryPhoneNumber, primaryPhonePrefix), // ✅ Value Object
    CustomerId = new CustomerId(customerId)    // ✅ Tipo de dominio
});
```

## Notas Importantes

- El seeder genera datos usando **Bogus** con un seed fijo (12345) para reproducibilidad
- Los IDs se generan usando el algoritmo **Snowflake** para garantizar unicidad distribuida
- Los emails generados tienen el formato correcto según la validación de `EMailAddress`
- Los números de teléfono tienen el formato `prefix+number` requerido por `PhoneNumber`
