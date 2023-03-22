using TreeStructure;
using TreeStructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDbContext<TreeContext>();
builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.AddScoped<TreeSeeder>();
builder.Services.AddScoped<ITreeService, TreeService>();
builder.Services.AddAutoMapper(typeof(TreeMappingProfile));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(option =>
{
    option.AddPolicy("FrontEndClient", builder =>
    builder.AllowAnyMethod().AllowAnyHeader().WithOrigins("http://localhost:8086")
    );
});

var app = builder.Build();
app.UseCors("FrontEndClient");
app.UseMiddleware<ErrorHandlingMiddleware>();
SeedDatabase();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


void SeedDatabase()
{
    using (var scope = app.Services.CreateScope())
    {
        var dbSeeder = scope.ServiceProvider.GetRequiredService<TreeSeeder>();
        dbSeeder.Seed();
    }
}
