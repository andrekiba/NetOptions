using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Options;

namespace NetOptions.Library;

[OptionsValidator]
public sealed partial class ToolOptions : IValidateOptions<ToolOptions>
{
    [Url, Required(AllowEmptyStrings = false)]
    public string? ImageUrl { get; set; }

    [AllowedValues(["Yellow", "Green", "Purple"])]
    public string Color { get; set; } = "Purple";

    public int Size { get; set; } = 10;

    public bool IsEnabled { get; set; } = true;
}
