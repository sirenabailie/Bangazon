using Bangazon.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Allows passing DateTime values without time zone data
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Configures the database connection using PostgreSQL and Entity Framework Core
builder.Services.AddNpgsql<BangazonDbContext>(builder.Configuration["BangazonDbConnectionString"]);

// Set the JSON serializer options
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//Product Endpoints//

// Get all products
app.MapGet("/api/products", (BangazonDbContext db) =>
{
    return db.Products.Where(p => p.IsAvailable).ToList();
});

// Get product details
app.MapGet("/api/products/{id}", (BangazonDbContext db, int id) =>
{
    var product = db.Products.SingleOrDefault(p => p.Id == id);
    return product is null ? Results.NotFound() : Results.Ok(product);
});

// Search for products by keyword
app.MapGet("/api/products/search", (BangazonDbContext db, string q) =>
{
    var results = db.Products
        .Where(p => p.Name.Contains(q) || p.Description.Contains(q))
        .ToList();
    return Results.Ok(results);
});

//Cart Endpoints//

// Get cart for logged-in user
app.MapGet("/api/cart", (BangazonDbContext db, HttpContext httpContext) =>
{
    var userUid = httpContext.User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
    if (userUid is null) return Results.Unauthorized();

    var customer = db.Customers.SingleOrDefault(c => c.UserId == userUid);
    if (customer is null) return Results.NotFound();

    var cartItems = db.Carts
        .Where(c => c.CustomerId == userUid)
        .Include(c => c.Product) // ✅ Ensures we load Product details
        .Select(c => new 
        {
            ProductName = c.Product != null ? c.Product.Name : "Unknown",
            ProductPrice = c.Product != null ? c.Product.Price : 0
        })
        .ToList();

    return Results.Ok(cartItems);
});

// Add a product to cart
app.MapPost("/api/cart/add", (BangazonDbContext db, Cart cartItem) =>
{
    db.Carts.Add(cartItem);
    db.SaveChanges();
    return Results.Created($"/api/cart/{cartItem.Id}", cartItem);
});

// Remove product from cart
app.MapDelete("/api/cart/remove/{productId}", (BangazonDbContext db, int productId) =>
{
    var cartItem = db.Carts.SingleOrDefault(c => c.ProductId == productId);
    if (cartItem is null) return Results.NotFound();

    db.Carts.Remove(cartItem);
    db.SaveChanges();
    return Results.NoContent();
});

//Order Endpoints//

// Get order history for user
app.MapGet("/api/orders", (BangazonDbContext db, HttpContext httpContext) =>
{
    var userUid = httpContext.User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
    if (userUid is null) return Results.Unauthorized();

    var orders = db.Orders
        .Where(o => o.CustomerId == userUid)
        .Include(o => o.Cart)  // ✅ Ensures Cart is included
        .ThenInclude(c => c.Product)  // ✅ Ensures Product details are loaded
        .Select(o => new 
        {
            OrderId = o.Id,
            IsComplete = o.IsComplete,
            CartItems = o.Cart != null && o.Cart.Product != null
                ? new 
                { 
                    ProductName = o.Cart.Product.Name,
                    ProductPrice = o.Cart.Product.Price
                }
                : null
        })
        .ToList();

    return Results.Ok(orders);
});

// Place an order
app.MapPost("/api/orders", (BangazonDbContext db, Order newOrder) =>
{
    db.Orders.Add(newOrder);
    db.SaveChanges();
    return Results.Created($"/api/orders/{newOrder.Id}", newOrder);
});

// Assign payment type to order
app.MapPut("/api/orders/{orderId}/payment", (BangazonDbContext db, int orderId, int paymentTypeId) =>
{
    var order = db.Orders.SingleOrDefault(o => o.Id == orderId);
    if (order is null) return Results.NotFound();

    order.IsComplete = true;
    db.SaveChanges();
    return Results.Ok(order);
});

//Seller Endpoints//

// Get seller dashboard data
app.MapGet("/api/seller/dashboard", (BangazonDbContext db, HttpContext httpContext) =>
{
    var userUid = httpContext.User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
    if (userUid is null) return Results.Unauthorized();

    var seller = db.Sellers
        .Include(s => s.Store) // ✅ Ensures Store is loaded
        .SingleOrDefault(s => s.SellerId == userUid);
    
    if (seller is null) return Results.NotFound();

    var totalSales = db.Orders
        .Where(o => o.Cart != null && o.Cart.Product.StoreId == seller.Store.Id)
        .Count();

    var pendingOrders = db.Orders
        .Where(o => !o.IsComplete && o.Cart != null && o.Cart.Product.StoreId == seller.Store.Id)
        .Count();

    var inventoryCounts = db.Products
        .Where(p => p.StoreId == seller.Store.Id)
        .GroupBy(p => p.CategoryId)
        .Select(g => new { Category = db.Categories.FirstOrDefault(c => c.Id == g.Key).Title, Count = g.Count() })
        .ToList();

    return Results.Ok(new
    {
        TotalSales = totalSales,
        PendingOrders = pendingOrders,
        InventoryCounts = inventoryCounts
    });
});

// Get all products for a seller
app.MapGet("/api/sellers/{sellerId}/products", (BangazonDbContext db, int sellerId) =>
{
    var products = db.Products.Where(p => p.StoreId == sellerId).ToList();
    return Results.Ok(products);
});

//Category Endpoints//

// Get all categories with product counts
app.MapGet("/api/categories", (BangazonDbContext db) =>
{
    var categories = db.Categories
        .Select(c => new
        {
            c.Title,
            ProductCount = db.Products.Count(p => p.CategoryId == c.Id),
            Products = db.Products.Where(p => p.CategoryId == c.Id).Take(3).Select(p => p.Name).ToList()
        })
        .ToList();

    return Results.Ok(categories);
});

/// **🛍️ Customer Profile Endpoints** ///

// View customer profile
app.MapGet("/api/customers/profile", (BangazonDbContext db, HttpContext httpContext) =>
{
    var userUid = httpContext.User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
    if (userUid is null) return Results.Unauthorized();

    var customer = db.Users.SingleOrDefault(u => u.Uid == userUid);
    if (customer is null) return Results.NotFound();

    return Results.Ok(customer);
});

// Allow customer to edit profile
app.MapPut("/api/customers/profile", (BangazonDbContext db, HttpContext httpContext, User updatedUser) =>
{
    var userUid = httpContext.User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
    if (userUid is null) return Results.Unauthorized();

    var customer = db.Users.SingleOrDefault(u => u.Uid == userUid);
    if (customer is null) return Results.NotFound();

    customer.FirstName = updatedUser.FirstName;
    customer.LastName = updatedUser.LastName;
    customer.Email = updatedUser.Email;
    customer.Address = updatedUser.Address;
    customer.City = updatedUser.City;
    customer.Zip = updatedUser.Zip;

    db.SaveChanges();
    return Results.NoContent();
});

app.Run();
