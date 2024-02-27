// public static void Main(string[] args)

using IssuesApi;
using IssuesApi.Features.Catalog;
using IssuesApi.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container. "Container is an Inversion of Control (IOC) Container"

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // Use reflection at runtime to document your API with an openapi3.0 spec.

var connectionString = builder.Configuration.GetConnectionString("issues") ?? throw new Exception("Need a connnection string");

builder.Services.AddDbContext<IssuesDataContext>(options =>
{
    options.UseNpgsql(connectionString);
});

// Don't inject anything into a Singleton service other than Singleton services
// This would inject Scoped service into a Singleton, which would be bad.
//builder.Services.AddSingleton<SoftwareCatalogManager>(); // BAD.


builder.Services.AddCatalogFeature();


// Above this line is "setup" configuration. 
var app = builder.Build();
// Everything after this line is actually mapping requests coming in to code.

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // that "AddSwaggerGen" thing above? This makes it available through a URL.
    app.UseSwaggerUI(); // Makes a web page available at /swagger/index.html that reads the above open api spec and shows a UI.
}

app.UseAuthorization();

app.MapControllers();
if (app.Environment.IsDevelopment())
{
    await SeedIssuesAsync();
}

app.Run(); // Blocking Call. This is the webserver (kestrel) running and listing as long as the application runs..

async Task SeedIssuesAsync()
{
    // You need to create a scope to get a scoped service, if you are doing this outside of an
    // an "injection context" - like not having it on the constructor a runtime thing like
    // a controller or a service.
    using (var scope = app.Services.CreateScope())
    {
        try
        {
            var scopedContext = scope.ServiceProvider.GetRequiredService<IssuesDataContext>();
            await DevelopmentSeedData.InitializeSoftwareCatalogAsync(scopedContext);
        }
        catch
        {

            throw;
        }
    }// The scope is "Disposed"
}