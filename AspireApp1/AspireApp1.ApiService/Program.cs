using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Data.SqlClient;
using AspireApp1.ApiService.Domain.Interfaces;
using AspireApp1.ApiService.Infrastructure.Repositories;


var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// JWT Authentication setup
var jwtKey = builder.Configuration["Jwt:Key"] ?? "super_secret_key_123456789012345678901234!";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "AspireApp";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "AspireAppAudience";
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});
builder.Services.AddAuthorization();


// Add services to the container.
builder.Services.AddProblemDetails();
builder.Services.AddControllers();
builder.AddSqlServerClient(connectionName: "AspireDb");
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<AspireApp1.ApiService.Application.Auth.IAuthService, AspireApp1.ApiService.Application.Auth.AuthService>();
builder.Services.AddScoped<AspireApp1.ApiService.Application.Auth.IPasswordHasher, AspireApp1.ApiService.Application.Auth.PasswordHasher>();
builder.Services.AddScoped<AspireApp1.ApiService.Application.Auth.IJwtTokenService, AspireApp1.ApiService.Application.Auth.JwtTokenService>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<AspireApp1.ApiService.Application.Posts.IPostService, AspireApp1.ApiService.Application.Posts.PostService>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<AspireApp1.ApiService.Application.Comments.ICommentsService, AspireApp1.ApiService.Application.Comments.CommentsService>();
builder.Services.AddScoped<AspireApp1.ApiService.Application.Moderation.IModerationService, AspireApp1.ApiService.Application.Moderation.ModerationService>();
// Enable Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Aspire API", Version = "v1" });
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: 'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9'"
    });
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

// Enable Swagger UI in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.
app.UseExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();
 

app.MapControllers();
app.MapDefaultEndpoints();

app.Run();
