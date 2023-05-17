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

        vehiclePositions.Sort(new Fxns.Location.CoordinateComparer());
       
        var positions = await ProcessAsync(vehiclePositions);

        stopwatch.Stop();

        long elapsedMilliseconds = stopwatch.ElapsedMilliseconds / 1000;
        
        positions.ForEach(x => Console.WriteLine($"Latitude {x.Latitude} : Longitude {x.Longitude}, Distance {x.Distance}") );
        
        Console.WriteLine(elapsedMilliseconds);
        
        Console.WriteLine("finished.");
    }

    private static async Task<List<VehiclePosition>> ProcessAsync(List<VehiclePosition> vehiclePositionsXSorted)
    {
        var results = new List<VehiclePosition>();
        var lookupPositions = Fxns.Location.GetLookupPositions().ToList();
        var lookupTasks = lookupPositions.Select(i => new TaskWrapper<VehiclePosition>(() => Task.Run(() => Fxns.Location.FindClosestCoordinate(vehiclePositionsXSorted, i))).InitAsync());
        results.AddRange(await Task.WhenAll(lookupTasks));

        return results;
    }
}