using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MediKef.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "devices",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    device_id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    device_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    manufacturer = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    model = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    serial_number = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    device_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    protocol = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    connection_type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    ip_address = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    port = table.Column<int>(type: "integer", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_devices", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "patients",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    patient_id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    tc_no = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: true),
                    first_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    last_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    birth_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    gender = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    address = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_patients", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tests",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    test_code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    test_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    test_category = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    unit = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    reference_range_min = table.Column<decimal>(type: "numeric", nullable: true),
                    reference_range_max = table.Column<decimal>(type: "numeric", nullable: true),
                    reference_range_text = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    sample_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    price = table.Column<decimal>(type: "numeric", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tests", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "lisbox_logs",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    device_id = table.Column<int>(type: "integer", nullable: true),
                    sample_barcode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    raw_data = table.Column<string>(type: "text", nullable: true),
                    parsed_data = table.Column<string>(type: "jsonb", nullable: true),
                    status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    error_message = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lisbox_logs", x => x.id);
                    table.ForeignKey(
                        name: "FK_lisbox_logs_devices_device_id",
                        column: x => x.device_id,
                        principalTable: "devices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "samples",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sample_id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    barcode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    patient_id = table.Column<int>(type: "integer", nullable: false),
                    sample_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    collection_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    received_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    priority = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    notes = table.Column<string>(type: "text", nullable: true),
                    created_by = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_samples", x => x.id);
                    table.ForeignKey(
                        name: "FK_samples_patients_patient_id",
                        column: x => x.patient_id,
                        principalTable: "patients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sample_tests",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sample_id = table.Column<int>(type: "integer", nullable: false),
                    test_id = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sample_tests", x => x.id);
                    table.ForeignKey(
                        name: "FK_sample_tests_samples_sample_id",
                        column: x => x.sample_id,
                        principalTable: "samples",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_sample_tests_tests_test_id",
                        column: x => x.test_id,
                        principalTable: "tests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "test_results",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sample_test_id = table.Column<int>(type: "integer", nullable: false),
                    device_id = table.Column<int>(type: "integer", nullable: true),
                    result_value = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    result_numeric = table.Column<decimal>(type: "numeric", nullable: true),
                    unit = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    reference_range = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    flag = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    result_status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    result_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    validated_by = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    validated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    notes = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_test_results", x => x.id);
                    table.ForeignKey(
                        name: "FK_test_results_devices_device_id",
                        column: x => x.device_id,
                        principalTable: "devices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_test_results_sample_tests_sample_test_id",
                        column: x => x.sample_test_id,
                        principalTable: "sample_tests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_devices_device_id",
                table: "devices",
                column: "device_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_lisbox_logs_device_id",
                table: "lisbox_logs",
                column: "device_id");

            migrationBuilder.CreateIndex(
                name: "IX_patients_patient_id",
                table: "patients",
                column: "patient_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_patients_tc_no",
                table: "patients",
                column: "tc_no",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_sample_tests_sample_id_test_id",
                table: "sample_tests",
                columns: new[] { "sample_id", "test_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_sample_tests_test_id",
                table: "sample_tests",
                column: "test_id");

            migrationBuilder.CreateIndex(
                name: "IX_samples_barcode",
                table: "samples",
                column: "barcode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_samples_patient_id",
                table: "samples",
                column: "patient_id");

            migrationBuilder.CreateIndex(
                name: "IX_samples_sample_id",
                table: "samples",
                column: "sample_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_test_results_device_id",
                table: "test_results",
                column: "device_id");

            migrationBuilder.CreateIndex(
                name: "IX_test_results_sample_test_id",
                table: "test_results",
                column: "sample_test_id");

            migrationBuilder.CreateIndex(
                name: "IX_tests_test_code",
                table: "tests",
                column: "test_code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "lisbox_logs");

            migrationBuilder.DropTable(
                name: "test_results");

            migrationBuilder.DropTable(
                name: "devices");

            migrationBuilder.DropTable(
                name: "sample_tests");

            migrationBuilder.DropTable(
                name: "samples");

            migrationBuilder.DropTable(
                name: "tests");

            migrationBuilder.DropTable(
                name: "patients");
        }
    }
}
