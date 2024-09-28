using MinimalBankAPI.CrossCuttingConcerns;
using MinimalBankAPI.Security;
using MinimalBankAPI.Bussines;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



// AddDataLayerDPIs methodunu çağırıyoruz
builder.Services.AddDataLayerDPIs(builder.Configuration);
//builder.Services.AddBussinesLayer();

//Katman Registrationları
builder.Services.AddCrossCuttingConcern();
builder.Services.AddSecurityServices();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
