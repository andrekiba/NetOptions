using Microsoft.Extensions.Options;
using NetOptions.Validation.Infrastructure.Features;

namespace NetOptions.Validation;

public class Worker(
    IOptionsMonitor<FeatureOptionsWithManualValidate> manualOptions,
    IOptionsMonitor<FeatureOptions> options, 
    ILogger<Worker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation(
                    "PreorderBike feature options: {Options}",
                    GetNamedOptionsAndLogValidationErrors("PreorderBike"));

                logger.LogInformation(
                    "CustomizeBike feature options:\n{Options}",
                    GetNamedOptionsAndLogValidationErrors("CustomizeBike"));
            }

            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }
    }
    
    string GetNamedOptionsAndLogValidationErrors(string name)
    {
        try
        {
            var validation = Environment.GetEnvironmentVariable("OptionsValidation");
            return validation switch
            {
                "Manual" => manualOptions.Get(name).ToString(),
                "Annotations" => options.Get(name).ToString(),
                _ => string.Empty
            };
        }
        catch (OptionsValidationException ex)
        {
            logger.LogError("{Name} ({Type}): {Errors}",
                ex.OptionsName, ex.OptionsType, ex.Message);

            return string.Empty;
        }
    }
}