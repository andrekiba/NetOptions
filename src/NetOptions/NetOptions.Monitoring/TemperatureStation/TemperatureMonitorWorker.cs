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
        logger.LogInformation("Current sensor station options: {Options}", options.CurrentValue);

        while (!stoppingToken.IsCancellationRequested)
        {
            var currentOptions = options.CurrentValue;

            foreach (var (sensor, thresholds) in currentOptions.Stations ?? [])
            {
                var service = temperatureStationFactory.Create(sensor);
                var temp = service.ReadTemperature();
                AlertSensorReadings(sensor, temp, thresholds);
            }

            await Task.Delay(currentOptions.PollingInterval, stoppingToken);
        }
    }

    void AlertSensorReadings(string sensor, double temp, TemperatureThresholdOptions temperatureThresholds)
    {
        var isLowerThanOrAtMax = temp <= temperatureThresholds.High;
        var isGreaterThanOrAtMin = temp >= temperatureThresholds.Low;

        if (isLowerThanOrAtMax && isGreaterThanOrAtMin)
        {
            logger.LogInformation("Normal '{Sensor}' reading: {Temp:F2}°C", sensor, temp);
        }
        else
        {
            logger.LogCritical("The '{Sensor}' reading is out of range: {Temp:F2}°C", sensor, temp);
        }
    }

    void OnOptionsChanged(TemperatureStationOptions latestOptions)
    {
        logger.LogInformation("Threshold's changed: {Options}", latestOptions);
    }

    public override void Dispose()
    {
        onChangeDisposable?.Dispose();
        base.Dispose();
    }
}