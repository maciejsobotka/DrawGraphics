using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MMDB.Extensions
{
    public static class TriangleExtensions
    {
        #region Public static methods

        public static Polygon Create(this Polygon triangle, Point p1, Point p2, int strokeThickness, Brush strokeBrush, Brush fillBrush)
        {
            Point t1, t2, t3;
            triangle.StrokeThickness = strokeThickness;
            triangle.Stroke = strokeBrush;
            triangle.Fill = fillBrush;
            if (p1.X < p2.X) // p1 left
            {
                if (p1.Y > p2.Y) // p1 bottom left
                {
                    t1 = new Point(p1.X, p1.Y);
                    t2 = new Point(p1.X + (p2.X - p1.X) / 2, p2.Y);
                    t3 = new Point(p2.X, p1.Y);
                }
                else
                {
                    // p1 top left
                    t1 = new Point(p1.X, p2.Y);
                    t2 = new Point(p1.X + (p2.X - p1.X) / 2, p1.Y);
                    t3 = new Point(p2.X, p2.Y);
                }
            }
            else // p2 left
            if (p1.Y > p2.Y) // p2 top left      
            {
                t1 = new Point(p2.X, p1.Y);
                t2 = new Point(p2.X + (p1.X - p2.X) / 2, p2.Y);
                t3 = new Point(p1.X, p1.Y);
            }
            else
            {
                // p2 bottom left
                t1 = new Point(p2.X, p2.Y);
                t2 = new Point(p2.X + (p1.X - p2.X) / 2, p1.Y);
                t3 = new Point(p1.X, p2.Y);
            }

            var trianglePoints = new PointCollection();
            trianglePoints.Add(t1);
            trianglePoints.Add(t2);
            trianglePoints.Add(t3);
            triangle.Points = trianglePoints;

            return triangle;
        }

        public static Point GetP1(this Polygon triangle)
        {
            var p = new Point(triangle.Points[0].X, triangle.Points[0].Y);
            for (var i = 1; i < triangle.Points.Count; ++i)
            {
                p.X = Math.Min(p.X, triangle.Points[i].X);
                p.Y = Math.Min(p.Y, triangle.Points[i].Y);
            }
            return p;
        }

        public static Point GetP2(this Polygon triangle)
        {
            var p = new Point(triangle.Points[0].X, triangle.Points[0].Y);
            for (var i = 1; i < triangle.Points.Count; ++i)
            {
                p.X = Math.Max(p.X, triangle.Points[i].X);
                p.Y = Math.Max(p.Y, triangle.Points[i].Y);
            }
            return p;
        }

        //=====================================================================
        // ToString
        public static string ParametersToString(this Polygon triangle)
        {
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

        public static void Resize(this Polygon triangle, Point p1, Point p2)
        {
            Point t1, t2, t3;
            if (p1.X < p2.X) // p1 left
            {
                if (p1.Y > p2.Y) // p1 bottom left
                {
                    t1 = new Point(p1.X, p1.Y);
                    t2 = new Point(p1.X + (p2.X - p1.X) / 2, p2.Y);
                    t3 = new Point(p2.X, p1.Y);
                }
                else
                {
                    // p1 top left
                    t1 = new Point(p1.X, p2.Y);
                    t2 = new Point(p1.X + (p2.X - p1.X) / 2, p1.Y);
                    t3 = new Point(p2.X, p2.Y);
                }
            }
            else // p2 left
            if (p1.Y > p2.Y) // p2 top left      
            {
                t1 = new Point(p2.X, p1.Y);
                t2 = new Point(p2.X + (p1.X - p2.X) / 2, p2.Y);
                t3 = new Point(p1.X, p1.Y);
            }
            else
            {
                // p2 bottom left
                t1 = new Point(p2.X, p2.Y);
                t2 = new Point(p2.X + (p1.X - p2.X) / 2, p1.Y);
                t3 = new Point(p1.X, p2.Y);
            }

            var trianglePoints = new PointCollection();
            trianglePoints.Add(t1);
            trianglePoints.Add(t2);
            trianglePoints.Add(t3);
            triangle.Points = trianglePoints;
        }

        #endregion
    }
}