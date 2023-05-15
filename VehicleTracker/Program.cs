using System.Diagnostics;
using Location.TaskWrapper;
using VehicleTracker.FileReader;

namespace VehicleTracker;

public class Program
{
    public static async Task Main()
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        List<VehiclePosition> vehiclePositions = DataFileParser.ReadDataFile();

        var vehiclePositionsYSorted = vehiclePositions.OrderBy(y => y.Latitude).ToList();
        var vehiclePositionsXSorted = vehiclePositionsYSorted.OrderBy(i => i.Longitude).ToList();

        var positions = await ProcessAsync(vehiclePositionsXSorted);

        stopwatch.Stop();

        long elapsedMilliseconds = stopwatch.ElapsedMilliseconds / 1000;
        
        Console.WriteLine(elapsedMilliseconds);
        
        Console.WriteLine("finished.");
    }

    private static async Task<List<VehiclePosition>> ProcessAsync(List<VehiclePosition> vehiclePositionsXSorted)
    {
        var results = new List<VehiclePosition>();
        var lookupPositions = Fxns.Location.GetLookupPositions().ToList();
        var lookupTasks = lookupPositions.Select(i => new TaskWrapper<VehiclePosition>(() => Task.Run(() => Fxns.Location.BinarySearch(vehiclePositionsXSorted, i))).InitAsync());
        results.AddRange(await Task.WhenAll(lookupTasks));

        return results;
    }
}