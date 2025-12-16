CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;
DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'customers') THEN
        CREATE SCHEMA customers;
    END IF;
END $EF$;

CREATE TABLE customers."Customers" (
    "Id" bigint NOT NULL,
    "FirstName" character varying(100) NOT NULL,
    "LastName" character varying(100) NOT NULL,
    "Created" timestamp with time zone NOT NULL,
    "CreatedBy" integer,
    "LastModified" timestamp with time zone,
    "LastModifiedBy" integer,
    "IsDeleted" boolean NOT NULL DEFAULT FALSE,
    "Deleted" timestamp with time zone,
    "DeletedBy" integer,
    CONSTRAINT "PK_Customers" PRIMARY KEY ("Id")
);

CREATE TABLE customers."ContactInformation" (
    "Id" bigint NOT NULL,
    "Email" character varying(255) NOT NULL,
    "PhoneValue" character varying(30),
    "PhoneNumber" character varying(20),
    "PhonePrefix" character varying(10),
    "IsVerified" boolean NOT NULL DEFAULT FALSE,
    "IsPrimary" boolean NOT NULL DEFAULT FALSE,
    "CustomerId" bigint NOT NULL,
    CONSTRAINT "PK_ContactInformation" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_ContactInformation_Customers_CustomerId" FOREIGN KEY ("CustomerId") REFERENCES customers."Customers" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_ContactInformation_CustomerId" ON customers."ContactInformation" ("CustomerId");

CREATE UNIQUE INDEX "IX_ContactInformation_Email" ON customers."ContactInformation" ("Email");

CREATE INDEX "IX_ContactInformation_IsPrimary" ON customers."ContactInformation" ("IsPrimary");

CREATE INDEX "IX_Customers_IsDeleted" ON customers."Customers" ("IsDeleted");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20251216133607_InitialCreate', '9.0.11');

COMMIT;

