using Azure.Security.KeyVault.Secrets;
using ExamenApiAzureCubosMauricio.Data;
using ExamenApiAzureCubosMauricio.Helpers;
using ExamenApiAzureCubosMauricio.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using NSwag;
using NSwag.Generation.Processors.Security;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddTransient<RepositoryCubos>();
builder.Services.AddAzureClients(factory =>
{
    factory.AddSecretClient(builder.Configuration.GetSection("KeyVault"));
});
SecretClient secretClient = builder.Services.BuildServiceProvider().GetService<SecretClient>();
HelperActionServicesOAuth helper = new HelperActionServicesOAuth(secretClient);
KeyVaultSecret secret = await secretClient.GetSecretAsync("SqlAzure");
string connectionString = secret.Value;
builder.Services.AddDbContext<CubosContext>(options =>
{
    options.UseSqlServer(connectionString);
});
builder.Services.AddSingleton<HelperActionServicesOAuth>(helper);
builder.Services.AddAuthentication(helper.GetAuthenticateSchema()).AddJwtBearer(helper.GetJwtBearerOptions());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(document =>
{
    document.Title = "ApiOAuth2 Examen Mauricio";
    document.Description = "Api Examen Cubos.  Proyecto Cubos";
    document.Version = "v1";
    document.AddSecurity("JWT", Enumerable.Empty<string>(),
        new OpenApiSecurityScheme
        {
            Type = OpenApiSecuritySchemeType.ApiKey,
            Name = "Authorization",
            In = OpenApiSecurityApiKeyLocation.Header,
            Description = "Copia y pega el Token en el campo 'Value:' así: Bearer {Token JWT}."
        }
    );
    document.OperationProcessors.Add(
    new AspNetCoreOperationSecurityScopeProcessor("JWT"));
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}
app.UseOpenApi();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint(url: "/swagger/v1/swagger.json", name: "Api OAuth2 Examen Mauricio");
    options.RoutePrefix = "";
});
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
