using Lab10_AlberthMayta.Configuration;
using Lab10_AlberthMayta.Infrastructure.Configuration;

var builder = WebApplication.CreateBuilder(args);

// --- Registro de Servicios ---
builder.Services.AddApiServices(builder.Configuration);

var app = builder.Build();

// --- Pipeline (Middleware) ---
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();            
    app.UseSwaggerUI();          
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();