using Microsoft.EntityFrameworkCore;
using OptionChain;
using Quartz;
using Quartz.Impl;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string connectionString = "Data Source=DESKTOP-PKUGHDC;Initial Catalog=OptionChain;Integrated Security=True;TrustServerCertificate=True";

builder.Services.AddDbContext<OptionDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddQuartz(q =>
{
    var fetchingJob = JobKey.Create("FetchingJob");
    var finalFetchJob = JobKey.Create("FinalFetchJob");

    q.AddJob<FetchAndProcessJob>(fetchingJob)
        .AddTrigger(trigger =>
        {
            trigger.ForJob(fetchingJob)
                .WithCronSchedule("0 15/5 9-15 ? * MON-FRI *"); // Start 9:15 AM to 3:30 PM
                //.WithSimpleSchedule(s => s.WithRepeatCount(0));
                //.WithCronSchedule("0 * * ? * *"); // Start of Every Minute                
        });
    
    q.AddJob<FetchAndProcessJob>(finalFetchJob)
        .AddTrigger(trigger =>
        {
            trigger.ForJob(finalFetchJob)
                .WithCronSchedule("0 45 15 ? * MON-FRI *"); // Final Call 3:45 PM
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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();