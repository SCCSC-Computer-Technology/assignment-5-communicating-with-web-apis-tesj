using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.SwaggerUI;
using T_Speich_CPT_206_Lab_5;
using T_Speich_CPT_206_Lab_5.Repositories;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    Console.WriteLine("Default output formatters: ");
    foreach (IOutputFormatter formatter in options.OutputFormatters)
    {
        OutputFormatter? mediaformmater = formatter as OutputFormatter;
        if (mediaformmater == null)
        {
            Console.WriteLine($" {formatter.GetType().Name}.");
        }
        else
        {
            Console.WriteLine(" {0}, Media Types: {1}", arg0: mediaformmater.GetType().Name, arg1: string.Join(", ",
                mediaformmater.SupportedMediaTypes));
        }
    }
}).AddXmlDataContractSerializerFormatters().AddXmlSerializerFormatters();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "CPT 206 Service Web API", Version = "v1" });
});

string root = AppContext.BaseDirectory.Substring(0, AppContext.BaseDirectory.Length - 40) + "T-Speich-CPT-206-Lab-5";
builder.Services.AddDbContext<StateDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("StateData").Replace("%ROOT%", root)));

builder.Services.AddScoped<IStateRepository, StateRepository>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "T-Speich-CPT-206-Lab-5 States API");
        c.SupportedSubmitMethods(new[] { SubmitMethod.Get, SubmitMethod.Post, SubmitMethod.Put, SubmitMethod.Delete });
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
