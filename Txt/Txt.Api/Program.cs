using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.OpenApi.Models;
using Txt.Api.Helpers;
using Txt.Application.Helpers;
using Txt.Domain.Entities;
using Txt.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Txt API", Version = "v1" });

    options.AddSecurityDefinition("Identity.BearerAndApplication", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please, enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme()
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Identity.BearerAndApplication",
                },
            },
            Array.Empty<string>()
        },
    });
});

builder.Services.AddDbContext<ApplicationDbContext>();

builder.Services
    .AddIdentityApiEndpoints<User>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 8;
        options.Password.RequiredUniqueChars = 4;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = true;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddLocalServices();
builder.Services.AddRepositories();
builder.Services.AddApplicationDependencies();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy => policy
            // .WithOrigins(
            //     "http://localhost:5000",
            //     "https://localhost:5000",
            //     "http://localhost:5002",
            //     "https://localhost:5002",
            //     "http://localhost:5004",
            //     "https://localhost:5004")
            .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler(err => err.UseErrors(app.Environment));
}

app.UseCors("CorsPolicy");
app.MapGroup("authorization").MapIdentityApi<User>();
app.UseAuthorization();

app.MapControllers();

app.Run();