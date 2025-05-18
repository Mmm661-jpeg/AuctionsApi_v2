using AuctionsApi_v2.Core.Interfaces;
using AuctionsApi_v2.Core.Services;
using AuctionsApi_v2.Data.DataModels;
using AuctionsApi_v2.Data.Interfaces;
using AuctionsApi_v2.Data.Repositories;
using AuctionsApi_v2.Domain.UtilityModels;
using AuctionsApi_v2.HelperMethods.Helpers;
using AuctionsApi_v2.HelperMethods.Interfaces;
using AuctionsApi_v2.Middleware.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using System.Text.Json.Serialization; 



var builder = WebApplication.CreateBuilder(args);


builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

var jwtSettings = builder.Configuration.GetSection("JwtSettings");


builder.Services.AddAuthenticationExtentsion(
    issuer: jwtSettings["Issuer"]
    , audience: jwtSettings["Audience"]
    , signingKey: jwtSettings["Secret"]);

builder.Services.AddAuthorization();

builder.Services.AddSwaggerExtended();


var corsPolicy = builder.Configuration.GetSection("Cors");

builder.Services.AddCors(options =>
{
    options.AddPolicy("P1", policy =>
    {
        policy.AllowAnyMethod().AllowAnyHeader()
        .WithOrigins(corsPolicy["Origin"]);
    });
});




//builder.Services.AddControllers().AddJsonOptions(options =>
//{
//    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
//});

builder.Services.AddControllers();

builder.Services.AddDbContext<AuctionDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
   

});

builder.Services.AddScoped<IAuctionsRepo,AuctionsRepo>();
builder.Services.AddScoped<IAuctionsService, AuctionsService>();

builder.Services.AddScoped<IBidsRepo,BidsRepo>();
builder.Services.AddScoped<IBidsService, BidsService>();

builder.Services.AddScoped<ICategoriesRepo,CategoriesRepo>();
builder.Services.AddScoped<ICategoriesService,CategoriesService>();

builder.Services.AddScoped<IUsersRepo,UsersRepo>();
builder.Services.AddScoped<IUsersService, UsersService>();

builder.Services.AddTransient<IJwtService,JwtService>();
builder.Services.AddScoped<ITokenReader,TokenReader>();








var app = builder.Build();
app.UseRouting();

app.UseCors("P1");

app.UseAuthentication();
app.UseAuthorization();

app.UseSwaggerExtended();


app.UseEndpoints(endpoints => { endpoints.MapControllers(); });



app.Run();
