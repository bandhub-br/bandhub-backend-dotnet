using BandHub.UserService.Common;
using BandHub.UserService.Features.Accounts.CreateAccount;
using BandHub.UserService.Features.Accounts.GetAccounts;
using BandHub.UserService.Features.Accounts.Login;
using BandHub.UserService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AccountDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddUserService(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapLoginEndpoint();
app.MapCreateAccountEndpoint();
app.MapGetAccountsEndpoint();

app.Run();
