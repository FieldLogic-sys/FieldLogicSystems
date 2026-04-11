using FieldLogic.Web.Client.Pages;
using FieldLogic.Web.Components;
using FieldLogic.Web.Models;
using FieldLogic.Web.Services; // Ensure this is present
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;



var builder = WebApplication.CreateBuilder(args);

// 1. Database Configuration
builder.Services.AddDbContextFactory<AppDbContext>(options =>
    options.UseSqlite("Data Source=FieldLogic.db"));

// 2. Register the Jikan Engine
builder.Services.AddHttpClient<JikanService>();

// 3. Blazor Services
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddMudServices();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(FieldLogic.Web.Client._Imports).Assembly);

// 4. THE STARTUP TEST
using (var scope = app.Services.CreateScope())
{
    var jikan = scope.ServiceProvider.GetRequiredService<JikanService>();
    var results = await jikan.SearchAnimeAsync("Iruma-kun");
    Console.WriteLine($"[SYSTEM CHECK] Found {results.Count} anime results.");
}

app.Run();