namespace NetOptions.Monitoring.TemperatureStation;

internal sealed class TemperatureStationFactory
{
    readonly Dictionary<string, ITemperatureStation> stations = 
        new(comparer: StringComparer.OrdinalIgnoreCase);

    public ITemperatureStation GetOrCreate(string name)
    {
        if (stations.TryGetValue(name, out var station))
            return station;
        
        return stations[name] = name switch
        {
            // TODO: Add real stations with different names...
            _ => new FakeTemperatureStation(maxChange: 2.5)
        };
    }
}
