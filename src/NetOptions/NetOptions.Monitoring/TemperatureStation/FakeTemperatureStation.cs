namespace NetOptions.Monitoring.TemperatureStation;

internal sealed class FakeTemperatureStation(
    double initialTemperature = 18, 
    double minChange = -0.05, 
    double maxChange = 0.5) : ITemperatureStation
{
    double currentTemperature = initialTemperature;

    public double ReadTemperature()
    {
        //simulate a realistic/gradual change
        var change = Random.Shared.NextDouble() * 
            (maxChange - minChange) + minChange;
        
        currentTemperature += change;

        return currentTemperature;
    }
}
