using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using System.Xml;

namespace MMDB.Utils
{
    public static class CalculateArea
    {
        public static double GetArea(XmlNode shape)
        {
            switch (shape.Name)
            {
                case "Ellipse":
                    return Math.PI * double.Parse(shape.Attributes["Height"].Value, CultureInfo.InvariantCulture) / 2 * double.Parse(shape.Attributes["Width"].Value, CultureInfo.InvariantCulture) / 2;
                case "Rectangle":
                    return double.Parse(shape.Attributes["Height"].Value, CultureInfo.InvariantCulture) * double.Parse(shape.Attributes["Width"].Value, CultureInfo.InvariantCulture);
                case "Polygon":
                    string[] pointCoords = shape.Attributes["Points"].Value.Split(new[] {',', ' '}, StringSplitOptions.RemoveEmptyEntries);
                    List<double> points = new List<double>();
                    foreach (var pointCoord in pointCoords)
                    {
                        points.Add(double.Parse(pointCoord, CultureInfo.InvariantCulture));
                    }
                    return (points[4] - points[0]) * (points[1] - points[3]) / 2;
                default:
                    return 0.0;
            }
        }
    }
}
