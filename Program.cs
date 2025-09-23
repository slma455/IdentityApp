using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Linq;
using System.Text;
using webApplication.Context;
using webApplication.Models;
using webApplication.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Register DbContext with SQL Server
builder.Services.AddDbContext<DbContextApp>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString")));
//to inject service inside our controller
builder.Services.AddScoped<JWTService>();
//first step
// define our identity service
builder.Services.AddIdentityCore<User>(options =>
{
    //password config
    options.Password.RequiredLength = 6;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    //email config
    options.SignIn.RequireConfirmedEmail = true;

}).AddRoles<IdentityRole>() // to be able to add roles
.AddEntityFrameworkStores<DbContextApp>() //providing context 
.AddSignInManager<SignInManager<User>>()  //make use of sign in manager
.AddUserManager<UserManager<User>>() //to make use of UserManager to create users
.AddDefaultTokenProviders(); //to be able to create tokens for email confirmation 
//3rd step
//to Authenticat this user by using jwt
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        //validate the token based on the key we have provided inside appsetting 
        ValidateIssuerSigningKey = true,
        // the issuer  signing key based on jwt key
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
        //the issuer which in here is the api project  url you are using 
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        //validate the issuer 
        ValidateIssuer = true,
        //dont validate audience(angular side)
        ValidateAudience = false,
    };
});
// In Program.cs or Startup.cs
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularDevServer",
        builder => builder
            .WithOrigins("http://localhost:4200") // Your Angular URL
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory =
    actionContext =>
    {
        var errors = actionContext.ModelState.Where(x => x.Value.Errors.Count() > 0)
        .SelectMany(x => x.Value.Errors).Select(x => x.ErrorMessage).ToArray();
        var toReturn = new
        {
            Error = errors
        };
        return new BadRequestObjectResult(toReturn);
    };
  
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAngularDevServer"); // Before UseAuthorization

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
