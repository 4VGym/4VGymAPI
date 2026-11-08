
using _4VGymAPI.Data;
using _4VGymAPI.Dtos;
using _4VGymAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var errorList = context.ModelState
                .Where(e => e.Value != null && e.Value.Errors.Count > 0)
                .SelectMany(entry => entry.Value!.Errors.Select(error =>
                {
                    var parts = error.ErrorMessage.Split('|');

                    if (parts.Length == 2 && int.TryParse(parts[0], out int parsedCode))
                    {
                        return new ErrorResponse(parsedCode, parts[1]);
                    }

                    return new ErrorResponse(9999, error.ErrorMessage);
                }))
                .ToList();

            return new BadRequestObjectResult(errorList);
        };
    });


builder.Services.AddOpenApi();


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<GymDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddScoped<ActivityTypeRepository>();
builder.Services.AddScoped<InstructorRepository>();
builder.Services.AddScoped<ActivityRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "RestAPI for 4VGym v1");
    });

}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
