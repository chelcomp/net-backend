using InterviewAssignment;
using InterviewAssignment.Business;
using InterviewAssignment.Database;
using InterviewAssignment.Database.Repositories.DbOperation;
using InterviewAssignment.Database.Repositories.DbOperationResult;
using InterviewAssignment.Database.Setup;
using InterviewAssignment.Infrastructure.InterviewApi;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IDbConnectionFactory>(_ =>
    new SqliteConnectionFactory(config["Database:ConnectionString"]));
builder.Services.AddSingleton<DatabaseInitializer>();

builder.Services.AddScoped<IDbOperationRepository, DbOperationRepository>();
builder.Services.AddScoped<IDbOperationResultRepository, DbOperationResultRepository>();
builder.Services.AddScoped<IInterviewApi, InterviewApi>();
builder.Services.AddScoped<IEnqueueDbOperationResultRepository, EnqueueDbOperationResultRepository>();
builder.Services.AddScoped<IEnqueueDbOperationRepository, EnqueueDbOperationRepository>();
builder.Services.ConfigureInterviewApi();

builder.Services.AddScoped<IServices, Services>();

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

var databaseInitializer = app.Services.GetRequiredService<DatabaseInitializer>();
await databaseInitializer.InitializeAsync();

app.Run();