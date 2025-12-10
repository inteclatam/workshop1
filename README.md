# Domain-Driven Design (DDD) - Customers

Este proyecto implementa una arquitectura basada en **Domain-Driven Design (DDD)** para la gestión de clientes, siguiendo los patrones tácticos y estratégicos de DDD.

## Estructura del Proyecto

```
src/
├── Primitives/         # Building Blocks de DDD
├── Domain/            # Capa de Dominio
├── Infrastructure/    # Capa de Infraestructura
└── Program.cs         # Punto de entrada
```

## Capa de Primitives (Building Blocks)

Los **Primitives** son los bloques de construcción fundamentales que implementan los patrones tácticos de DDD:

### Aggregate
- **Archivo**: `Primitives/Aggregate.cs`
- **Propósito**: Clase base para las raíces de agregado
- **Características**:
  - Hereda de `Entity<TId>` e implementa `IAggregate<TId>`
  - Define el límite transaccional del modelo de dominio
  - Garantiza la consistencia dentro de sus límites

### Entity
- **Archivo**: `Primitives/Entity.cs`
- **Propósito**: Clase base para entidades del dominio
- **Características**:
  - Tiene identidad única (`Id`)
  - La igualdad se basa en su identidad, no en sus atributos
  - Las entidades tienen ciclo de vida dentro del sistema

### ValueObject
- **Archivo**: `Primitives/ValueObject.cs`
- **Propósito**: Clase base para objetos de valor
- **Características**:
  - Inmutables por diseño
  - La igualdad se basa en sus atributos (`ValuesAreEqual`)
  - Sin identidad conceptual
  - Implementa operadores de comparación (`==`, `!=`)
  - Override de `GetHashCode()` y `Equals()`

### Identity / AggregateId
- **Archivo**: `Primitives/Identity.cs`, `Primitives/AggregateId.cs`
- **Propósito**: Identificadores tipados para entidades y agregados
- **Características**:
  - Proporciona type-safety para los IDs
  - Evita confusiones entre diferentes tipos de identificadores
  - Implementa conversiones implícitas
  - Validaciones en la construcción

### Interfaces de Comportamiento

El proyecto incluye interfaces que definen capacidades transversales:

- **IHaveAudit**: Auditoría de creación y modificación
- **IHaveSoftDelete**: Eliminación lógica
- **IHaveDomainEvents**: Manejo de eventos de dominio
- **IHaveAggregateVersion**: Control de versiones del agregado
- **IBusinessRule**: Reglas de negocio encapsuladas

## Capa de Dominio (Domain Layer)

La capa de dominio contiene la lógica de negocio y las reglas del dominio.

### Aggregate Root: Customer
- **Archivo**: `Domain/Customer.cs`
- **Patrón**: Aggregate Root
- **Responsabilidades**:
  - Raíz del agregado Customer
  - Mantiene invariantes de negocio
  - Encapsula lógica de negocio
  - Proporciona Factory Methods (`Create`)
  - Expone comportamiento del dominio mediante métodos

**Comportamiento del Dominio**:
```csharp
- Create()           // Factory method
- UpdateName()       // Actualización de nombre
- UpdateEmail()      // Actualización de email
- UpdatePhoneNumber()// Actualización de teléfono
- VerifyContact()    // Verificación de contacto
- SoftDelete()       // Eliminación lógica
- Restore()          // Restauración
```

**Principios Aplicados**:
- Encapsulación: Los setters son `private`, solo se modifica a través de métodos
- Invariantes: Las validaciones se realizan en los métodos de negocio
- Factory Pattern: `Create()` garantiza instancias válidas
- Tell, Don't Ask: Los métodos modifican el estado internamente

### Entity: ContactInformation
- **Archivo**: `Domain/ContactInformation.cs`
- **Patrón**: Entity dentro del agregado
- **Características**:
  - Entidad hija del agregado Customer
  - Encapsula información de contacto
  - Mantiene estado de verificación
  - Solo accesible a través del agregado raíz

**Encapsulación**:
```csharp
// El Customer expone la información de contacto mediante facade
public EMailAddress Email => ContactInformation.Email;
public PhoneNumber? PhoneNumber => ContactInformation.PhoneNumber;
public bool IsContactVerified => ContactInformation.IsVerified;
```

### Value Objects

Los Value Objects representan conceptos del dominio sin identidad:

#### CustomerName
- **Archivo**: `Domain/ValueObjects/CustomerName.cs`
- **Tipo**: `sealed record`
- **Características**:
  - Compuesto por FirstName y LastName
  - Expone FullName como propiedad calculada
  - Validación en constructor
  - Inmutable

#### EMailAddress
- **Archivo**: `Domain/ValueObjects/EMailAddress.cs`
- **Tipo**: `readonly record struct`
- **Características**:
  - Validación de formato básico
  - Inmutable
  - Eficiente (struct)

#### PhoneNumber
- **Archivo**: `Domain/ValueObjects/PhoneNumber.cs`
- **Características**:
  - Separación de código de país (prefix) y número
  - Validación de formato

### CustomerId
- **Archivo**: `Domain/CustomerId.cs`
- **Patrón**: Strongly Typed ID
- **Características**:
  - Hereda de `AggregateId`
  - Validaciones específicas
  - Type-safety para IDs de Customer

