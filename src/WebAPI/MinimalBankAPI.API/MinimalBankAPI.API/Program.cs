using MinimalBankAPI.CrossCuttingConcerns;
using MinimalBankAPI.Security;
using MinimalBankAPI.Bussines;
//using MinimalBankAPI.Bussines;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



//// AddDataLayerDPIs methodunu çağırıyoruz
builder.Services.AddDataLayerDPIs(builder.Configuration);
builder.Services.AddBussinesLayer();

//Katman Registrationları
builder.Services.AddCrossCuttingConcern();
builder.Services.AddSecurityServices();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAll");
app.UseAuthorization();

app.MapControllers();

app.Run();
