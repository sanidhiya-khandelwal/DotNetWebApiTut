using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using tutWebApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers(options => options.ReturnHttpNotAcceptable = true).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<CollegeDBContext>(
   options =>
   {
       options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
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

//---- Either use useRouting & useEndpoints OR use MapControllers for mapping the controller via route------
// app.UseRouting();
// app.UseEndpoints(endpoints =>
// {
//     endpoints.MapControllers();
// });
app.MapControllers();
app.Run();