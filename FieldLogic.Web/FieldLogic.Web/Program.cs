using FieldLogic.Web.Components;
using FieldLogic.Web.Services;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using FieldLogic.Shared;

var builder = WebApplication.CreateBuilder(args);

// 1. Database Configuration - Use Npgsql for your Postgres DB
// builder.Services.AddDbContextFactory<AppDbContext>(options =>
//     options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
                       ?? throw new InvalidOperationException("CRITICAL FAULT: Connection string 'DefaultConnection' not found in appsettings.json.");

builder.Services.AddDbContextFactory<AppDbContext>(options =>
    options.UseNpgsql(connectionString));


// 2. Register Services
builder.Services.AddHttpClient<JikanService>();
builder.Services.AddMudServices();

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

// THE KEY FIX: Use the full namespace for App to avoid the 'FieldLogic.Web' conflict
app.MapRazorComponents<FieldLogic.Web.Components.App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(FieldLogic.Web.Client._Imports).Assembly);

app.Run();