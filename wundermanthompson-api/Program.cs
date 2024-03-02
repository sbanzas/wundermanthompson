using wundermanthompson_api.persistence;
using wundermanthompson_api.services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<InMemoryDbContext>();
builder.Services.AddScoped<IDataJobRepository, DataJobRepository>();
builder.Services.AddScoped<ILinksRepository, LinksRepository>();
builder.Services.AddScoped<IResultsRepository, ResultsRepository>();
builder.Services.AddTransient<IDataProcessorService, DataProcessorService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(s =>
{
    s.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Wunderman Thompson API",
        Version = "v1",
        Description = "Wunderman Thompson API",
    });
});

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Wunderman Thompson API V1");
});


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
