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
        //=====================================================================
        // Create
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

        //=====================================================================
        // Resize
        public Shape ResizeShape(Shape shape, Point p1, Point p2)
        {
            if (shape is Line)
            {
                Line line = shape as Line;
                line.X1 = p1.X;
                line.Y1 = p1.Y;
                line.X2 = p2.X;
                line.Y2 = p2.Y;

                return line;
            }
            if (shape is Ellipse)
            {
                Ellipse ellipse = shape as Ellipse;
                ellipse.Height = Math.Abs(p1.Y - p2.Y);
                ellipse.Width = Math.Abs(p1.X - p2.X);
                double leftMargin = p1.X;
                double topMargin = p1.Y;
                if (p1.X > p2.X) leftMargin = p2.X;
                if (p1.Y > p2.Y) topMargin = p2.Y;
                ellipse.Margin = new Thickness(leftMargin, topMargin, 0, 0);

                return ellipse;
            }
            if(shape is Rectangle)
            {
                Rectangle rectangle = shape as Rectangle;
                rectangle.Height = Math.Abs(p1.Y - p2.Y);
                rectangle.Width = Math.Abs(p1.X - p2.X);
                double leftMargin = p1.X;
                double topMargin = p1.Y;
                if (p1.X > p2.X) leftMargin = p2.X;
                if (p1.Y > p2.Y) topMargin = p2.Y;
                rectangle.Margin = new Thickness(leftMargin, topMargin, 0, 0);

                return rectangle;
            }
            if(shape is Polygon)
            {
                Polygon triangle = shape as Polygon;
                Point t1, t2, t3;
                if (p1.X < p2.X)                         // p1 left
                    if (p1.Y > p2.Y)                     // p1 bottom left
                    {
                        t1 = new Point(p1.X, p1.Y);
                        t2 = new Point(p1.X + (p2.X - p1.X) / 2, p2.Y);
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
            return shape;
        }

        public Point GetP1(Shape shape)
        {
            Point p = new Point();
            if (shape is Line)
            {
                Line line = shape as Line;
                return new Point(line.X1, line.Y1);
            }
            if (shape is Ellipse)
            {
                Ellipse ellipse = shape as Ellipse;
                return new Point(ellipse.Margin.Left, ellipse.Margin.Top);
            }
            if (shape is Rectangle)
            {
                Rectangle rectangle = shape as Rectangle;
                return new Point(rectangle.Margin.Left, rectangle.Margin.Top);
            }
            if (shape is Polygon)
            {
                Polygon triangle = shape as Polygon;
                p = new Point(triangle.Points[0].X, triangle.Points[0].Y);
                for (int i = 1; i < triangle.Points.Count; ++i)
                {
                    p.X = Math.Min(p.X, triangle.Points[i].X);
                    p.Y = Math.Min(p.Y, triangle.Points[i].Y);
                }
                return p;
            }
            return p;
        }

        public Point GetP2(Shape shape)
        {
            Point p = new Point();
            if (shape is Line)
            {
                Line line = shape as Line;
                return new Point(line.X2, line.Y2);
            }
            if (shape is Ellipse)
            {
                Ellipse ellipse = shape as Ellipse;
                return new Point(ellipse.Margin.Left + ellipse.Width, ellipse.Margin.Top + ellipse.Height);
            }
            if (shape is Rectangle)
            {
                Rectangle rectangle = shape as Rectangle;
                return new Point(rectangle.Margin.Left + rectangle.Width, rectangle.Margin.Top + rectangle.Height);
            }
            if (shape is Polygon)
            {
                Polygon triangle = shape as Polygon;
                p = new Point(triangle.Points[0].X, triangle.Points[0].Y);
                for (int i = 1; i < triangle.Points.Count; ++i)
                {
                    p.X = Math.Max(p.X, triangle.Points[i].X);
                    p.Y = Math.Max(p.Y, triangle.Points[i].Y);
                }
                return p;
            }
            return p;
        }
        //=====================================================================
        // ToString
        public String ParametersToString(Shape shape)
        {
            if(shape is Line)
            {
                Line line = shape as Line;
                return "line: p1=(" + (line.X1 + line.Margin.Left) + "," + (line.Y1 + line.Margin.Top)+ "), "
                    + "p2=(" + (line.X2 + line.Margin.Left) + "," + (line.Y2 + line.Margin.Top) + "), "
                    + "stroke=" + line.StrokeThickness + ", "
                    + "brush=" + line.Stroke;
            }
            if (shape is Ellipse)
            {
                Ellipse ellipse = shape as Ellipse;
                return "ellipse: p=("
                    + (ellipse.Margin.Left + ellipse.Width / 2) + ","
                    + (ellipse.Margin.Top + ellipse.Height / 2) + "), "
                    + "width=" + ellipse.Width + ", "
                    + "height=" + ellipse.Height + ", "
                    + "stroke=" + ellipse.StrokeThickness + ", "
                    + "brush=" + ellipse.Stroke + ", "
                    + "fill=" + ellipse.Fill;
            }
            if(shape is Rectangle)
            {
                Rectangle rectangle = shape as Rectangle;
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
            if (shape is Polygon)
            {
                Polygon triangle = shape as Polygon;
                return "triangle: p1=("
                    + (triangle.Points[0].X + triangle.Margin.Left) + ","
                    + (triangle.Points[0].Y + triangle.Margin.Top) + "), "
                    + "p2=("
                    + (triangle.Points[1].X + triangle.Margin.Left) + ","
                    + (triangle.Points[1].Y + triangle.Margin.Top) + "), "
                    + "p3=("
                    + (triangle.Points[2].X + triangle.Margin.Left) + ","
                    + (triangle.Points[2].Y + triangle.Margin.Top) + "), "
                    + "stroke=" + triangle.StrokeThickness + ", "
                    + "brush=" + triangle.Stroke + ", "
                    + "fill=" + triangle.Fill;
            }
            return "none";
        }
    }
}
