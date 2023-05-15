using GeoCoordinatePortable;
using VehicleTracker.FileReader;

namespace VehicleTracker.Fxns;

public class Location
{

    public static VehiclePosition BinarySearch(List<VehiclePosition> positions, Coord target)
    {
        return BinarySearch(positions, target, 0, positions.Count - 1);
    }

    private static VehiclePosition BinarySearch(List<VehiclePosition> positions, Coord target, int left, int right, VehiclePosition currentCoord = null ,double previousDistance = 0)
    {
        var middle = (left + right) / 2;
        
        if (right < left)
            return positions[middle];

        double distance = DistanceBetween(target.Latitude, target.Longitude, positions[middle].Latitude, positions[middle].Longitude);
        if(distance == 0)
        {
            return currentCoord;
        } 
        
        if(distance < previousDistance)
        {
            return positions[middle];
        }        
        
        if(positions[middle].Latitude < target.Latitude || (Math.Abs(positions[middle].Longitude - target.Longitude) < 0.000001 && positions[middle].Latitude < target.Latitude))
        {
            BinarySearch(positions, target, left, middle -1,  positions[middle], distance);
        }
    
        return BinarySearch(positions, target, middle + 1, right,  positions[middle], distance);
    }
    
    private static double DistanceBetween(
        float latitude1,
        float longitude1,
        float latitude2,
        float longitude2)
    {
        return new GeoCoordinate(latitude1, longitude1).GetDistanceTo(new GeoCoordinate(latitude2, longitude2));
    }


    private static (double lowerXBound, double upperXBound, double lowerYBound, double upperYBound) FindPointQuadrant(Coord queryPoint, float minLat, float maxLat, float minLong, float maxLong)
    {
        var latitudeSlices = DivideArea(minLat, maxLat, 8);
        var longitudeSlices = DivideArea(minLong, maxLong, 8);

        var rangeY = latitudeSlices.Single(x => x.lowerBound <= queryPoint.Latitude && x.upperBound >= queryPoint.Latitude);
        var rangeX = longitudeSlices.Single(x => x.lowerBound <= queryPoint.Longitude && x.upperBound >= queryPoint.Longitude);

        return (rangeX.lowerBound, rangeX.upperBound, rangeY.lowerBound, rangeY.upperBound);
    }

    private static List<(double lowerBound, double upperBound)> DivideArea(double lowerBound, double upperBound,
        int numQuadrants)
    {
        List<(double lowerBound, double upperBound)> quadrants = new List<(double lowerBound, double upperBound)>();

        if (numQuadrants == 1)
        {
            quadrants.Add((lowerBound, upperBound));
        }
        else
        {
            double midpoint = (lowerBound + upperBound) / 2;

            List<(double lowerBound, double upperBound)> leftQuadrants = DivideArea(lowerBound, midpoint, numQuadrants / 2);
            List<(double lowerBound, double upperBound)> rightQuadrants = DivideArea(midpoint, upperBound, numQuadrants / 2);

            quadrants.AddRange(leftQuadrants);
            quadrants.AddRange(rightQuadrants);
        }

        return quadrants;
    }

    public static Coord[] GetLookupPositions()
    {
        Coord[] lookupPositions = new Coord[11];
        lookupPositions[0].Latitude = 34.54491f;
        lookupPositions[0].Longitude = -102.100845f;
        lookupPositions[1].Latitude = 32.345543f;
        lookupPositions[1].Longitude = -99.12312f;
        lookupPositions[2].Latitude = 33.234234f;
        lookupPositions[2].Longitude = -100.21413f;
        lookupPositions[3].Latitude = 35.19574f;
        lookupPositions[3].Longitude = -95.3489f;
        lookupPositions[4].Latitude = 31.89584f;
        lookupPositions[4].Longitude = -97.78957f;
        lookupPositions[5].Latitude = 32.89584f;
        lookupPositions[5].Longitude = -101.78957f;
        lookupPositions[6].Latitude = 34.115837f;
        lookupPositions[6].Longitude = -100.22573f;
        lookupPositions[7].Latitude = 32.33584f;
        lookupPositions[7].Longitude = -99.99223f;
        lookupPositions[8].Latitude = 33.53534f;
        lookupPositions[8].Longitude = -94.79223f;
        lookupPositions[9].Latitude = 32.234234f;
        lookupPositions[9].Longitude = -100.22222f;
        lookupPositions[10].Latitude = 31.89584f;
        lookupPositions[10].Longitude = -97.78999f;
        return lookupPositions;
    }
}