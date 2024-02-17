using Quartz;

namespace FisMonitoring.Jobs;

/// <summary>
/// https://www.quartz-scheduler.net/documentation/quartz-2.x/tutorial/more-about-jobs.html#job-state-and-concurrency
/// </summary>
/// <param name="logger"></param>
[PersistJobDataAfterExecution, DisallowConcurrentExecution]
public class AliveJob(ILogger<AliveJob> logger) : IJob
{
    private static readonly Random Random = new();

    public async  Task Execute(IJobExecutionContext context)
    {
        var dataMap = context.JobDetail.JobDataMap;

        var counter = dataMap.GetInt("Counter");

        var delay = Random.Next(100, 5000);
        logger.LogInformation("Triggered AliveJob: {Delay}, Counter = {Counter}", delay, counter);

        await Task.Delay(delay);
        logger.LogInformation("  -> Finnish AliveJob: {Delay}, Counter = {Counter}\r\n", delay, counter);

        dataMap.Put("Counter", ++counter);
    }
}
