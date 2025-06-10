using Microsoft.EntityFrameworkCore;
using TesteTecnicoItau.Application;
using TesteTecnicoItau.Application.Services;
using TesteTecnicoItau.Components;
using TesteTecnicoItau.Domain.Interfaces.Infraestructure;
using TesteTecnicoItau.Infrastructure.Api;
using TesteTecnicoItau.Infrastructure.Data;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureApplication();
builder.Services.ConfigureInfrastructureApi();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.ConfigureInfrastructureData(connectionString);


// fazer as injecoes de dependencia do infraestructure 
//Automapper
builder.Services.AddHttpClient<ICotacaoB3ApiManager, CotacaoB3ApiManager>();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Add Controllers + Swagger
builder.Services.AddControllers(); // Para API REST
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAntiforgery();

app.MapControllers(); // Swagger REST

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
