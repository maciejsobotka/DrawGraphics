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
    enum Shapes { None, Line, Ellipse, Rectangle, Triangle}
    enum Operations { None, Paint, Grab}
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

        public Ellipse CreateEllipse(Point p1,Point p2, int strokeThickness, Brush strokeBrush, Brush fillBrush)
        {
            Ellipse ellipse = new Ellipse();
            ellipse.Height = Math.Abs(p1.Y - p2.Y);
            ellipse.Width = Math.Abs(p1.X - p2.X);
            ellipse.StrokeThickness = strokeThickness;
            ellipse.Stroke = strokeBrush;
            ellipse.Fill = fillBrush;
            double leftMargin = p1.X;
            double topMargin = p1.Y;
            if (p1.X > p2.X) leftMargin = p2.X;
            if (p1.Y > p2.Y) topMargin = p2.Y;
            ellipse.Margin = new Thickness(leftMargin, topMargin, 0, 0);

            return ellipse;
        }

        public Rectangle CreateRectangle(Point p1, Point p2, int strokeThickness, Brush strokeBrush, Brush fillBrush)
        {
            Rectangle rectangle = new Rectangle();
            rectangle.Height = Math.Abs(p1.Y - p2.Y);
            rectangle.Width = Math.Abs(p1.X - p2.X);
            rectangle.StrokeThickness = strokeThickness;
            rectangle.Stroke = strokeBrush;
            rectangle.Fill = fillBrush;
            double leftMargin = p1.X;
            double topMargin = p1.Y;
            if (p1.X > p2.X) leftMargin = p2.X;
            if (p1.Y > p2.Y) topMargin = p2.Y;
            rectangle.Margin = new Thickness(leftMargin, topMargin, 0, 0);

            return rectangle;
        }

        public Polygon CreateTriangle(Point p1, Point p2, int strokeThickness, Brush strokeBrush, Brush fillBrush)
        {
            Polygon triangle = new Polygon();
            Point t1, t2, t3;
            triangle.StrokeThickness = strokeThickness;
            triangle.Stroke = strokeBrush;
            triangle.Fill = fillBrush;
            if(p1.X < p2.X)                         // p1 left
                if(p1.Y > p2.Y)                     // p1 bottom left
                {
                    t1 = new Point(p1.X, p1.Y);
                    t2 = new Point(p1.X + (p2.X - p1.X)/2, p2.Y);
                    t3 = new Point(p2.X, p1.Y);
                }
                else
                {                                   // p1 top left
                    t1 = new Point(p1.X, p2.Y);
                    t2 = new Point(p1.X + (p2.X - p1.X) / 2, p1.Y);
                    t3 = new Point(p2.X, p2.Y);
                }
            else                                    // p2 left
                if (p1.Y > p2.Y)                    // p2 top left      
                {
                    t1 = new Point(p2.X, p1.Y);
                    t2 = new Point(p2.X + (p1.X - p2.X) / 2, p2.Y);
                    t3 = new Point(p1.X, p1.Y);
                }
                else
                {                                   // p2 bottom left
                    t1 = new Point(p2.X, p2.Y);
                    t2 = new Point(p2.X + (p1.X - p2.X) / 2, p1.Y);
                    t3 = new Point(p1.X, p2.Y);
                }

            PointCollection trianglePoints = new PointCollection();
            trianglePoints.Add(t1);
            trianglePoints.Add(t2);
            trianglePoints.Add(t3);
            triangle.Points = trianglePoints;

            return triangle;
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

        public String ParametersToString(Rectangle rectangle)
        {
            return "rectangle: p1=("
                + rectangle.Margin.Left + ","
                + (rectangle.Margin.Top + rectangle.Height) + "), "
                + "p2=("
                + (rectangle.Margin.Left + rectangle.Width) + ","
                + (rectangle.Margin.Top + rectangle.Height) + "), "
                + "p3=("
                + (rectangle.Margin.Left + rectangle.Width) + ","
                + rectangle.Margin.Top + "), "
                + "p4=("
                + rectangle.Margin.Left + ","
                + rectangle.Margin.Top + "), "
                + "stroke=" + rectangle.StrokeThickness + ", "
                + "brush=" + rectangle.Stroke + ", "
                + "fill=" + rectangle.Fill;
        }

        public String ParametersToString(Polygon triangle)
        {
            return "triangle: p1=("
                + triangle.Points[0].X + ","
                + triangle.Points[0].Y + "), "
                + "p2=("
                + triangle.Points[1].X + ","
                + triangle.Points[1].Y + "), "
                + "p3=("
                + triangle.Points[2].X + ","
                + triangle.Points[2].Y + "), "
                + "stroke=" + triangle.StrokeThickness + ", "
                + "brush=" + triangle.Stroke + ", "
                + "fill=" + triangle.Fill;
        }
    }
}
