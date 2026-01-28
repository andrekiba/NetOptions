using Microsoft.Extensions.Options;
using NetOptions.Validation.Infrastructure.Features;

namespace NetOptions.Validation;

public class Worker(IOptionsMonitor<FeatureOptions> options, ILogger<Worker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation(
                    "PreorderBike feature options: {Options}",
                    GetNamedOptionsAsLogString("PreorderBike"));

                logger.LogInformation(
                    "CustomizeBike feature options:\n{Options}",
                    GetNamedOptionsAsLogString("CustomizeBike"));
            }

            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }
    }
    
    string GetNamedOptionsAsLogString(string name)
    {
        try
        {
            return options.Get(name).ToString();
        }
        catch (OptionsValidationException ex)
        {
            logger.LogError("{Name} ({Type}): {Errors}",
                ex.OptionsName, ex.OptionsType, ex.Message);

            return string.Empty;
        }
    }
}