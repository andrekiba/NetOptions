using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using NetOptions.Validation;
using NetOptions.Validation.Infrastructure.Features;
using static NetOptions.Validation.Infrastructure.Features.FeatureOptionsValidators;

var builder = Host.CreateApplicationBuilder(args);

#region Manual Validate

if (Environment.GetEnvironmentVariable("OptionsValidation") is "Manual")
{
    builder.Services.AddOptions<FeatureOptionsWithManualValidate>(name: "PreorderBike")
        .BindConfiguration("Features:PreorderBike")
        //.Bind(config: builder.Configuration.GetSection(key: "Features:PreorderBike"))
        .ValidateOnStart(); //instead of at runtime

    builder.Services.AddOptionsWithValidateOnStart<FeatureOptionsWithManualValidate>(name: "CustomizeBike")
        .BindConfiguration("Features:CustomizeBike");

    // Register the manual validator
    // the validator is satisfied by the FeatureOptionsWithManualValidate itself
    builder.Services.TryAddSingleton<IValidateOptions<FeatureOptionsWithManualValidate>, FeatureOptionsWithManualValidate>();    
}

#endregion

#region Annotations

if (Environment.GetEnvironmentVariable("OptionsValidation") is "Annotations")
{
    builder.Services.AddOptions<FeatureOptions>(name: "PreorderBike")
        .BindConfiguration("Features:PreorderBike")
        //.ValidateOnStart()
        .ValidateDataAnnotations();

    builder.Services.AddOptions<FeatureOptions>(name: "CustomizeBike")
        .BindConfiguration("Features:CustomizeBike")
        //.ValidateOnStart()
        //.ValidateDataAnnotations()
        
        //validate with function chaining, this only applies to CustomizeBike
        .Validate(options => options is not { Enabled: true, Endpoint: null }, 
            "The CustomizeBike feature cannot be enabled without a valid URI.")
        //or we can extract the validator to a separate class to reuse and better test it
        .Validate(
            validation: EnabledWithMissingEndpoint.Validation, 
            failureMessage: EnabledWithMissingEndpoint.FailureMessage);
}

#endregion

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();