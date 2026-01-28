using Microsoft.Extensions.Options;

namespace NetOptions.Monitoring.TemperatureStation;

internal sealed class TemperatureMonitorWorker : BackgroundService
{
    readonly TemperatureStationFactory temperatureStationFactory;
    readonly IOptionsMonitor<TemperatureStationOptions> options;
    readonly ILogger<TemperatureMonitorWorker> logger;
    readonly IDisposable? onChangeDisposable;

    public TemperatureMonitorWorker(
        TemperatureStationFactory temperatureStationFactory,
        IOptionsMonitor<TemperatureStationOptions> options,
        ILogger<TemperatureMonitorWorker> logger)
    {
        this.temperatureStationFactory = temperatureStationFactory;
        this.options = options;
        this.logger = logger;
        onChangeDisposable = options.OnChange(OnOptionsChanged);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Station options at start:\n{Options}", options.CurrentValue);

        while (!stoppingToken.IsCancellationRequested)
        {
            var currentOptions = options.CurrentValue;

            foreach (var (stationName, thresholds) in currentOptions.Stations ?? [])
            {
                var station = temperatureStationFactory.GetOrCreate(stationName);
                var temp = station.ReadTemperature();
                AlertStationReadings(stationName, temp, thresholds);
            }

            await Task.Delay(currentOptions.PollingInterval, stoppingToken);
        }
    }

    void AlertStationReadings(string stationName, double temp, TemperatureThresholdOptions temperatureThresholds)
    {
        var isInRange = temp >= temperatureThresholds.Low && temp <= temperatureThresholds.High;

        if (isInRange)
            logger.LogInformation("Normal '{Station}' reading: {Temp:F2}°C", stationName, temp);
        else
            logger.LogCritical("The '{Station}' reading is out of range: {Temp:F2}°C", stationName, temp);
    }

    void OnOptionsChanged(TemperatureStationOptions latestOptions)
    {
        logger.LogInformation("Station options changed:\n{Options}", latestOptions);
    }

    public override void Dispose()
    {
        onChangeDisposable?.Dispose();
        base.Dispose();
    }
}