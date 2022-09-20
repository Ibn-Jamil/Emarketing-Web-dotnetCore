using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AngularCoreEmarketing.Data.Migrations
{
    public partial class AllTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CatagoriesMajor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MajorName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatagoriesMajor", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryAddress",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StreetAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ZipCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SendGift = table.Column<bool>(type: "bit", nullable: false),
                    TimeForForienKeyHelp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AdditionalRequests = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryAddress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeliveryAddress_AspNetUsers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductsFeedback",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Questions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Answar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuestionTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AnswarTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProductDetailsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsFeedback", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CatagoriesMajorSub",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MajorSubName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CatagoryMajorId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatagoriesMajorSub", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CatagoriesMajorSub_CatagoriesMajor_CatagoryMajorId",
                        column: x => x.CatagoryMajorId,
                        principalTable: "CatagoriesMajor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CatagoriesSpecific",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CatagoriesName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CatagoriesMajor = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatagoriesSpecific", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CatagoriesSpecific_CatagoriesMajorSub_CatagoriesMajor",
                        column: x => x.CatagoriesMajor,
                        principalTable: "CatagoriesMajorSub",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductsDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeliveryCharge = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<float>(type: "real", nullable: false),
                    ProductPrice = table.Column<int>(type: "int", nullable: false),
                    ProductImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductPostDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProductSellerId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ProductStock = table.Column<int>(type: "int", nullable: false),
                    ProductDiscount = table.Column<int>(type: "int", nullable: false),
                    CatagoriesSpecificId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductsDetails_AspNetUsers_ProductSellerId",
                        column: x => x.ProductSellerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductsDetails_CatagoriesSpecific_CatagoriesSpecificId",
                        column: x => x.CatagoriesSpecificId,
                        principalTable: "CatagoriesSpecific",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Size = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    CartChecked = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Carts_AspNetUsers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Carts_ProductsDetails_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ProductsDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomerReviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ratingStars = table.Column<int>(type: "int", nullable: false),
                    ReviewsDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReviewResponse = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerReviewd = table.Column<bool>(type: "bit", nullable: false),
                    SellerAnswered = table.Column<bool>(type: "bit", nullable: false),
                    ReviewTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResponseTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProductDetailsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerReviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerReviews_ProductsDetails_ProductDetailsId",
                        column: x => x.ProductDetailsId,
                        principalTable: "ProductsDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ImagesDirectories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductDetailsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImagesDirectories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImagesDirectories_ProductsDetails_ProductDetailsId",
                        column: x => x.ProductDetailsId,
                        principalTable: "ProductsDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderdProducts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Price = table.Column<int>(type: "int", nullable: false),
                    DeliveryCharges = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Size = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Disprition = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    CustomerId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AddressId = table.Column<int>(type: "int", nullable: false),
                    OrderStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderdProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderdProducts_AspNetUsers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderdProducts_DeliveryAddress_AddressId",
                        column: x => x.AddressId,
                        principalTable: "DeliveryAddress",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderdProducts_ProductsDetails_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ProductsDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductsSizes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AllSizes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    ProductDetailsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsSizes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductsSizes_ProductsDetails_ProductDetailsId",
                        column: x => x.ProductDetailsId,
                        principalTable: "ProductsDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Carts_CustomerId",
                table: "Carts",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_ProductId",
                table: "Carts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_CatagoriesMajorSub_CatagoryMajorId",
                table: "CatagoriesMajorSub",
                column: "CatagoryMajorId");

            migrationBuilder.CreateIndex(
                name: "IX_CatagoriesSpecific_CatagoriesMajor",
                table: "CatagoriesSpecific",
                column: "CatagoriesMajor");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerReviews_ProductDetailsId",
                table: "CustomerReviews",
                column: "ProductDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryAddress_CustomerId",
                table: "DeliveryAddress",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_ImagesDirectories_ProductDetailsId",
                table: "ImagesDirectories",
                column: "ProductDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderdProducts_AddressId",
                table: "OrderdProducts",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderdProducts_CustomerId",
                table: "OrderdProducts",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderdProducts_ProductId",
                table: "OrderdProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsDetails_CatagoriesSpecificId",
                table: "ProductsDetails",
                column: "CatagoriesSpecificId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsDetails_ProductSellerId",
                table: "ProductsDetails",
                column: "ProductSellerId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsSizes_ProductDetailsId",
                table: "ProductsSizes",
                column: "ProductDetailsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropTable(
                name: "CustomerReviews");

            migrationBuilder.DropTable(
                name: "ImagesDirectories");

            migrationBuilder.DropTable(
                name: "OrderdProducts");

            migrationBuilder.DropTable(
                name: "ProductsFeedback");

            migrationBuilder.DropTable(
                name: "ProductsSizes");

            migrationBuilder.DropTable(
                name: "DeliveryAddress");

            migrationBuilder.DropTable(
                name: "ProductsDetails");

            migrationBuilder.DropTable(
                name: "CatagoriesSpecific");

            migrationBuilder.DropTable(
                name: "CatagoriesMajorSub");

            migrationBuilder.DropTable(
                name: "CatagoriesMajor");
        }
    }
}
