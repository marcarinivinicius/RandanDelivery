using Microsoft.OpenApi.Models;
using Vehicle.API.Middlewares;
using Vehicle.API.Profiles;
using Vehicle.Infra;
using Vehicle.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "RandanDelivery Vehicle API",
        Version = "v1",
        Description = "This API is builded for challenge in the other Company",
        Contact = new OpenApiContact
        {
            Name = "Vinícius A. Marcarini",
            Email = "viniciusantoniomarcarini@gmail.com",
            Url = new Uri("https://www.linkedin.com/in/marcarinivinicius/")
        }
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "The Bearer token is required.",
        Type = SecuritySchemeType.ApiKey
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
      {
        {
          new OpenApiSecurityScheme
          {
            Reference = new OpenApiReference
              {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
              }
            },
            new List<string>()
          }
        });
});

builder.Services.AddInfraModules();
builder.Services.AddServicesModules();

builder.Services.AddControllers();

builder.Services.AddAutoMapper(typeof(MotoProfile));
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
app.UseRabbitListener();
app.UseAWSListener();
app.UseMiddleware<ValidateAuthMiddleware>();
app.Run();
