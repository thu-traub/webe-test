public class BackgroundWorker : BackgroundService
{
    private readonly ILogger<BackgroundWorker> logger;
    // private readonly IDataStore ds;

    public BackgroundWorker(ILogger<BackgroundWorker> logger)
    {
        this.logger = logger;
//        this.ds = ds;
    }

    protected override Task ExecuteAsync(CancellationToken ct) {
        // logger.LogWarning("Background Task started");
        // Person p = new("Hans", "Mayer", "12345");
        // ds.createPerson(p);
        return Task.CompletedTask;
    }
}