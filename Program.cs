using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebUygulamaProje1.Models;
using WebUygulamaProje1.Utility;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();



builder.Services.AddDbContext<UygulamaDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Dikkat: her yeni repository eklediğinizde, burada da onu eklemeyi unutmayın. Aksi takdirde, o repository'yi kullanmaya çalıştığınızda hata alırsınız.

// Dependency Injection (Bağımlılık Enjeksiyonu) yapılandırması
// IKitapTuruRepository -> KitapTuruRepository'yi kullanarak oluşturulacak. Yani IKitapTuruRepository'ye ihtiyaç duyan bir sınıf olduğunda, KitapTuruRepository'nin bir örneği sağlanacak.
builder.Services.AddScoped<IKitapTuruRepository, KitapTuruRepository>();

// IKitapRepository -> KitapRepository'yi kullanarak oluşturulacak. Yani IKitapRepository'ye ihtiyaç duyan bir sınıf olduğunda, KitapRepository'nin bir örneği sağlanacak.
builder.Services.AddScoped<IKitapRepository, KitapRepository>();

// IKiralamaRepository -> KiralamaRepository'yi kullanarak oluşturulacak. Yani IKiralamaRepository'ye ihtiyaç duyan bir sınıf olduğunda, KiralamaRepository'nin bir örneği sağlanacak.
builder.Services.AddScoped<IKiralamaRepository, KiralamaRepository>();


builder.Services.AddDbContext<UygulamaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDefaultIdentity<IdentityUser>()
    .AddEntityFrameworkStores<UygulamaDbContext>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
