using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Snapcart.Application.Interfaces;
using Snapcart.Application.Services;
using Snapcart.Infrastructure.Data;
using Snapcart.Infrastructure.Interfaces;
using Snapcart.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Snapcart API", Version = "v1" });
});
builder.Services.AddDbContext<SnapcartDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var githubModelsToken = builder.Configuration["Tokens:GithubModels"]!;
builder.Services.AddSingleton<IDetectProductService>(new DetectProductService(githubModelsToken));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IListRepository, ListRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IBuyRepository, BuyRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Snapcart API V1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
