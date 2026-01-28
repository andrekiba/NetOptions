using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.Extensions.Options;

namespace NetOptions.Monitoring.TemperatureStation;

[OptionsValidator]
public sealed partial class TemperatureStationOptions 
    : IValidateOptions<TemperatureStationOptions>
{
    public const string Name = "TemperatureStationOptions";

    /// <summary>
    /// Gets or sets the polling interval between sensor temperature readings.
    /// </summary>
    [Range(
        type: typeof(TimeSpan), 
        minimum: "00:00:00", 
        maximum: "23:59:59", 
        ErrorMessage = "Time must be between 00:00:00 and 23:59:59")]
    [RegularExpression(
        pattern: "^([0-1]?[0-9]|2[0-3]):[0-5][0-9]:[0-5][0-9]$", 
        ErrorMessage = "Time must be in the format HH:mm:ss and between 00:00:00 and 23:59:59")]
    public TimeSpan PollingInterval { get; set; }

    /// <summary>
    /// Gets or sets the map of named thresholds settings.
    /// </summary>
    [Required(ErrorMessage = "A mapping of station names to thresholds is required")]
    public Dictionary<string, TemperatureThresholdOptions>? Stations { get; set; } = null!;

    public override string ToString()
    {
        var builder = new StringBuilder();

        builder.Append($"Polling interval: {PollingInterval}");

        builder.AppendLine();

        if (Stations is { Count: > 0 })
            builder.AppendLine("Stations:");
        
        foreach (var (name, thresholds) in Stations ?? [])
        {
            builder.Append($"\"{name}\" — Threshold range: ({thresholds.Low:F2}°C - {thresholds.High:F2}°C)");
            builder.AppendLine();
        }

        return builder.ToString();
    }
}
