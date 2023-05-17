using GeoCoordinatePortable;
using VehicleTracker.FileReader;

namespace VehicleTracker.Fxns;

public class Location
{
    
    public static VehiclePosition FindClosestCoordinate(List<VehiclePosition> coordinates, Coord queryPoint)
    {
        return FindClosestCoordinateRecursive(coordinates, queryPoint, 0, coordinates.Count - 1);
    }

    static VehiclePosition FindClosestCoordinateRecursive(List<VehiclePosition> coordinates, Coord queryPoint, int left, int right)
    {
        if (left == right)
        {
            return coordinates[left];
        }

        int mid = (left + right) / 2;
        coordinates[mid].Distance = Distance(queryPoint, coordinates[mid]);
        if (Math.Abs(queryPoint.Longitude - coordinates[mid].Longitude) < 0.000001 &&  (Math.Abs(queryPoint.Longitude - coordinates[mid].Longitude) < 0.000001))
        {
            return coordinates[mid];
        }
        else if (queryPoint.Longitude < coordinates[mid].Longitude || (Math.Abs(queryPoint.Longitude - coordinates[mid].Longitude) < 0.000001 && queryPoint.Latitude < coordinates[mid].Latitude))
        {
            if (mid > 0 && Distance(queryPoint, coordinates[mid - 1]) < Distance(queryPoint, coordinates[mid]))
            {
                return FindClosestCoordinateRecursive(coordinates, queryPoint, left, mid - 1);
            }
            else
            {
                return coordinates[mid];
            }
        }
        else
        {
            if (mid < coordinates.Count - 1 && Distance(queryPoint, coordinates[mid + 1]) < Distance(queryPoint, coordinates[mid]))
            {
                return FindClosestCoordinateRecursive(coordinates, queryPoint, mid + 1, right);
            }
            else
            {
                return coordinates[mid];
            }
        }
    }

    static double Distance(Coord point1, VehiclePosition point2)
    {
        double dx = point1.Longitude - point2.Longitude;
        double dy = point1.Latitude - point2.Latitude;
        return Math.Sqrt(dx * dx + dy * dy);
    }
    public class CoordinateComparer : IComparer<VehiclePosition>
    {
        public int Compare(VehiclePosition c1, VehiclePosition c2)
        {
            int result = c1.Longitude.CompareTo(c2.Longitude);
            if (result == 0)
                result = c1.Latitude.CompareTo(c2.Latitude);
            return result;
        }
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