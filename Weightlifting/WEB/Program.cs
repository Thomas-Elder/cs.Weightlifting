using WEB.Services;

IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddScoped<IAccountService, AccountService>();

builder.Services.AddHttpClient<IAccountService, AccountService>(client =>
{
    // Should be able to access app.settings here with something like:
    // config.GetRequiredSection("API_URL").Value
    client.BaseAddress = new Uri("https://localhost:7207/");
});

builder.Services.AddHttpClient<IHomeService, HomeService>(client =>
{
    // Should be able to access app.settings here with something like:
    // config.GetRequiredSection("API_URL").Value
    client.BaseAddress = new Uri("https://localhost:7207/");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
