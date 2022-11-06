using Acme.Energy.Backend;

var builder = WebApplication.CreateBuilder(args);


// CORS
var authorizedCorsOrigins = builder.Configuration.GetSection("Cors:AuthorizedOrigins").Get<List<string>>();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "CorsPolicy", builder =>
    {
        builder.WithOrigins(authorizedCorsOrigins.ToArray())
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials()
               .SetPreflightMaxAge(new TimeSpan(8, 0, 0)) // Cache for 8H
               .Build();
    });
});

// Add services to the container.
builder.Services.AddControllers();

// NSWAG for OpenAPI 3.0
builder.Services.AddOpenApiDocument(doc =>
{
    doc.Title = "Acme.Energy";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // NSWAG for OpenAPI 3.0
    app.UseOpenApi(configure =>
    {
        configure.Path = $"/openapi/v1/openapi.json";
    });
    app.UseSwaggerUi3(configure =>
    {
        configure.DocumentPath = "/openapi/{documentName}/openapi.json";
    });
}

// Register in Consul (if configured)
ConsulRegistrator.RegisterInConsul(app.Configuration, app, app.Lifetime);

app.UseCors("CorsPolicy");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
