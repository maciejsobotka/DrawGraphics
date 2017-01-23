using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MMDB.Utils
{
    public static class CalculatePerimeter
    {
        public static double GetPerimeter(XmlNode shape)
        {
            switch (shape.Name)
            {
                case "Ellipse":
                    return 2 * Math.PI * Math.Sqrt((Math.Pow(double.Parse(shape.Attributes["Height"].Value, CultureInfo.InvariantCulture), 2) + Math.Pow(double.Parse(shape.Attributes["Width"].Value, CultureInfo.InvariantCulture), 2)) / 8);
                case "Rectangle":
                    return 2 * (double.Parse(shape.Attributes["Height"].Value, CultureInfo.InvariantCulture) + double.Parse(shape.Attributes["Width"].Value, CultureInfo.InvariantCulture));
                case "Polygon":
                    string[] pointCoords = shape.Attributes["Points"].Value.Split(new[] {',', ' '}, StringSplitOptions.RemoveEmptyEntries);
                    List<double> points = new List<double>();
                    foreach (var pointCoord in pointCoords)
                    {
                        points.Add(double.Parse(pointCoord, CultureInfo.InvariantCulture));
                    }
                    double a = Math.Sqrt(Math.Pow(points[0] - points[2], 2) + Math.Pow(points[1] - points[3], 2));
                    double b = Math.Sqrt(Math.Pow(points[2] - points[4], 2) + Math.Pow(points[3] - points[5], 2));
                    double c = Math.Sqrt(Math.Pow(points[4] - points[0], 2) + Math.Pow(points[5] - points[1], 2));
                    return a + b + c;
                default:
                    return 0.0;
            }
        }
    }
}
