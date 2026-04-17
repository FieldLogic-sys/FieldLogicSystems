using FieldLogic.Web.Components;
using FieldLogic.Web.Services;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using FieldLogic.Shared;

var builder = WebApplication.CreateBuilder(args);

// 1. Database Configuration
// This pulls from User Secrets locally and Environment Variables in Production
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
                       ?? throw new InvalidOperationException("CRITICAL FAULT: Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContextFactory<AppDbContext>(options =>
    options.UseNpgsql(connectionString, x =>
        x.MigrationsAssembly("FieldLogic.Web")));

// 2. Register Services
builder.Services.AddHttpClient<JikanService>();
builder.Services.AddMudServices();
builder.Services.AddScoped<IntelligenceService>();

// 3. Blazor Setup
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

var app = builder.Build();

// Configuration Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}

app.UseHttpsRedirection();
app.UseAntiforgery();
app.MapStaticAssets();

// Map the Root Component
app.MapRazorComponents<FieldLogic.Web.Components.App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(FieldLogic.Web.Client._Imports).Assembly);

app.Run();