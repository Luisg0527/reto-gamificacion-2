
using Reto.Model; //carpeta que contiene la clase DataBaseContext
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Sesiones
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
 options.IdleTimeout = TimeSpan.FromHours(1);
 options.Cookie.HttpOnly = true;
 options.Cookie.IsEssential = true;
});

// Base de datos
builder.Services.Add(new ServiceDescriptor(typeof(DataBaseContext), new DataBaseContext()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
