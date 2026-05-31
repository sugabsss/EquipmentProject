using Asp.Versioning;
using EquipmentManagement.Application.Commands;
using EquipmentManagement.Application.Mappers;
using EquipmentManagement.Application.Queries;
using EquipmentManagement.Domain.Interfaces;
using EquipmentManagement.Infra.Context;
using EquipmentManagement.Infra.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true; // retorna no header quais versões existem
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),        // /api/v1/equipamentos
        new HeaderApiVersionReader("X-Version"), // header: X-Version: 1.0
        new QueryStringApiVersionReader("ver")   // ?ver=1.0
    );
})
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// Auto Mapper properties :
builder.Services.AddAutoMapper(typeof(EquipmentProfile).Assembly);

// React Cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:5174")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddMediatR(
    typeof(GetEquipmentQuery).Assembly,      // Queries
    typeof(InsertEquipmentCommand).Assembly  // Commands
);

builder.Services.AddScoped<IEquipmentRepository, EquipmentRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowReact");

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();
app.MapControllers();

app.Run();
