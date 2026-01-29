using System.Text;
using Microsoft.Extensions.Options;

namespace NetOptions.Validation.Infrastructure.Features;

public sealed record FeatureOptionsWithManualValidate : IValidateOptions<FeatureOptionsWithManualValidate>
{
    public bool Enabled { get; set; }
    public string? Name { get; set; }
    public string? Version { get; set; }
    public Uri? Endpoint { get; set; }
    public string? ApiKey { get; set; }
    //public string[] Tags { get; set; } = [];
    public TagItem[] Tags { get; set; } = [];
    public record TagItem
    {
        public required string Value { get; set; }
    }
    
    //called one time per named option
    public ValidateOptionsResult Validate(string? name, FeatureOptionsWithManualValidate options)
    {
        if (IsNamed(name, "PreorderBike"))
        {
            if (!options.Enabled)
                return ValidateOptionsResult.Success;
            
            var errors = new List<string>();
            
            if (options.Endpoint is null)
                errors.Add("The Endpoint is required.");
            
            if (string.IsNullOrWhiteSpace(options.ApiKey))
                errors.Add("The ApiKey is required.");

            if (errors.Count > 0)
                ValidateOptionsResult.Fail(errors);
        }
        
        if (IsNamed(name, "CustomizeBike") && options is {Enabled: true})
        {
            return ValidateOptionsResult.Fail("The CustomizeBike feature is not yet implemented.");
        }
        
        return ValidateOptionsResult.Skip;

        static bool IsNamed(string? name, string expectedName)
        {
            return string.Equals(name, expectedName, StringComparison.OrdinalIgnoreCase);
        }
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