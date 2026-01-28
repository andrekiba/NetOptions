using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Options;

namespace NetOptions.Monitoring.TemperatureStation;

[OptionsValidator]
public sealed partial record TemperatureThresholdOptions : IValidateOptions<TemperatureThresholdOptions>
{
    [Range(
        minimum: -0.001d, 
        maximum: +1.000d)]
    [Required(ErrorMessage = "A low threshold value is required")]
    public double Low { get; set; }
    
    [Range(
        minimum: +1d,
        maximum: +5d)]
    [Required(ErrorMessage = "A high threshold value is required")]
    public double High { get; set; }
}
