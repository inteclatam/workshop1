# Domain-Driven Design (DDD) - Customers
<img src="https://img.shields.io/badge/.NET-5C2D91?style=badge&logo=.net&logoColor=white" >

Este proyecto implementa una arquitectura basada en **Domain-Driven Design (DDD)** para la gestión de clientes, siguiendo los patrones tácticos y estratégicos de DDD.

## 📋 Tabla de Contenidos

- [Estructura del Proyecto](#estructura-del-proyecto)
- [Capa de Primitives (Building Blocks)](#capa-de-primitives-building-blocks)
- [Capa de Dominio](#capa-de-dominio-domain-layer)
- [Capa de Infraestructura](#capa-de-infraestructura-infrastructure-layer)
- [Capa de Aplicación](#capa-de-aplicación-application-layer)
- [Domain Events](#domain-events)
- [Logging y Observabilidad](#logging-y-observabilidad)
- [Configuración con Variables de Entorno](#configuración-con-variables-de-entorno-env)
- [Tecnologías y Librerías](#tecnologías-y-librerías)
- [Ejecución del Proyecto](#ejecución-del-proyecto)
- [Beneficios de Esta Arquitectura](#beneficios-de-esta-arquitectura-completa)

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

### 7. Domain Events
Sistema completo de eventos de dominio para capturar eventos importantes del negocio.

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

## Capa de Aplicación (Application Layer)

La capa de aplicación orquesta la lógica de negocio del dominio y coordina el flujo de datos entre la capa de presentación y el dominio.

### CQRS Pattern (Command Query Responsibility Segregation)

El proyecto implementa el patrón CQRS para separar las operaciones de lectura (Queries) de las operaciones de escritura (Commands).

#### Primitives CQRS

**ICommand / ICommandHandler**:
- **Archivos**: `Primitives/ICommand.cs`, `Primitives/ICommandHandler.cs`
- **Propósito**: Definir comandos (operaciones que modifican estado)
```csharp
public interface ICommand<TResponse> { }

public interface ICommandHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
    Task<TResponse> HandleAsync(TCommand command, CancellationToken ct = default);
}
```

**IQuery / IQueryHandler**:
- **Archivos**: `Primitives/IQuery.cs`, `Primitives/IQueryHandler.cs`
- **Propósito**: Definir queries (operaciones de solo lectura)

**CommandDispatcher / QueryDispatcher**:
- **Archivo**: `Primitives/Dispatcher.cs`
- **Propósito**: Despachar comandos y queries a sus handlers correspondientes
- **Características**:
  - Resolución dinámica de handlers mediante reflection
  - Integración con el contenedor de DI
  - Ejecución asíncrona

### Feature: CreateCustomer

Ejemplo completo de implementación CQRS para la creación de clientes.

**CreateCustomerCommand**:
- **Archivo**: `Application/Features/CreateCustomer/CreateCustomerCommand.cs`
- **Tipo**: Command
- **Propósito**: Encapsula los datos necesarios para crear un cliente

**CreateCustomerCommandHandler**:
- **Archivo**: `Application/Features/CreateCustomer/CreateCustomerCommandHandler.cs`
- **Responsabilidades**:
  - Generar IDs únicos usando SnowflakeIdGenerator
  - Crear la entidad Customer usando el factory method del dominio
  - Persistir usando Repository y Unit of Work
  - Logging de operaciones

**CreateCustomerValidator**:
- **Archivo**: `Application/Features/CreateCustomer/CreateCustomerValidator.cs`
- **Propósito**: Validación de datos de entrada usando FluentValidation

**CreateCustomerEndpoint**:
- **Archivo**: `Application/Features/CreateCustomer/CreateCustomerEndpoint.cs`
- **Tipo**: Minimal API Endpoint
- **Características**:
  - Mapeo HTTP POST
  - Integración con CommandDispatcher
  - Documentación OpenAPI

**Flujo Completo**:
```
HTTP Request → Endpoint → Validator → Command → CommandHandler → Domain → Repository → UnitOfWork → Database
```

## Infraestructura Avanzada

### Unit of Work Pattern

**IUnitOfWork / UnitOfWork**:
- **Archivos**: `Infrastructure/IUnitOfWork.cs`, `Infrastructure/UnitOfWork.cs`
- **Propósito**: Coordinar múltiples operaciones de repositorio en una sola transacción
- **Operaciones**:
  - `SaveChangesAsync()`: Guardar cambios pendientes
  - `BeginTransactionAsync()`: Iniciar transacción explícita
  - `CommitTransactionAsync()`: Confirmar transacción
  - `RollbackTransactionAsync()`: Revertir transacción

**Ventajas**:
- Garantiza consistencia transaccional
- Simplifica el manejo de transacciones
- Separación de responsabilidades con Repository

### SnowflakeId Generator

Sistema de generación de IDs distribuidos basado en el algoritmo Snowflake de Twitter.

**IIdGenerator / SnowflakeIdGenerator**:
- **Archivos**: `Infrastructure/SnowflakeId/IIdGenerator.cs`, `Infrastructure/SnowflakeId/SnowflakeIdGenerator.cs`
- **Propósito**: Generar IDs únicos de 64 bits de forma distribuida
- **Características**:
  - IDs ordenados cronológicamente
  - Garantía de unicidad en sistemas distribuidos
  - Alto rendimiento (millones de IDs por segundo)

**IdGeneratorOptions**:
- **Archivo**: `Infrastructure/SnowflakeId/IdGeneratorOptions.cs`
- **Configuración**:
  - `WorkerId`: Identificador del worker (0-31)
  - `DatacenterId`: Identificador del datacenter (0-31)

**IIdGeneratorPool / DefaultIdGeneratorPool**:
- **Archivos**: `Infrastructure/SnowflakeId/IIdGeneratorPool.cs`, `Infrastructure/SnowflakeId/DefaultIdGeneratorPool.cs`
- **Propósito**: Pool de generadores para alta concurrencia

**Estructura del ID**:
```
64 bits = 1 bit (unused) + 41 bits (timestamp) + 5 bits (datacenter) + 5 bits (worker) + 12 bits (sequence)
```

### Manejo Global de Excepciones

**GlobalExceptionHandler**:
- **Archivo**: `Infrastructure/Exceptions/GlobalException.cs`
- **Patrón**: Exception Handler Middleware
- **Características**:
  - Captura todas las excepciones no manejadas
  - Logging centralizado de errores
  - Respuestas HTTP estandarizadas (Problem Details)
  - Manejo especial de ValidationException de FluentValidation
  - Códigos de estado HTTP apropiados

**Tipos de Excepciones Manejadas**:
- `ValidationException`: 400 Bad Request con detalles de validación
- Otras excepciones: 500 Internal Server Error

### Validación con FluentValidation

**ValidationFilter**:
- **Archivo**: `Infrastructure/Filters/ValidationFilter.cs`
- **Propósito**: Filtro para validación de requests

**Validadores Registrados**:
- Los validadores se registran automáticamente desde el assembly
- Integración con el pipeline de ASP.NET Core

## Domain Events

El proyecto implementa un sistema completo de eventos de dominio para capturar y comunicar eventos importantes del negocio.

### Infraestructura de Domain Events

**IDomainEvent**:
- **Archivo**: `Primitives/IDomainEvent.cs`
- **Propósito**: Interface base para todos los eventos de dominio
- **Propiedades**:
  - `EventId`: Identificador único del evento (Guid)
  - `OccurredOn`: Timestamp de cuándo ocurrió el evento (DateTime UTC)

**DomainEvent**:
- **Archivo**: `Primitives/DomainEvent.cs`
- **Propósito**: Clase base abstracta para eventos de dominio
- **Características**:
  - Genera automáticamente `EventId` único
  - Registra timestamp de ocurrencia
  - Herencia para todos los eventos específicos

**IHaveDomainEvents**:
- **Archivo**: `Primitives/IHaveDomainEvents.cs`
- **Propósito**: Interface para agregados que publican eventos
- **Métodos**:
  - `GetDomainEvents()`: Obtiene eventos pendientes
  - `ClearDomainEvents()`: Limpia eventos después de publicar
  - `RaiseDomainEvent()`: Registra un nuevo evento

**Aggregate (actualizado)**:
- **Archivo**: `Primitives/Aggregate.cs`
- **Propósito**: Clase base para agregados con soporte de eventos
- **Características**:
  - Lista interna de eventos de dominio
  - Implementa `IHaveDomainEvents`
  - Protege la colección de eventos (private set)

### Eventos del Dominio de Customers

El dominio de Customers publica los siguientes eventos:

**CustomerCreatedEvent**:
- **Archivo**: `Domain/Events/CustomerCreatedEvent.cs`
- **Se dispara cuando**: Se crea un nuevo cliente
- **Propiedades**: CustomerId, FirstName, LastName, Email, PhoneNumber

**CustomerNameChangedEvent**:
- **Archivo**: `Domain/Events/CustomerNameChangedEvent.cs`
- **Se dispara cuando**: Se actualiza el nombre de un cliente
- **Propiedades**: CustomerId, OldName, NewName

**CustomerDeletedEvent**:
- **Archivo**: `Domain/Events/CustomerDeletedEvent.cs`
- **Se dispara cuando**: Se elimina (soft delete) un cliente
- **Propiedades**: CustomerId

**CustomerRestoredEvent**:
- **Archivo**: `Domain/Events/CustomerRestoredEvent.cs`
- **Se dispara cuando**: Se restaura un cliente eliminado
- **Propiedades**: CustomerId

**ContactInformationAddedEvent**:
- **Archivo**: `Domain/Events/ContactInformationAddedEvent.cs`
- **Se dispara cuando**: Se agrega información de contacto
- **Propiedades**: CustomerId, ContactId, Email, PhoneNumber

**ContactEmailUpdatedEvent**:
- **Archivo**: `Domain/Events/ContactEmailUpdatedEvent.cs`
- **Se dispara cuando**: Se actualiza el email de contacto
- **Propiedades**: CustomerId, OldEmail, NewEmail

**ContactPhoneUpdatedEvent**:
- **Archivo**: `Domain/Events/ContactPhoneUpdatedEvent.cs`
- **Se dispara cuando**: Se actualiza el teléfono de contacto
- **Propiedades**: CustomerId, OldPhone, NewPhone

**ContactVerifiedEvent**:
- **Archivo**: `Domain/Events/ContactVerifiedEvent.cs`
- **Se dispara cuando**: Se verifica el contacto de un cliente
- **Propiedades**: CustomerId

**ContactRemovedEvent**:
- **Archivo**: `Domain/Events/ContactRemovedEvent.cs`
- **Se dispara cuando**: Se elimina información de contacto
- **Propiedades**: CustomerId, ContactId

**PrimaryContactChangedEvent**:
- **Archivo**: `Domain/Events/PrimaryContactChangedEvent.cs`
- **Se dispara cuando**: Se cambia el contacto principal
- **Propiedades**: CustomerId, OldContactId, NewContactId

### Uso de Domain Events en el Aggregate

```csharp
public class Customer : Aggregate<CustomerId>
{
    public static Customer Create(...)
    {
        var customer = new Customer(...);

        // Registrar evento de dominio
        customer.RaiseDomainEvent(new CustomerCreatedEvent(
            customer.Id.Value,
            customer.Name.FirstName,
            customer.Name.LastName,
            customer.Email.Value,
            customer.PhoneNumber?.ToString()
        ));

        return customer;
    }

    public void UpdateName(string firstName, string lastName)
    {
        var oldName = Name;
        Name = new CustomerName(firstName, lastName);

        // Registrar evento cuando cambia el nombre
        RaiseDomainEvent(new CustomerNameChangedEvent(
            Id.Value,
            $"{oldName.FirstName} {oldName.LastName}",
            Name.FullName
        ));
    }
}
```

### Beneficios de Domain Events

1. **Desacoplamiento**: Los agregados no necesitan conocer los efectos secundarios de sus acciones
2. **Auditabilidad**: Todos los cambios importantes quedan registrados
3. **Integraciones**: Otros bounded contexts pueden reaccionar a eventos
4. **Event Sourcing Ready**: Base para implementar Event Sourcing si se necesita
5. **Trazabilidad**: Historia completa de qué pasó en el dominio
6. **Procesamiento Asíncrono**: Los eventos pueden procesarse de forma asíncrona

### Data Seeding

**DatabaseSeeder**:
- **Archivo**: `Infrastructure/Configuration/DatabaseSeeder.cs`
- **Propósito**: Inicialización de base de datos

**CustomerSeeder**:
- **Archivo**: `Infrastructure/Data/CustomerSeeder.cs`
- **Propósito**: Generar datos de prueba usando Bogus
- **Características**:
  - Generación de datos realistas
  - Configurable para diferentes entornos

**SeedDataEndpoint**:
- **Archivo**: `Application/Features/SeedData/SeedDataEndpoint.cs`
- **Propósito**: Endpoint para ejecutar seeding manualmente
- **Disponibilidad**: Solo en modo Development

## Inyección de Dependencias

**DependencyInjection.cs**:
- **Archivo**: `DependencyInjection.cs`
- **Propósito**: Configuración centralizada de servicios

**Servicios Registrados**:
1. **Database Context**: CustomersDbContext con PostgreSQL
2. **ID Generator**: Configuración de SnowflakeIdGenerator
3. **Repositories**: ICustomerRepository → CustomerRepository
4. **Unit of Work**: IUnitOfWork → UnitOfWork
5. **Data Seeder**: CustomerSeeder
6. **CQRS Dispatchers**: CommandDispatcher, QueryDispatcher
7. **Handlers**: Registro automático de todos los Command/Query Handlers
8. **Validators**: FluentValidation validators
9. **Filters**: ValidationFilter

**Registro Automático de Handlers**:
```csharp
// Escanea el assembly y registra todos los handlers automáticamente
RegisterHandlers(services, typeof(ICommandHandler<,>));
RegisterHandlers(services, typeof(IQueryHandler<,>));
```

## Logging y Observabilidad

### Serilog

**Configuración**:
- **Archivo**: `Program.cs`
- **Sinks**:
  - **Spectre.Console**: Salida coloreada y formateada en consola
  - **Seq**: Plataforma de análisis y búsqueda de logs estructurados
- **Características**:
  - Logging estructurado
  - Configuración desde appsettings.json
  - Integración con ASP.NET Core
  - Enriquecimiento automático (MachineName, ProcessId, ThreadId, etc.)

**Configuración Serilog en appsettings.Development.json**:
```json
{
  "Serilog": {
    "MinimumLevel": "Debug",
    "Enrich": [
      "FromLogContext",
      "WithMachineName",
      "WithEnvironmentName",
      "WithProcessId",
      "WithProcessName",
      "WithThreadId"
    ],
    "WriteTo": [
      {
        "Name": "Spectre",
        "Args": {
          "outputTemplate": "[{Timestamp:HH:mm:ss.fff} {Level:u3}] {Message:lj}{NewLine}{Exception}"
        }
      },
      {
        "Name": "Seq",
        "Args": {
          "serverUrl": "http://localhost:5341",
          "apiKey": ""
        }
      }
    ]
  }
}
```

**Logging en Handlers**:
```csharp
_logger.LogInformation("Creating customer with ID: {CustomerId}", customerId);
```

### Seq - Structured Log Server

**Seq** es una plataforma de análisis de logs que permite:
- Búsqueda y filtrado avanzado de logs
- Visualización de logs estructurados
- Análisis en tiempo real
- Alertas y notificaciones
- Dashboards personalizados

**Configuración**:
- **URL por defecto**: `http://localhost:5341`
- **Paquete NuGet**: `Serilog.Sinks.Seq`
- **Integración**: Automática mediante configuración

**Ejecutar Seq con Docker**:
```bash
docker run --name seq -d --restart unless-stopped \
  -e ACCEPT_EULA=Y \
  -p 5341:80 \
  datalust/seq:latest
```

**Acceso a UI de Seq**:
```
http://localhost:5341
```

### Spectre.Console

**Display Header**:
- **Archivo**: `Program.cs`
- **Propósito**: Banner ASCII art en el inicio de la aplicación
- **Características**: Salida coloreada y formateada

## API Documentation

### Scalar OpenAPI

**Configuración**:
- **Archivo**: `Program.cs`
- **Características**:
  - Documentación interactiva de la API
  - Generación automática desde endpoints
  - UI moderna para explorar la API
  - Solo disponible en Development

**Acceso**:
```
GET /scalar/v1
```

### OpenAPI

- Generación automática de especificación OpenAPI
- Integración con Minimal APIs
- Documentación de requests/responses

## Configuración y Startup

### Program.cs

**Características Principales**:
1. **Configuración**:
   - appsettings.json
   - Variables de entorno

2. **Servicios Registrados**:
   - OpenAPI
   - Infrastructure (vía DependencyInjection)
   - Exception Handling
   - Serilog Logging

3. **Middleware Pipeline**:
   - Exception Handler
   - OpenAPI/Scalar (Development)
   - Custom Endpoints

4. **Endpoints Mapeados**:
   - `MapCreateCustomerEndpoint()`: Creación de clientes
   - `MapSeedDataEndpoint()`: Seeding (solo Development)

## Tecnologías y Librerías

### Paquetes NuGet Principales

**Domain & Infrastructure**:
- `Ardalis.GuardClauses` (5.0.0): Guard clauses para validaciones
- `Microsoft.EntityFrameworkCore` (9.0.11): ORM
- `Npgsql.EntityFrameworkCore.PostgreSQL` (9.0.4): Provider PostgreSQL

**CQRS & Validation**:
- `FluentValidation` (12.1.1): Validación fluida
- `FluentValidation.DependencyInjectionExtensions` (11.11.1): Integración DI

**API & Documentation**:
- `Microsoft.AspNetCore.OpenApi` (10.0.0-rc.2): OpenAPI
- `Scalar.AspNetCore` (2.11.6): Documentación API interactiva

**Logging & Console**:
- `Serilog` (4.3.0): Logging estructurado
- `Serilog.Extensions.Hosting` (10.0.0): Integración hosting
- `Serilog.Settings.Configuration` (10.0.0): Configuración
- `Serilog.Sinks.Spectre` (0.5.0): Sink para Spectre.Console
- `Serilog.Sinks.Seq` (9.0.0): Sink para servidor Seq
- `Spectre.Console` (0.54.0): Consola rica y colorida

**Configuración**:
- `DotNetEnv` (3.1.1): Soporte para archivos .env

**Testing & Data Generation**:
- `Bogus` (35.6.1): Generación de datos falsos

### Framework

- **.NET 10.0**: Versión más reciente
- **ASP.NET Core**: Framework web
- **Minimal APIs**: Endpoints ligeros sin controllers

## Arquitectura en Capas Actualizada

```
┌─────────────────────────────────────────────────────────┐
│                    Presentation Layer                    │
│  (Minimal API Endpoints, OpenAPI, Scalar Documentation) │
└────────────────────┬────────────────────────────────────┘
                     │
┌────────────────────▼────────────────────────────────────┐
│                   Application Layer                      │
│         (CQRS: Commands, Queries, Handlers,             │
│          Validators, DTOs, Dispatchers)                 │
└────────────────────┬────────────────────────────────────┘
                     │
┌────────────────────▼────────────────────────────────────┐
│                     Domain Layer                         │
│    (Aggregates, Entities, Value Objects, Domain Logic,  │
│              Business Rules, Invariants)                │
└────────────────────┬────────────────────────────────────┘
                     │
┌────────────────────▼────────────────────────────────────┐
│                Infrastructure Layer                      │
│  (EF Core, Repositories, Unit of Work, DbContext,       │
│   ID Generation, Exception Handling, Data Seeding)      │
└─────────────────────────────────────────────────────────┘
```

## Patrones y Prácticas Adicionales

- **CQRS Pattern**: Separación de Commands y Queries
- **Unit of Work Pattern**: Gestión transaccional
- **Dispatcher Pattern**: Mediación entre endpoints y handlers
- **Domain Events Pattern**: Eventos de dominio para comunicación entre agregados
- **Global Exception Handling**: Manejo centralizado de errores
- **Dependency Injection**: IoC container para todas las dependencias
- **Distributed ID Generation**: Snowflake algorithm
- **Data Seeding**: Generación automatizada de datos de prueba
- **Structured Logging**: Serilog con logging estructurado y Seq
- **API Documentation**: OpenAPI/Scalar para documentación interactiva
- **Environment Configuration**: Dotenv para configuración flexible
- **Web Server Configuration**: Kestrel con puerto configurable

## Flujo de Trabajo Completo: Crear un Customer

### 1. Request HTTP
```http
POST /customers
Content-Type: application/json

{
  "firstName": "Juan",
  "lastName": "Pérez",
  "email": "juan.perez@example.com",
  "phoneNumber": "+5491123456789"
}
```

### 2. Endpoint (Presentation)
```csharp
app.MapPost("/customers", async (
    CreateCustomerRequest request,
    CommandDispatcher dispatcher,
    CancellationToken ct) =>
{
    var command = new CreateCustomerCommand(
        request.FirstName,
        request.LastName,
        request.Email,
        request.PhoneNumber
    );

    var response = await dispatcher.DispatchAsync<CreateCustomerResponse>(command, ct);
    return Results.Created($"/customers/{response.CustomerId}", response);
});
```

### 3. Validación (Application)
```csharp
// CreateCustomerValidator valida el comando
// Si falla, lanza ValidationException
// GlobalExceptionHandler la captura y retorna 400
```

### 4. Command Handler (Application)
```csharp
public async Task<CreateCustomerResponse> HandleAsync(
    CreateCustomerCommand command,
    CancellationToken ct)
{
    // Genera IDs únicos con Snowflake
    var customerId = _idGenerator.GenerateId();
    var contactId = _idGenerator.GenerateId();

    // Crea el agregado usando factory method del dominio
    var customer = Customer.Create(
        customerId, contactId,
        command.FirstName, command.LastName,
        command.Email, command.PhoneNumber
    );

    // Persiste usando Repository
    await _repository.AddAsync(customer, ct);

    // Confirma cambios con Unit of Work
    await _unitOfWork.SaveChangesAsync(ct);

    return new CreateCustomerResponse(...);
}
```

### 5. Dominio
```csharp
// Customer.Create() valida invariantes
// Crea Value Objects (CustomerName, EmailAddress, PhoneNumber)
// Retorna agregado válido
```

### 6. Repository (Infrastructure)
```csharp
public async Task AddAsync(Customer customer, CancellationToken ct)
{
    await _context.Customers.AddAsync(customer, ct);
}
```

### 7. Unit of Work (Infrastructure)
```csharp
public async Task<int> SaveChangesAsync(CancellationToken ct)
{
    return await _context.SaveChangesAsync(ct);
}
```

### 8. Response
```json
{
  "fullName": "Juan Pérez",
  "email": "juan.perez@example.com",
  "customerId": 1234567890123456789
}
```

## Configuración con Variables de Entorno (.env)

El proyecto soporta configuración mediante archivos `.env` para facilitar el desarrollo local y el despliegue.

### Soporte de DotEnv

**Paquete**: `DotNetEnv`
- **Propósito**: Cargar variables de entorno desde archivos .env
- **Ubicación de archivos**:
  - `src/.env`: Configuración de desarrollo local
  - `deploy/.env`: Configuración de despliegue

**Archivo src/.env**:
```bash
KESTREL_PORT=5002
APP_NAME=CUSTOMERS
```

**Carga en Program.cs**:
```csharp
// Cargar variables de entorno desde archivo .env
DotNetEnv.Env.Load();
```

### Configuración de Kestrel

El servidor web Kestrel se configura mediante variables de entorno:

**Puerto Configurable**:
- Variable de entorno: `KESTREL_PORT`
- Puerto por defecto: `5002`
- Permite cambiar el puerto sin modificar código

**Configuración en Program.cs**:
```csharp
builder.WebHost.ConfigureKestrel(options =>
{
    var port = Environment.GetEnvironmentVariable("KESTREL_PORT");
    if (!string.IsNullOrEmpty(port) && int.TryParse(port, out var portNumber))
    {
        options.ListenAnyIP(portNumber);
    }
});
```

**Ventajas**:
- Configuración flexible por entorno
- Sin hardcodear puertos en el código
- Facilita despliegues en contenedores
- Compatible con orquestadores (Docker, Kubernetes)

## Configuración del Proyecto

### appsettings.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=customers;Username=user;Password=pass"
  },
  "IdGenerator": {
    "WorkerId": 1,
    "DatacenterId": 1
  },
  "Serilog": {
    "MinimumLevel": "Information",
    "WriteTo": [
      {
        "Name": "Spectre"
      }
    ]
  }
}
```

## Ejecución del Proyecto

### Prerequisitos
- .NET 10.0 SDK
- PostgreSQL 15+
- Docker (opcional, para Seq)

### Comandos
```bash
# Restaurar paquetes
dotnet restore

# Ejecutar migraciones
dotnet ef database update

# Opcional: Iniciar Seq para logs (con Docker)
docker run --name seq -d --restart unless-stopped \
  -e ACCEPT_EULA=Y \
  -p 5341:80 \
  datalust/seq:latest

# Ejecutar proyecto
dotnet run

# El puerto se configura desde src/.env (KESTREL_PORT=5002)
# Acceder a la aplicación: http://localhost:5002

# Acceder a documentación API
# http://localhost:5002/scalar/v1

# Acceder a Seq (logs)
# http://localhost:5341

# Seed de datos (Development)
# POST http://localhost:5002/seed
```

### Variables de Entorno

El proyecto usa un archivo `.env` en `src/.env` para configuración local:

```bash
KESTREL_PORT=5002        # Puerto del servidor web
APP_NAME=CUSTOMERS        # Nombre de la aplicación
```

Puedes modificar estas variables según tus necesidades sin tocar el código.

## Beneficios de Esta Arquitectura Completa

1. **Separación Clara de Responsabilidades**: Cada capa tiene un propósito bien definido
2. **Testabilidad Extrema**: Todas las dependencias son inyectadas e intercambiables
3. **Mantenibilidad**: Cambios aislados por capa y feature
4. **Escalabilidad**: CQRS permite escalar lectura y escritura independientemente
5. **Observabilidad Avanzada**: Logging estructurado con Serilog y Seq para análisis en tiempo real
6. **Trazabilidad Completa**: Domain Events capturan todos los cambios importantes del negocio
7. **Consistencia**: Unit of Work garantiza transacciones ACID
8. **Performance**: IDs distribuidos sin dependencia de secuencias de BD
9. **Documentación**: OpenAPI generada automáticamente
10. **Validación Robusta**: FluentValidation con reglas expresivas
11. **Developer Experience**: Scalar UI para explorar y probar la API
12. **Configuración Flexible**: Variables de entorno con dotenv para diferentes ambientes
13. **Desacoplamiento**: Domain Events permiten reaccionar a cambios sin acoplar código
14. **Auditabilidad**: Eventos de dominio proporcionan historia completa de cambios
15. **Portabilidad**: Configuración con variables de entorno facilita despliegues

## Conclusión

Este proyecto demuestra una implementación completa de **Domain-Driven Design** con arquitectura en capas, aplicando patrones tácticos y estratégicos modernos.

### Características Principales:

**Patrones de Dominio**:
- **CQRS** para separación de responsabilidades
- **Domain Events** para comunicación desacoplada entre agregados
- **Unit of Work** para consistencia transaccional
- **Repository Pattern** para abstracción de persistencia

**Infraestructura Robusta**:
- **Snowflake ID Generation** para IDs distribuidos de alto rendimiento
- **Serilog con Seq** para observabilidad y análisis de logs en tiempo real
- **FluentValidation** para validación expresiva y mantenible
- **Global Exception Handling** para manejo centralizado de errores

**Configuración Flexible**:
- **DotEnv** para configuración por entorno
- **Kestrel** con puerto configurable
- **Múltiples sinks de logging** (Console y Seq)

**Calidad de Código**:
- Separación clara entre dominio, aplicación e infraestructura
- Inyección de dependencias en todos los niveles
- Logging estructurado y trazable
- Eventos de dominio para auditabilidad completa
- Validación en múltiples capas

El resultado es un **código altamente mantenible, testeable, observable y escalable** que refleja fielmente las reglas del negocio y permite evolucionar el sistema de manera sostenible. La incorporación de Domain Events y observabilidad avanzada con Seq proporciona trazabilidad completa de todos los cambios en el sistema, facilitando debugging, auditoría y análisis de comportamiento.
