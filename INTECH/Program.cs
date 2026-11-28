using INTECH.Context;
using INTECH.Services;
using INTECH.Components;
using INTECH.Components.Account;
using INTECH.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

var ConStr = builder.Configuration.GetConnectionString("SqlConStr") ?? throw new InvalidOperationException("Connection string 'SqlConStr' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(ConStr));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDbContextFactory<Contexto>(options =>
    options.UseSqlServer(ConStr));

builder.Services.AddScoped<EstadosSolicitudesService>();
builder.Services.AddScoped<SolicitudesService>();
builder.Services.AddScoped<ClientesService>();
builder.Services.AddScoped<LevantamientosService>();
builder.Services.AddScoped<AgendasService>();
builder.Services.AddScoped<OrdenesTrabajoService>();
builder.Services.AddScoped<HistorialesService>();
builder.Services.AddScoped<CategoriasServiciosService>();
builder.Services.AddScoped<ServiciosService>();

builder.Services.AddBlazorBootstrap();

builder.Services.AddIdentityCore<ApplicationUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        options.Stores.SchemaVersion = IdentitySchemaVersions.Version3;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets().AllowAnonymous();
app.MapGet("/", (HttpContext http) =>
{
    if (http.User.IsInRole("Técnico"))
    {
        return Results.Redirect("/Tecnico/Inicio");
    }
    if (http.User.IsInRole("Gerente"))
    {
        return Results.Redirect("/Gerente/Inicio");
    }
    if (http.User.Identity?.IsAuthenticated == true)
    {
        return Results.Redirect("/Account/Manage");
    }

    return Results.Redirect("/Account/Login");
});
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();
