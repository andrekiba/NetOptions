using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.Extensions.Options;

namespace NetOptions.Validation.Infrastructure.Features;

[OptionsValidator]
public sealed partial class FeatureOptions : IValidateOptions<FeatureOptions>
{
    public bool Enabled { get; set; }
    
    [MaxLength(length: 100, ErrorMessage = "The name cannot be longer than 100 characters.")]
    public string? Name { get; set; }
    
    [RegularExpression(pattern: @"^\d+(\.\d+){1,3}$", ErrorMessage = "The version input doesn't match the regex.")]
    public string? Version { get; set; }
    
    [Required]
    public Uri? Endpoint { get; set; }
    
    [Key]
    public string? ApiKey { get; set; }
    
    //[DeniedValues(values: ["deprecated", "out-of-date"])]
    //public string[] Tags { get; set; } = [];
    
    [ValidateEnumeratedItems]
    public TagItem[] Tags { get; set; } = [];
    public record TagItem
    {
        [DeniedValues("deprecated", "out-of-date")]
        public required string Value { get; set; }
    }
    
    public override string ToString()
    {
        var builder = new StringBuilder();

        builder.Append($"Name: {Name}");
        builder.AppendLine();
        builder.Append($"Enabled: {Enabled}");
        builder.AppendLine();
        builder.Append($"Version: {Version}");
        builder.AppendLine();
        builder.Append($"Endpoint: {Endpoint}");
        builder.AppendLine();
        builder.Append($"ApiKey: {ApiKey}");
        builder.AppendLine();

        if (Tags is { Length: > 0 })
            builder.AppendLine($"Tags: {string.Join(", ", Tags.Select(tag => tag.Value))}");

        return builder.ToString();
    }
}