
using Microsoft.EntityFrameworkCore;

using API.Data;
using API.Data.Managers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

// Add authentication


// Add JWTHandler rego


// Configure Swagger to have auth option
builder.Services.AddSwaggerGen();

// Add DBContext
builder.Services.AddDbContext<DatabaseContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("WeightliftingDB"));
});

// Configure Identity


// Rego Managers
builder.Services.AddScoped<IAccountManager, AccountManager>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
