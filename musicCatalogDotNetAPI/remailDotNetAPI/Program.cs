using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using musicCatalogDotNetAPI;
using musicCatalogDotNetAPI.Infrastructure;
using musicCatalogDotNetAPI.Services;
using System.ComponentModel;
using System.Text;
using System.Text.Json.Serialization;

//var allowCORS = "_allowCORS";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>      //nowe @ robienie uploading songs - z https://github.com/dotnet/runtime/issues/47569
    {
        //options.JsonSerializerOptions.IgnoreNullValues = true;
        //options.JsonSerializerOptions.Converters.Add(new TimeSpanConverter());
        //options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
///builder.Services.AddSwaggerGen();
////    !!!! dodanie Auth widgeta do swaggera:
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "JWTToken_Auth_API",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});


/// add jwt
var jwtTokenConfig = builder.Configuration.GetSection("jwtTokenConfig").Get<JwtTokenConfig>();      /// get settings for JwtBearer from appsettings.json jwtTokenConfig object
builder.Services.AddSingleton(jwtTokenConfig);
builder.Services.AddAuthentication(x =>                                     /// add auth here, using JwtBearer scheme
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>                                                        /// add JwtBearer for auth here, with settings
{
    x.RequireHttpsMetadata = true;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtTokenConfig.Issuer,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtTokenConfig.Secret)),
        ValidAudience = jwtTokenConfig.Audience,
        ValidateAudience = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.FromMinutes(1)
    };
});
builder.Services.AddSingleton<IJwtAuthManager, JwtAuthManager>();   /// register services so they can be resolved in requests
//builder.Services.AddHostedService<JwtRefreshTokenCache>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ISongService, SongService>();


var ConnectionString = builder.Configuration.GetSection("ConnectionStrings").Get<ConnectionStrings>();      //unsure how to pass this to constructor, seems to be done from controller not from here, leaving it as is for now because I got sidetracked 
builder.Services.AddDbContext<musicCatalogDotNetAPI.Services.DBService>();    //register DBService as a service, to use local SQLite database, as defined in DBService.cs



/*builder.Services.AddCors(options =>
{
    options.AddPolicy(name: allowCORS,
                      policy =>
                      {
                          policy.WithOrigins("*", "https://localhost", "https://localhost:7026");
                      });
});*/
builder.Services.AddCors();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseCors(allowCORS);
app.UseCors(
    options => options.WithOrigins("*").AllowAnyMethod().AllowAnyHeader()
//WithMethods("POST", "GET", "PUT", "DELETE") //AllowAnyMethod()
);


app.UseAuthentication();    //before UseAuth!
app.UseAuthorization();

app.MapControllers();

app.Run();
