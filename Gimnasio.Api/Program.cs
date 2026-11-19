using System.Text;
using Gimnasio.Api.Filtros;
using Gimnasio.Datos;
using Gimnasio.Negocio;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddExceptionHandler<ReglaNegocioHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, _, _) =>
    {
        document.Info.Title = "Gimnasio Aura API";
        document.Info.Version = "v1";
        document.Info.Description =
            "Planes y miembros del gimnasio, con autenticación JWT. " +
            "Recepcionista administra. Entrenador solo consulta.";

        var esquema = new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            Description = "Pega el token que devuelve POST /api/auth/login. Sin la palabra Bearer."
        };

        document.Components ??= new OpenApiComponents();
        document.AddComponent("Bearer", esquema);

        document.Security ??= [];
        document.Security.Add(new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference("Bearer", document)] = []
        });

        return Task.CompletedTask;
    });
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        "Falta ConnectionStrings:DefaultConnection. Copia appsettings.Development.example.json a appsettings.Development.json y completa la cadena. No la subas al repositorio.");
}

var claveJwt = builder.Configuration["Jwt:Key"];
if (string.IsNullOrWhiteSpace(claveJwt) || claveJwt.Length < 32)
{
    throw new InvalidOperationException(
        "Jwt:Key debe tener al menos 32 caracteres. Ponla en appsettings.Development.json o en la variable Jwt__Key. No va en el repositorio.");
}

var expiracion = 60;
if (int.TryParse(builder.Configuration["Jwt:ExpiracionMinutos"], out var minutos) && minutos > 0)
{
    expiracion = minutos;
}

builder.Services.AgregarDatos(connectionString);
builder.Services.AgregarNegocio(new JwtOpciones
{
    Key = claveJwt,
    Issuer = builder.Configuration["Jwt:Issuer"] ?? "GimnasioAura",
    Audience = builder.Configuration["Jwt:Audience"] ?? "GimnasioAura.Clientes",
    ExpiracionMinutos = expiracion
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.MapInboundClaims = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(claveJwt)),
            ClockSkew = TimeSpan.FromSeconds(30),
            RoleClaimType = "role",
            NameClaimType = "sub"
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseExceptionHandler();
app.MapOpenApi();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/openapi/v1.json", "Gimnasio Aura API");
    options.RoutePrefix = "swagger";
});

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapGet("/", () => Results.Redirect("/swagger"));

app.Run();
