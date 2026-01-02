using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BirikimPro.Data;
using Npgsql;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// ================= DATABASE =================

var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");



if (!string.IsNullOrEmpty(databaseUrl))
{
    // 🔵 RENDER / PROD → POSTGRES
    var databaseUri = new Uri(databaseUrl);
    var userInfo = databaseUri.UserInfo.Split(':');

    var connectionStringBuilder = new NpgsqlConnectionStringBuilder
    {
        Host = databaseUri.Host,
        Port = databaseUri.Port > 0 ? databaseUri.Port : 5432,
        Username = userInfo[0],
        Password = userInfo[1],
        Database = databaseUri.AbsolutePath.Trim('/'),
        SslMode = SslMode.Require,
        TrustServerCertificate = true
    };

    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseNpgsql(connectionStringBuilder.ConnectionString));
}
else
{
    // 🟢 LOCAL → SQLITE
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlite("Data Source=app.db"));
}

// ================= IDENTITY =================

builder.Services
    .AddDefaultIdentity<IdentityUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        options.Password.RequiredLength = 6;
        options.Password.RequireDigit = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddRazorPages();

// ================= TÜRKÇE =================

builder.Services.AddLocalization(options =>
{
    options.ResourcesPath = "Resources";
});

var app = builder.Build();


var cultures = new[] { new CultureInfo("tr-TR") };
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("tr-TR"),
    SupportedCultures = cultures,
    SupportedUICultures = cultures
});

// ================= AUTO MIGRATION =================

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}




// ================= PIPELINE =================

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
