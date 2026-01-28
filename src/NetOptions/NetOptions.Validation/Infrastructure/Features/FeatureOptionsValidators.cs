namespace NetOptions.Validation.Infrastructure.Features;

internal static class FeatureOptionsValidators
{
    internal static (Func<FeatureOptions, bool> Validation, string FailureMessage) EnabledWithMissingEndpoint => (
        Validation: static options => options is not { Enabled: true, Endpoint: null },
        FailureMessage: "The weather station cannot be enabled without a valid URI."
    );
}
