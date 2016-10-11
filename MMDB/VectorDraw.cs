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
            line.X1 = p1.X;
            line.Y1 = p1.Y;
            line.X2 = p2.X;
            line.Y2 = p2.Y;
            line.StrokeThickness = strokeThickness;
            line.Stroke = color;

            return line;
        }

        public Ellipse CreateEllipse(Point p1,int width, int height, int strokeThickness, Brush strokeBrush, Brush fillBrush, double parentWidth, double parentHeight)
        {
            Ellipse ellipse = new Ellipse();
            ellipse.Height = height;
            ellipse.Width = width;
            ellipse.StrokeThickness = strokeThickness;
            ellipse.Stroke = strokeBrush;
            ellipse.Fill = fillBrush;
            double leftMargin = p1.X - width / 2;
            double topMargin = p1.Y - height / 2;
            if (leftMargin < 0) leftMargin = 0;
            if (leftMargin > parentWidth - width) leftMargin = parentWidth - width;
            if (topMargin < 0) topMargin = 0;
            if (topMargin > parentHeight - height) topMargin = parentHeight - height;
            ellipse.Margin = new Thickness(leftMargin, topMargin, 0, 0);
            return ellipse;
        }

        public String ParametersToString(Line line)
        {
            return "line: p1=(" + line.X1 + "," + line.Y1 + "), "
                + "p2=(" + line.X2 + "," + line.Y2 + "), "
                + "stroke=" + line.StrokeThickness + ", "
                + "brush=" + line.Stroke;
        }

        public String ParametersToString(Ellipse ellipse)
        {
            return "ellipse: p=("
                + (ellipse.Margin.Left + ellipse.Width/2) + ","
                + (ellipse.Margin.Top + ellipse.Height/2) + "), "
                + "width=" + ellipse.Width + ", "
                + "height=" + ellipse.Height + ", "
                + "stroke=" + ellipse.StrokeThickness + ", "
                + "brush=" + ellipse.Stroke + ", "
                + "fill=" + ellipse.Fill;
        }
    }
}
