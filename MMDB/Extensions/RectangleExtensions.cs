using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MMDB.Extensions
{
    public static class RectangleExtensions
    {
        #region Public static methods

        public static double Area(this Rectangle rectangle)
        {
            return rectangle.Height * rectangle.Width;
        }

        public static Rectangle Create(this Rectangle rectangle, Point p1, Point p2, int strokeThickness, Brush strokeBrush, Brush fillBrush)
        {
            rectangle.Height = Math.Abs(p1.Y - p2.Y);
            rectangle.Width = Math.Abs(p1.X - p2.X);
            rectangle.StrokeThickness = strokeThickness;
            rectangle.Stroke = strokeBrush;
            rectangle.Fill = fillBrush;
            double leftMargin = p1.X;
            double topMargin = p1.Y;
            if (p1.X > p2.X)
            {
                leftMargin = p2.X;
            }
            if (p1.Y > p2.Y)
            {
                topMargin = p2.Y;
            }
            rectangle.Margin = new Thickness(leftMargin, topMargin, 0, 0);

            return rectangle;
        }

        public static Point GetP1(this Rectangle rectangle)
        {
            return new Point(rectangle.Margin.Left, rectangle.Margin.Top);
        }

        public static Point GetP2(this Rectangle rectangle)
        {
            return new Point(rectangle.Margin.Left + rectangle.Width, rectangle.Margin.Top + rectangle.Height);
        }

        public static string ParametersToString(this Rectangle rectangle)
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

        public static double Perimeter(this Rectangle rectangle)
        {
            return 2 * (rectangle.Height + rectangle.Width);
        }

        public static void Resize(this Rectangle rectangle, Point p1, Point p2)
        {
            rectangle.Height = Math.Abs(p1.Y - p2.Y);
            rectangle.Width = Math.Abs(p1.X - p2.X);
            double leftMargin = p1.X;
            double topMargin = p1.Y;
            if (p1.X > p2.X)
            {
                leftMargin = p2.X;
            }
            if (p1.Y > p2.Y)
            {
                topMargin = p2.Y;
            }
            rectangle.Margin = new Thickness(leftMargin, topMargin, 0, 0);
        }

        #endregion
    }
}