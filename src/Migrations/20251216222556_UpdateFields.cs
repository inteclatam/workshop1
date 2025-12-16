using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Intec.Workshop1.Customers.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "customers");

            migrationBuilder.CreateTable(
                name: "Customers",
                schema: "customers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<int>(type: "integer", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    Deleted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContactInformation",
                schema: "customers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    PhoneValue = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    PhoneNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    PhonePrefix = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    IsVerified = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsPrimary = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CustomerId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactInformation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContactInformation_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "customers",
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "Customers",
                columns: new[] { "Id", "Created", "CreatedBy", "Deleted", "DeletedBy", "LastModified", "LastModifiedBy" },
                values: new object[,]
                {
                    { 259446041282220032L, new DateTime(2025, 4, 17, 11, 17, 31, 625, DateTimeKind.Unspecified).AddTicks(3384), 52, null, null, null, null },
                    { 259446041319968768L, new DateTime(2025, 2, 3, 18, 24, 47, 144, DateTimeKind.Unspecified).AddTicks(7331), 59, null, null, null, null },
                    { 259446041319968771L, new DateTime(2024, 3, 27, 21, 54, 35, 243, DateTimeKind.Unspecified).AddTicks(7498), 34, null, null, null, null },
                    { 259446041324163072L, new DateTime(2024, 9, 10, 12, 42, 19, 398, DateTimeKind.Unspecified).AddTicks(5211), 55, null, null, null, null },
                    { 259446041324163075L, new DateTime(2023, 9, 28, 23, 49, 26, 751, DateTimeKind.Unspecified).AddTicks(9656), 85, null, null, null, null },
                    { 259446041324163079L, new DateTime(2024, 8, 15, 22, 54, 53, 354, DateTimeKind.Unspecified).AddTicks(9771), 68, null, null, null, null },
                    { 259446041324163081L, new DateTime(2025, 8, 14, 16, 58, 34, 489, DateTimeKind.Unspecified).AddTicks(4406), 62, null, null, null, null },
                    { 259446041324163084L, new DateTime(2025, 10, 31, 0, 13, 9, 226, DateTimeKind.Unspecified).AddTicks(6625), 45, null, null, null, null },
                    { 259446041324163087L, new DateTime(2025, 12, 3, 19, 49, 50, 795, DateTimeKind.Unspecified).AddTicks(3652), 40, null, null, null, null },
                    { 259446041324163091L, new DateTime(2025, 10, 12, 17, 22, 10, 714, DateTimeKind.Unspecified).AddTicks(5058), 49, null, null, null, null },
                    { 259446041324163094L, new DateTime(2025, 3, 8, 1, 34, 58, 311, DateTimeKind.Unspecified).AddTicks(5104), 84, null, null, null, null },
                    { 259446041324163096L, new DateTime(2025, 9, 3, 9, 25, 15, 259, DateTimeKind.Unspecified).AddTicks(1826), 73, null, null, null, null },
                    { 259446041324163099L, new DateTime(2024, 12, 15, 11, 53, 51, 640, DateTimeKind.Unspecified).AddTicks(7984), 22, null, null, null, null },
                    { 259446041324163103L, new DateTime(2024, 3, 2, 10, 47, 48, 580, DateTimeKind.Unspecified).AddTicks(3627), 11, null, null, null, null },
                    { 259446041324163106L, new DateTime(2024, 9, 19, 8, 44, 51, 330, DateTimeKind.Unspecified).AddTicks(9388), 50, null, null, null, null },
                    { 259446041324163110L, new DateTime(2023, 12, 5, 21, 59, 45, 335, DateTimeKind.Unspecified).AddTicks(1429), 91, null, null, null, null },
                    { 259446041324163114L, new DateTime(2025, 5, 9, 13, 15, 48, 795, DateTimeKind.Unspecified).AddTicks(8865), 85, null, null, null, null },
                    { 259446041324163116L, new DateTime(2024, 11, 18, 10, 49, 55, 96, DateTimeKind.Unspecified).AddTicks(1876), 25, null, null, null, null },
                    { 259446041324163119L, new DateTime(2024, 3, 20, 22, 39, 16, 750, DateTimeKind.Unspecified).AddTicks(4390), 61, null, null, null, null },
                    { 259446041324163121L, new DateTime(2023, 5, 27, 2, 45, 53, 948, DateTimeKind.Unspecified).AddTicks(724), 37, null, null, null, null },
                    { 259446041324163123L, new DateTime(2024, 5, 22, 15, 50, 40, 702, DateTimeKind.Unspecified).AddTicks(1084), 48, null, null, null, null },
                    { 259446041324163126L, new DateTime(2025, 9, 3, 17, 53, 51, 721, DateTimeKind.Unspecified).AddTicks(6258), 6, null, null, null, null },
                    { 259446041324163129L, new DateTime(2025, 3, 15, 0, 0, 19, 777, DateTimeKind.Unspecified).AddTicks(1270), 19, null, null, null, null },
                    { 259446041324163132L, new DateTime(2025, 2, 7, 13, 7, 10, 196, DateTimeKind.Unspecified).AddTicks(4659), 28, null, null, null, null },
                    { 259446041324163134L, new DateTime(2024, 9, 22, 13, 55, 14, 391, DateTimeKind.Unspecified).AddTicks(6619), 82, null, null, null, null },
                    { 259446041324163136L, new DateTime(2024, 10, 5, 4, 34, 36, 72, DateTimeKind.Unspecified).AddTicks(7843), 85, null, null, null, null },
                    { 259446041324163140L, new DateTime(2023, 2, 22, 16, 23, 27, 654, DateTimeKind.Unspecified).AddTicks(4428), 13, null, null, null, null },
                    { 259446041324163142L, new DateTime(2024, 12, 25, 5, 46, 8, 120, DateTimeKind.Unspecified).AddTicks(4096), 70, null, null, null, null },
                    { 259446041324163146L, new DateTime(2023, 2, 24, 16, 24, 23, 154, DateTimeKind.Unspecified).AddTicks(7784), 55, null, null, null, null },
                    { 259446041324163148L, new DateTime(2025, 11, 16, 22, 0, 27, 886, DateTimeKind.Unspecified).AddTicks(6368), 9, null, null, null, null },
                    { 259446041324163152L, new DateTime(2023, 4, 16, 12, 31, 50, 153, DateTimeKind.Unspecified).AddTicks(9733), 88, null, null, null, null },
                    { 259446041324163156L, new DateTime(2025, 4, 18, 8, 25, 6, 531, DateTimeKind.Unspecified).AddTicks(9251), 87, null, null, null, null },
                    { 259446041324163158L, new DateTime(2024, 10, 2, 15, 52, 58, 108, DateTimeKind.Unspecified).AddTicks(4615), 43, null, null, null, null },
                    { 259446041324163162L, new DateTime(2023, 10, 10, 15, 15, 25, 906, DateTimeKind.Unspecified).AddTicks(645), 70, null, null, null, null },
                    { 259446041324163164L, new DateTime(2025, 11, 14, 6, 26, 59, 892, DateTimeKind.Unspecified).AddTicks(3442), 51, null, null, null, null },
                    { 259446041324163168L, new DateTime(2024, 5, 8, 10, 33, 53, 826, DateTimeKind.Unspecified).AddTicks(837), 50, null, null, null, null },
                    { 259446041324163171L, new DateTime(2025, 12, 4, 4, 13, 44, 576, DateTimeKind.Unspecified).AddTicks(9665), 57, null, null, null, null },
                    { 259446041324163173L, new DateTime(2023, 4, 28, 2, 27, 22, 136, DateTimeKind.Unspecified).AddTicks(3978), 97, null, null, null, null },
                    { 259446041324163175L, new DateTime(2024, 11, 13, 1, 57, 59, 538, DateTimeKind.Unspecified).AddTicks(8232), 44, null, null, null, null },
                    { 259446041324163179L, new DateTime(2025, 4, 28, 17, 9, 21, 534, DateTimeKind.Unspecified).AddTicks(7596), 33, null, null, null, null },
                    { 259446041324163183L, new DateTime(2023, 8, 6, 18, 39, 26, 543, DateTimeKind.Unspecified).AddTicks(9829), 4, null, null, null, null },
                    { 259446041324163187L, new DateTime(2024, 6, 22, 9, 39, 34, 60, DateTimeKind.Unspecified).AddTicks(1993), 3, null, null, null, null },
                    { 259446041324163190L, new DateTime(2025, 11, 11, 15, 23, 55, 973, DateTimeKind.Unspecified).AddTicks(6189), 17, null, null, null, null },
                    { 259446041324163192L, new DateTime(2024, 7, 3, 10, 47, 23, 427, DateTimeKind.Unspecified).AddTicks(5838), 15, null, null, null, null },
                    { 259446041324163195L, new DateTime(2023, 10, 2, 9, 26, 24, 601, DateTimeKind.Unspecified).AddTicks(9968), 15, null, null, null, null },
                    { 259446041324163197L, new DateTime(2024, 8, 7, 12, 2, 49, 267, DateTimeKind.Unspecified).AddTicks(987), 16, null, null, null, null },
                    { 259446041324163201L, new DateTime(2023, 11, 11, 17, 6, 44, 871, DateTimeKind.Unspecified).AddTicks(8149), 59, null, null, null, null },
                    { 259446041324163203L, new DateTime(2024, 9, 14, 17, 19, 58, 69, DateTimeKind.Unspecified).AddTicks(3411), 55, null, null, null, null },
                    { 259446041324163207L, new DateTime(2024, 6, 3, 8, 46, 48, 849, DateTimeKind.Unspecified).AddTicks(2988), 43, null, null, null, null },
                    { 259446041324163210L, new DateTime(2024, 8, 3, 9, 11, 50, 495, DateTimeKind.Unspecified).AddTicks(3852), 61, null, null, null, null }
                });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId", "IsPrimary", "IsVerified" },
                values: new object[] { 259446041290608640L, 259446041282220032L, true, true });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId", "IsPrimary" },
                values: new object[] { 259446041319968769L, 259446041319968768L, true });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId" },
                values: new object[] { 259446041319968770L, 259446041319968768L });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId", "IsPrimary", "IsVerified" },
                values: new object[,]
                {
                    { 259446041319968772L, 259446041319968771L, true, true },
                    { 259446041324163073L, 259446041324163072L, true, true }
                });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId" },
                values: new object[] { 259446041324163074L, 259446041324163072L });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId", "IsPrimary" },
                values: new object[] { 259446041324163076L, 259446041324163075L, true });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId" },
                values: new object[,]
                {
                    { 259446041324163077L, 259446041324163075L },
                    { 259446041324163078L, 259446041324163075L }
                });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId", "IsPrimary" },
                values: new object[] { 259446041324163080L, 259446041324163079L, true });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId", "IsPrimary", "IsVerified" },
                values: new object[] { 259446041324163082L, 259446041324163081L, true, true });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId" },
                values: new object[] { 259446041324163083L, 259446041324163081L });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId", "IsPrimary", "IsVerified" },
                values: new object[] { 259446041324163085L, 259446041324163084L, true, true });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId", "IsVerified" },
                values: new object[] { 259446041324163086L, 259446041324163084L, true });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId", "IsPrimary", "IsVerified" },
                values: new object[] { 259446041324163088L, 259446041324163087L, true, true });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId" },
                values: new object[,]
                {
                    { 259446041324163089L, 259446041324163087L },
                    { 259446041324163090L, 259446041324163087L }
                });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId", "IsPrimary" },
                values: new object[] { 259446041324163092L, 259446041324163091L, true });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId" },
                values: new object[] { 259446041324163093L, 259446041324163091L });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId", "IsPrimary" },
                values: new object[,]
                {
                    { 259446041324163095L, 259446041324163094L, true },
                    { 259446041324163097L, 259446041324163096L, true }
                });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId", "IsVerified" },
                values: new object[] { 259446041324163098L, 259446041324163096L, true });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId", "IsPrimary", "IsVerified" },
                values: new object[] { 259446041324163100L, 259446041324163099L, true, true });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId", "IsVerified" },
                values: new object[] { 259446041324163101L, 259446041324163099L, true });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId" },
                values: new object[] { 259446041324163102L, 259446041324163099L });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId", "IsPrimary", "IsVerified" },
                values: new object[] { 259446041324163104L, 259446041324163103L, true, true });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId" },
                values: new object[] { 259446041324163105L, 259446041324163103L });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId", "IsPrimary" },
                values: new object[] { 259446041324163107L, 259446041324163106L, true });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId" },
                values: new object[] { 259446041324163108L, 259446041324163106L });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId", "IsVerified" },
                values: new object[] { 259446041324163109L, 259446041324163106L, true });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId", "IsPrimary" },
                values: new object[] { 259446041324163111L, 259446041324163110L, true });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId", "IsVerified" },
                values: new object[] { 259446041324163112L, 259446041324163110L, true });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId" },
                values: new object[] { 259446041324163113L, 259446041324163110L });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId", "IsPrimary" },
                values: new object[] { 259446041324163115L, 259446041324163114L, true });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId", "IsPrimary", "IsVerified" },
                values: new object[] { 259446041324163117L, 259446041324163116L, true, true });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId" },
                values: new object[] { 259446041324163118L, 259446041324163116L });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId", "IsPrimary" },
                values: new object[] { 259446041324163120L, 259446041324163119L, true });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId", "IsPrimary", "IsVerified" },
                values: new object[] { 259446041324163122L, 259446041324163121L, true, true });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId", "IsPrimary" },
                values: new object[] { 259446041324163124L, 259446041324163123L, true });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId" },
                values: new object[] { 259446041324163125L, 259446041324163123L });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId", "IsPrimary", "IsVerified" },
                values: new object[] { 259446041324163127L, 259446041324163126L, true, true });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId" },
                values: new object[] { 259446041324163128L, 259446041324163126L });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId", "IsPrimary" },
                values: new object[] { 259446041324163130L, 259446041324163129L, true });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId" },
                values: new object[] { 259446041324163131L, 259446041324163129L });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId", "IsPrimary", "IsVerified" },
                values: new object[,]
                {
                    { 259446041324163133L, 259446041324163132L, true, true },
                    { 259446041324163135L, 259446041324163134L, true, true },
                    { 259446041324163137L, 259446041324163136L, true, true }
                });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId" },
                values: new object[,]
                {
                    { 259446041324163138L, 259446041324163136L },
                    { 259446041324163139L, 259446041324163136L }
                });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId", "IsPrimary", "IsVerified" },
                values: new object[] { 259446041324163141L, 259446041324163140L, true, true });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId", "IsPrimary" },
                values: new object[] { 259446041324163143L, 259446041324163142L, true });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId" },
                values: new object[,]
                {
                    { 259446041324163144L, 259446041324163142L },
                    { 259446041324163145L, 259446041324163142L }
                });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId", "IsPrimary", "IsVerified" },
                values: new object[] { 259446041324163147L, 259446041324163146L, true, true });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId", "IsPrimary" },
                values: new object[] { 259446041324163149L, 259446041324163148L, true });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId" },
                values: new object[] { 259446041324163150L, 259446041324163148L });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId", "IsVerified" },
                values: new object[] { 259446041324163151L, 259446041324163148L, true });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId", "IsPrimary" },
                values: new object[] { 259446041324163153L, 259446041324163152L, true });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId" },
                values: new object[,]
                {
                    { 259446041324163154L, 259446041324163152L },
                    { 259446041324163155L, 259446041324163152L }
                });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId", "IsPrimary" },
                values: new object[,]
                {
                    { 259446041324163157L, 259446041324163156L, true },
                    { 259446041324163159L, 259446041324163158L, true }
                });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId" },
                values: new object[,]
                {
                    { 259446041324163160L, 259446041324163158L },
                    { 259446041324163161L, 259446041324163158L }
                });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId", "IsPrimary", "IsVerified" },
                values: new object[] { 259446041324163163L, 259446041324163162L, true, true });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId", "IsPrimary" },
                values: new object[] { 259446041324163165L, 259446041324163164L, true });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId" },
                values: new object[,]
                {
                    { 259446041324163166L, 259446041324163164L },
                    { 259446041324163167L, 259446041324163164L }
                });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId", "IsPrimary" },
                values: new object[] { 259446041324163169L, 259446041324163168L, true });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId", "IsVerified" },
                values: new object[] { 259446041324163170L, 259446041324163168L, true });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId", "IsPrimary", "IsVerified" },
                values: new object[,]
                {
                    { 259446041324163172L, 259446041324163171L, true, true },
                    { 259446041324163174L, 259446041324163173L, true, true }
                });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId", "IsPrimary" },
                values: new object[] { 259446041324163176L, 259446041324163175L, true });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId" },
                values: new object[,]
                {
                    { 259446041324163177L, 259446041324163175L },
                    { 259446041324163178L, 259446041324163175L }
                });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId", "IsPrimary", "IsVerified" },
                values: new object[] { 259446041324163180L, 259446041324163179L, true, true });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId", "IsVerified" },
                values: new object[] { 259446041324163181L, 259446041324163179L, true });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId" },
                values: new object[] { 259446041324163182L, 259446041324163179L });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId", "IsPrimary", "IsVerified" },
                values: new object[] { 259446041324163184L, 259446041324163183L, true, true });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId", "IsVerified" },
                values: new object[] { 259446041324163185L, 259446041324163183L, true });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId" },
                values: new object[] { 259446041324163186L, 259446041324163183L });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId", "IsPrimary" },
                values: new object[] { 259446041324163188L, 259446041324163187L, true });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId", "IsVerified" },
                values: new object[] { 259446041324163189L, 259446041324163187L, true });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId", "IsPrimary" },
                values: new object[] { 259446041324163191L, 259446041324163190L, true });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId", "IsPrimary", "IsVerified" },
                values: new object[] { 259446041324163193L, 259446041324163192L, true, true });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId", "IsVerified" },
                values: new object[] { 259446041324163194L, 259446041324163192L, true });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId", "IsPrimary", "IsVerified" },
                values: new object[,]
                {
                    { 259446041324163196L, 259446041324163195L, true, true },
                    { 259446041324163198L, 259446041324163197L, true, true }
                });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId" },
                values: new object[,]
                {
                    { 259446041324163199L, 259446041324163197L },
                    { 259446041324163200L, 259446041324163197L }
                });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId", "IsPrimary", "IsVerified" },
                values: new object[,]
                {
                    { 259446041324163202L, 259446041324163201L, true, true },
                    { 259446041324163204L, 259446041324163203L, true, true }
                });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId" },
                values: new object[,]
                {
                    { 259446041324163205L, 259446041324163203L },
                    { 259446041324163206L, 259446041324163203L }
                });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId", "IsPrimary" },
                values: new object[] { 259446041324163208L, 259446041324163207L, true });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId", "IsVerified" },
                values: new object[] { 259446041324163209L, 259446041324163207L, true });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId", "IsPrimary", "IsVerified" },
                values: new object[] { 259446041324163211L, 259446041324163210L, true, true });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId" },
                values: new object[] { 259446041324163212L, 259446041324163210L });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "ContactInformation",
                columns: new[] { "Id", "CustomerId", "IsVerified" },
                values: new object[] { 259446041324163213L, 259446041324163210L, true });

            migrationBuilder.CreateIndex(
                name: "IX_ContactInformation_CustomerId",
                schema: "customers",
                table: "ContactInformation",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactInformation_Email",
                schema: "customers",
                table: "ContactInformation",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContactInformation_IsPrimary",
                schema: "customers",
                table: "ContactInformation",
                column: "IsPrimary");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_IsDeleted",
                schema: "customers",
                table: "Customers",
                column: "IsDeleted");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContactInformation",
                schema: "customers");

            migrationBuilder.DropTable(
                name: "Customers",
                schema: "customers");
        }
    }
}
