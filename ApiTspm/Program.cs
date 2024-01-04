using ApiTspm.Utilidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext >(o => o.UseSqlServer("name=ConnectionStrings:default"));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddSingleton<IAlmacenadorArchivos, AlmacenadorDeArchivosEnLocal>();
builder.Services.AddHttpContextAccessor();


////CORS  ******** Eliminar cuando se suba a producción !!!!!!!!!!!
builder.Services.AddCors(options =>
{
    options.AddPolicy("MyAllowedOrigins",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200", "https://todo-san-pablo-del-monte.web.app", "https://localhost:4200") // fíjate que el puerto incluido
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

}







////A continuación las peticiones a las que quieres que responda tu API
//app.MapGet("/", () =>
//{
//    string html = "<h1>Hola mundo</h1>";
//    return Results.Content(html, "text/html");
//});

app.UseHttpsRedirection();


app.UseStaticFiles();

//app.UseStaticFiles(new StaticFileOptions
//{
//    FileProvider = new PhysicalFileProvider(Path.Combine(
//    AppDomain.CurrentDomain.BaseDirectory,
//    "wwwroot")),
//});

app.UseCors("MyAllowedOrigins");

app.UseAuthorization();

app.MapControllers();

Console.WriteLine($"WebRootPath: {builder.Environment.WebRootPath}");

app.Run();
