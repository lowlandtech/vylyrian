var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder
    .Services
    .AddDbContext<GraphContext>(options =>
    {
        options.UseInMemoryDatabase("VylyrGraph");
    });

// Add device-specific services used by the LowlandTech.Vylyr.Core project
builder.Services.AddSingleton<IFormFactor, FormFactor>();
builder.Services.AddSingleton<AppVm>();
builder.Services.AddMudServices();

using var scope = builder.Services.BuildServiceProvider().CreateScope();
var db = scope.ServiceProvider.GetRequiredService<GraphContext>();
await db.Database.EnsureCreatedAsync();
await db.UseCaseData();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(typeof(LowlandTech.Vylyr.Core._Imports).Assembly);

app.Run();
