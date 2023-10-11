using CityInfoAPI.Data;
using CityInfoAPI.Models.DatabaseSessionConnection;
using CityInfoAPI.Services;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using System.Text.Json.Serialization;

Log.Logger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.Console()
    .WriteTo.File("logs/cityinfo.txt",rollingInterval:RollingInterval.Day)
    .CreateLogger();
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.UseSerilog();
builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<FileExtensionContentTypeProvider>();
builder.Services.AddSingleton<CitiesDataStore>();
builder.Services.AddTransient<IMailService,MailManager>();
builder.Services.AddSingleton<CloudMailManager>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<ICityRepository, CityRepositoryManager>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("MyAllowSpecificOrigins",
                      policy =>
                      {
                          policy.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
                      });
});
builder.Services.AddDbContext<CityDbContext>(options => 
options.UseSqlite(builder.Configuration["ConnectionStrings:CityDBConnectionString"]));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
//Adding Authentication Configuration for the App via JwtBearer
builder.Services.AddAuthentication("Bearer").AddJwtBearer(
    options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Authentication:Issuer"],
            ValidAudience = builder.Configuration["Authentication:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Authentication:SecretForKey"]))
        };
    }
    );
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("MyAllowSpecificOrigins");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();//Routing Http Requests

app.Run();
