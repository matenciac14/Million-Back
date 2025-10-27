using ASP.MongoDb.API.ProductRepository;
using ASP.MongoDb.API.Repository;
using ASP.MongoDb.API.Services;
using DotNetEnv;

// Load environment variables from .env file
Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Configure MongoDB Settings from environment variables
builder.Services.Configure<ASP.MongoDb.API.Settings.MongoDbSettings>(options =>
{
    options.ConnectionString = Environment.GetEnvironmentVariable("MONGODB_CONNECTION_STRING") ?? "";
    options.DatabaseName = Environment.GetEnvironmentVariable("MONGODB_DATABASE_NAME") ?? "";
});

// Configure Cloudinary Settings from environment variables
builder.Services.Configure<ASP.MongoDb.API.Settings.CloudinarySettings>(options =>
{
    options.CloudName = Environment.GetEnvironmentVariable("CLOUDINARY_CLOUD_NAME") ?? "";
    options.ApiKey = Environment.GetEnvironmentVariable("CLOUDINARY_API_KEY") ?? "";
    options.ApiSecret = Environment.GetEnvironmentVariable("CLOUDINARY_API_SECRET") ?? "";
    options.BaseUrl = Environment.GetEnvironmentVariable("CLOUDINARY_BASE_URL") ?? "https://res.cloudinary.com";
    options.SecureUrl = Environment.GetEnvironmentVariable("CLOUDINARY_SECURE_URL") ?? "https://res.cloudinary.com";
});

// Add CORS for frontend integration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://localhost:3001", "https://localhost:3000",
                          "http://localhost:5179", "https://localhost:7007")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });

    // Add a more permissive policy for development/Swagger
    options.AddPolicy("Development", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Add services to the container.
// Add the Repository Dependency Injection
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Add Product Repository Dependency Injection (Legacy)
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// Add Real Estate Repository Dependency Injection
builder.Services.AddScoped<IPropertyRepository, PropertyRepository>();
builder.Services.AddScoped<IOwnerRepository, OwnerRepository>();

// Add Image Service Dependency Injection
builder.Services.AddScoped<IImageService, CloudinaryImageService>();

// Add Controllers
builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "Real Estate API",
        Version = "v1",
        Description = "API for managing real estate properties and owners"
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Real Estate API v1");
        c.RoutePrefix = string.Empty; // Set Swagger UI at app root
    });
}

// Use CORS - Use permissive policy in development
if (app.Environment.IsDevelopment())
{
    app.UseCors("Development");
}
else
{
    app.UseCors("AllowReactApp");
}

// Configure HTTPS redirection only in production or when HTTPS is available
if (app.Environment.IsProduction() || app.Configuration["ASPNETCORE_URLS"]?.Contains("https") == true)
{
    app.UseHttpsRedirection();
}

app.MapControllers();

app.Run();