## Capa de Infraestructura (Infrastructure Layer)

La capa de infraestructura proporciona implementaciones técnicas de los conceptos del dominio.

### Repository Pattern

#### ICustomerRepository
- **Archivo**: `Infrastructure/ICustomerRepository.cs`
- **Patrón**: Repository
- **Propósito**: Abstracción de la persistencia
- **Operaciones**:
  - `GetByIdAsync()`: Obtener por ID tipado
  - `GetByEmailAsync()`: Obtener por email (Value Object)
  - `AddAsync()`: Agregar nuevo cliente
  - `UpdateAsync()`: Actualizar cliente existente
  - `DeleteAsync()`: Eliminar cliente

**Ventajas**:
- Oculta detalles de persistencia del dominio
- Permite testing sin base de datos
- Trabaja con objetos del dominio, no DTOs

### Entity Framework Core Configuration

#### CustomerConfigurationMapping
- **Archivo**: `Infrastructure/Configuration/CustomerConfigurationMapping.cs`
- **Propósito**: Mapeo de Customer a base de datos
- **Características**:
  - Configuración de tabla y relaciones
  - Conversión de Value Objects a columnas
  - Mapeo de identidades tipadas

#### ContactInformationConfigurationMapping
- **Archivo**: `Infrastructure/Configuration/ContactInformationConfigurationMapping.cs`
- **Propósito**: Mapeo de ContactInformation
- **Características**:
  - Mapeo de entidad hija
  - Relación con Customer

### CustomersDbContext
- **Archivo**: `Infrastructure/CustomersDbContext.cs`
- **Propósito**: DbContext de Entity Framework
- **Características**:
  - Configuración de DbSets
  - Aplicación de configuraciones
  - Gestión de transacciones

## Principios de DDD Aplicados

### 1. Ubiquitous Language (Lenguaje Ubicuo)
Los nombres de clases y métodos reflejan el lenguaje del negocio:
- `Customer`, `ContactInformation`, `CustomerName`
- `VerifyContact()`, `SoftDelete()`, `Restore()`

### 2. Bounded Context (Contexto Delimitado)
El proyecto representa el contexto de "Customers" con límites claros.

### 3. Aggregate Pattern
- **Customer** es la raíz del agregado
- **ContactInformation** es parte del agregado
- Solo se puede acceder a ContactInformation a través de Customer
- Las transacciones se realizan sobre el agregado completo

### 4. Entity vs Value Object
- **Entities**: Customer, ContactInformation (tienen identidad)
- **Value Objects**: CustomerName, EMailAddress, PhoneNumber (no tienen identidad)

### 5. Repository Pattern
Abstracción de persistencia que trabaja con agregados completos.

### 6. Encapsulation (Encapsulación)
- Setters privados
- Modificación solo a través de métodos de negocio
- Invariantes protegidas

### 7. Domain Events (preparado para)
La infraestructura incluye `IHaveDomainEvents` para eventos de dominio.

### 8. Strongly Typed IDs
Uso de `CustomerId` en lugar de `long` para type-safety.

## Beneficios de Esta Arquitectura

1. **Separación de Responsabilidades**: Clara división entre dominio e infraestructura
2. **Testabilidad**: El dominio se puede testear sin dependencias de infraestructura
3. **Mantenibilidad**: Cambios en persistencia no afectan el dominio
4. **Expresividad**: El código refleja el lenguaje del negocio
5. **Integridad**: Los agregados protegen sus invariantes
6. **Evolución**: Fácil agregar nuevos comportamientos o reglas de negocio
7. **Type Safety**: Los IDs tipados previenen errores comunes

## Patrones y Prácticas

- **Factory Pattern**: Método `Create()` en Customer
- **Repository Pattern**: Abstracción de persistencia
- **Aggregate Pattern**: Customer como raíz de agregado
- **Value Object Pattern**: CustomerName, EMailAddress, PhoneNumber
- **Strongly Typed IDs**: CustomerId
- **Soft Delete Pattern**: IHaveSoftDelete
- **Audit Pattern**: IHaveAudit
- **Guard Clauses**: Validaciones defensivas con `Ardalis.GuardClauses`

## Flujo de Trabajo Típico

1. **Crear un Customer**:
   ```csharp
   var customer = Customer.Create(firstName, lastName, email, phoneNumber);
   await repository.AddAsync(customer);
   ```

2. **Modificar un Customer**:
   ```csharp
   var customer = await repository.GetByIdAsync(customerId);
   customer.UpdateEmail(newEmail);
   await repository.UpdateAsync(customer);
   ```

3. **Verificar Contacto**:
   ```csharp
   var customer = await repository.GetByIdAsync(customerId);
   customer.VerifyContact();
   await repository.UpdateAsync(customer);
   ```

## Conclusión

Este proyecto demuestra una implementación completa de Domain-Driven Design, aplicando patrones tácticos y estratégicos que resultan en un código mantenible, testeable y que refleja fielmente las reglas del negocio. La separación clara entre dominio e infraestructura permite evolucionar el sistema de manera sostenible.
