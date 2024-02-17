using FisMonitoring.Jobs;
using Quartz;
using Quartz.AspNetCore;

namespace FisMonitoring.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddQuartz();

        services.AddQuartzServer(options =>
        {
            options.WaitForJobsToComplete = true;
        });


        services.AddQuartz(opt =>
        {
            var jobKey = new JobKey(nameof(AliveJob));

            opt.AddJob<AliveJob>(opts =>
            {
                opts.WithIdentity(jobKey)
                    .UsingJobData("Counter", 200);
            });

            opt.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity("AliveJob-trigger")
                .StartAt(DateTimeOffset.UtcNow.AddSeconds(10))
                // .StartNow()
                // .WithSimpleSchedule(x => x.WithInterval(TimeSpan.FromSeconds(10)).RepeatForever()))
                .WithCronSchedule("0/3 * * * * ?")) // every 3 seconds
                ;
        });

        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
    }
}
