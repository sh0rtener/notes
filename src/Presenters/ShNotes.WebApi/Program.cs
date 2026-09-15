using Microsoft.EntityFrameworkCore;
using ShNotes.Caching;
using ShNotes.Data;
using ShNotes.Data.EntityFramework.Contexts;
using ShNotes.UseCases;
using ShNotes.WebApi;
using ShNotes.WebApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterJwt(builder.Configuration);
builder.Services.AddJwtAuth(builder.Configuration);
builder.Services.AddData(builder.Configuration);
builder.Services.AddUseCases();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();
builder.Services.AddCaching();

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "Angular",
        policy =>
        {
            policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        }
    );
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.Migrate();
}
app.UseCors("Angular");
app.MapControllers();

app.UseMiddleware<EndpointHandlerMiddleware>();

app.Run();
