using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MMDB
{
    class VectorDraw
    {
        public Line CreateLine(Point p1, Point p2, int strokeThickness, Brush color)
        {
            Line line = new Line();
            line.Stroke = color;
            line.X1 = p1.X;
            line.Y1 = p1.Y;
            line.X2 = p2.X;
            line.Y2 = p2.Y;
            line.StrokeThickness = strokeThickness;

            return line;
        }

        public String ParametersToString(Line line)
        {
            return "line: p1=(" + line.X1 + "," + line.Y1 + "), "
                + "p2=(" + line.X2 + "," + line.Y2 + "), "
                + "stroke=" + line.StrokeThickness + ", "
                + "brush=" + line.Stroke;
        }
    }
}
