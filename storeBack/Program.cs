using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using storeBack;
using storeBack.Services.Articulos;
using storeBack.Services.ArticulosCliente;
using storeBack.Services.ArticulosTienda;
using storeBack.Services.Auth;
using storeBack.Services.Cliente;
using storeBack.Services.ShoppingCart;
using storeBack.Services.Tienda;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
string secretKey = builder.Configuration.GetSection("JwtSettings:SecretKey").Value!;
// Add services to the container.


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DevConnection")));
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
      builder => builder.WithOrigins("http://localhost:4200")
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateLifetime = true
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Researchers", Version = "v1" });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

builder.Services.AddScoped<IClienteService, storeBack.Services.Cliente.ClienteService>(); ;
builder.Services.AddScoped<ITiendaService, storeBack.Services.Tienda.TiendaService>();
builder.Services.AddScoped<IArticuloService, storeBack.Services.Articulos.ArticuloService>();
builder.Services.AddScoped<IArticuloClienteService, storeBack.Services.ArticulosCliente.ArticuloClienteService>();
builder.Services.AddScoped<IArticuloTiendaService, storeBack.Services.ArticulosTienda.ArticuloTiendaService>();
builder.Services.AddScoped<IShoppingCartService, storeBack.Services.ShoppingCart.ShoppingCartService>();
builder.Services.AddScoped<IAuthService, storeBack.Services.Auth.AuthService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseCors("AllowAngularApp");

app.Run();
