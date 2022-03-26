using DistributedCachingPoC.Application;
using DistributedCachingPoC.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();

using (var serviceProvider = builder.Services.BuildServiceProvider())
{
    var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
    builder.Services.AddInfrastructure(builder.Configuration, loggerFactory);
}

builder.Services.AddControllers();
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
