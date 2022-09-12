using WebApplication1.Models;
using Microsoft.EntityFrameworkCore;

string connectionString = "Host=localhost;Port=5432;Database=OnlyTest;Username=postgres;Password=1998";

var builder = WebApplication.CreateBuilder(args);
//Добавляем контекст для взаимодействия с БД
builder.Services.AddDbContext<CompanyStructureContext>(option => option.UseNpgsql(connectionString));
builder.Services.AddControllers();

var app = builder.Build();
app.UseRouting();
app.UseEndpoints(endpoints => endpoints.MapControllers());
app.Run();
