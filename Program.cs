using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

{
    var appConnectionString = builder.Configuration.GetConnectionString("DbContextConnection") ?? throw new InvalidOperationException("Connection string 'PlopDbContextConnection' not found.");
    builder.Services.AddDbContext<BlankyBlankLibrary.Data.AppDbContext>(options => {
        options.UseSqlite(appConnectionString);
    });
    builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<BlankyBlankLibrary.Data.AppDbContext>();
}

builder.Services.AddScoped<BlankyBlankLibrary.Services.PasswordServices>();
builder.Services.AddScoped<BlankyBlankLibrary.Services.StructureServices>();
builder.Services.AddScoped<BlankyBlankLibrary.Services.WordServices>();

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

using (var scope = app.Services.CreateScope()) {

    var db = scope.ServiceProvider.GetRequiredService<BlankyBlankLibrary.Data.AppDbContext>();
    
    // db.Database.EnsureDeleted();
    db.Database.EnsureCreated();
    
    var um = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    var host = await um.FindByNameAsync("host");
    if (host == null) {
        host = new IdentityUser() {
                UserName = "host",
                Email = "bbl.spamjerky@spamgourmet.com"
            };
        var resultsCreate = await um.CreateAsync(host);
        if (!resultsCreate.Succeeded) throw new Exception("Failed to create host user.");
        var resultAddPassword = await um.AddPasswordAsync(host, "devp@SS1");
        if (!resultAddPassword.Succeeded) throw new Exception("Failed to set host password.");
    }

    await db.SaveChangesAsync();
}

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
