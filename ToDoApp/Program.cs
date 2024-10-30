using MongoDB.Driver;
using ToDoApp.Services;

var builder = WebApplication.CreateBuilder(args);

var mongoConnectionString = builder.Configuration.GetSection("MongoDB").GetValue<string>("MongoDb");

if (string.IsNullOrEmpty(mongoConnectionString))
{
    throw new ArgumentNullException("MongoDb connection string is not set.");
}

var mongoClient = new MongoClient(mongoConnectionString);
var mongoDatabase = mongoClient.GetDatabase("TodoAppDb");

// Add services to the container.
builder.Services.AddSingleton(mongoDatabase);
builder.Services.AddScoped<MongoDbService>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<ITodoService, TodoService>();

builder.Services.AddControllers();

// Swagger/OpenAPI config
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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