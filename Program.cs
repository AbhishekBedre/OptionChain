using Microsoft.EntityFrameworkCore;
using OptionChain;
using Quartz;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<OptionDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddQuartz(q =>
{
    var firstSession = JobKey.Create("FirstSession");
    var midSession = JobKey.Create("MidSession");
    var lastSession = JobKey.Create("LastSession");
    var finalCall = JobKey.Create("FinalCall");

    q.AddJob<FetchAndProcessJob>(firstSession)
        .AddTrigger(trigger =>
        {
            trigger.ForJob(firstSession)
                .WithCronSchedule("0 15-59/5 9 ? * MON-FRI"); // From 9:15 AM to 9:59 AM, Monday to Friday
                //.WithSimpleSchedule(s => s.WithRepeatCount(0));
                //.WithCronSchedule("0 * * ? * *"); // Start of Every Minute                
        });

    q.AddJob<FetchAndProcessJob>(midSession)
        .AddTrigger(trigger =>
        {
            trigger.ForJob(midSession)
                .WithCronSchedule("0 0-59/5 10-14 ? * MON-FRI"); // From 10:00 AM to 2:59 PM, Monday to Friday
        });


    q.AddJob<FetchAndProcessJob>(lastSession)
        .AddTrigger(trigger =>
        {
            trigger.ForJob(lastSession)
                .WithCronSchedule("0 0-30/5 15 ? * MON-FRI"); // From 3:00 PM to 3:30 PM, Monday to Friday
        });

    q.AddJob<FetchAndProcessJob>(finalCall)
        .AddTrigger(trigger =>
        {
            trigger.ForJob(finalCall)
                .WithCronSchedule("0 45 15 ? * MON-FRI"); // At 3:40 PM, Monday to Friday
        });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", builder =>
    {
        builder.AllowAnyOrigin() //.WithOrigins("https://localhost", "http://localhost", "https://localhost/", "http://localhost/") // Add your allowed origins here
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

builder.Services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();