using Microsoft.EntityFrameworkCore;
using OptionChain;
using Quartz;

Console.WriteLine("Program started");

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

Console.WriteLine(connectionString);

builder.Services.AddDbContext<OptionDbContext>(options =>
    options.UseSqlServer(connectionString));

Console.WriteLine("Adding Quartz settings");

/*builder.Services.AddQuartz(q =>
{
    var firstSession = JobKey.Create("FirstSession");
    var midSession = JobKey.Create("MidSession");
    var lastSession = JobKey.Create("LastSession");
    var finalCall = JobKey.Create("FinalCall");

    
    q.AddJob<FetchAndProcessJob>(firstSession)
        .AddTrigger(trigger =>
        {
            trigger.ForJob(firstSession)
                .WithCronSchedule("0 15-59/5 9 ? * MON-FRI");            
        });

    q.AddJob<FetchAndProcessJob>(midSession)
        .AddTrigger(trigger =>
        {
            trigger.ForJob(midSession).WithCronSchedule("0 0-59/5 10-14 ? * MON-FRI"); // From 10:00 AM to 2:59 PM, Monday to Friday
        });


    q.AddJob<FetchAndProcessJob>(lastSession)
        .AddTrigger(trigger =>
        {
            trigger.ForJob(lastSession).WithCronSchedule("0 0-30/5 15 ? * MON-FRI"); // From 3:00 PM to 3:30 PM, Monday to Friday
        });

    q.AddJob<FetchAndProcessJob>(finalCall)
        .AddTrigger(trigger =>
        {
            trigger.ForJob(finalCall).WithCronSchedule("0 0 16 ? * MON-FRI"); // At 4:00 PM, Monday to Friday
        });
});*/

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

//builder.Services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

Console.WriteLine("Building app");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseCors("AllowSpecificOrigin");

app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

Console.WriteLine("Running App");

app.Run();