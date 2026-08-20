using ShNotes.Caching;
using ShNotes.Data;
using ShNotes.UseCases;
using ShNotes.WebApi;
using ShNotes.WebApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddData(builder.Configuration);
builder.Services.AddUseCases();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();
builder.Services.AddCaching();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
// app.UseMiddleware<EndpointHandlerMiddleware>();

app.Run();
