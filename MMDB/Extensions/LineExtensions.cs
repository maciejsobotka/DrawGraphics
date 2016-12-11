using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MMDB.Extensions
{
    public static class LineExtensions
    {
        public static Line Create(this Line line, Point p1, Point p2, int strokeThickness, Brush color)
        {
            line.X1 = p1.X;
            line.Y1 = p1.Y;
            line.X2 = p2.X;
            line.Y2 = p2.Y;
            line.StrokeThickness = strokeThickness;
            line.Stroke = color;

            return line;
        }
        public static void Resize(this Line line, Point p1, Point p2)
        {
            line.X1 = p1.X;
            line.Y1 = p1.Y;
            line.X2 = p2.X;
            line.Y2 = p2.Y;
        }
        public static Point GetP1(this Line line)
        {
            return new Point(line.X1, line.Y1);
        }

        public static Point GetP2(this Line line)
        {
            return new Point(line.X2, line.Y2);
        }

        //=====================================================================
        // ToString
        public static String ParametersToString(this Line line)
        {
            return "line: p1=(" + (line.X1 + line.Margin.Left) + "," + (line.Y1 + line.Margin.Top) + "), "
                    + "p2=(" + (line.X2 + line.Margin.Left) + "," + (line.Y2 + line.Margin.Top) + "), "
                    + "stroke=" + line.StrokeThickness + ", "
                    + "brush=" + line.Stroke;
        }
    }
}
