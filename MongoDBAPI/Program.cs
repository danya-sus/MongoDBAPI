using MongoDBAPI.Repositories;
using MongoDBAPI.Repositories.Context;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MongoSettings>(
    builder.Configuration.GetSection("MongoSettings"));

builder.Services.AddControllers();
builder.Services.AddSingleton<IAirlineContext, AirlineContext>();
builder.Services.AddTransient<ICompanyRepository, CompanyRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
