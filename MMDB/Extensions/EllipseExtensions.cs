using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MMDB.Extensions
{
    public static class EllipseExtensions
    {
        #region Public static methods

        public static double Area(this Ellipse ellipse)
        {
            return Math.PI * (ellipse.Height / 2) * (ellipse.Width / 2);
        }

        public static Ellipse Create(this Ellipse ellipse, Point p1, Point p2, int strokeThickness, Brush strokeBrush, Brush fillBrush)
        {
            ellipse.Height = Math.Abs(p1.Y - p2.Y);
            ellipse.Width = Math.Abs(p1.X - p2.X);
            ellipse.StrokeThickness = strokeThickness;
            ellipse.Stroke = strokeBrush;
            ellipse.Fill = fillBrush;
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
            ellipse.Margin = new Thickness(leftMargin, topMargin, 0, 0);

            return ellipse;
        }

        public static Point GetP1(this Ellipse ellipse)
        {
            return new Point(ellipse.Margin.Left, ellipse.Margin.Top);
        }

        public static Point GetP2(this Ellipse ellipse)
        {
            return new Point(ellipse.Margin.Left + ellipse.Width, ellipse.Margin.Top + ellipse.Height);
        }

        public static string ParametersToString(this Ellipse ellipse)
        {
            return "ellipse: p=("
                   + (ellipse.Margin.Left + ellipse.Width / 2) + ","
                   + (ellipse.Margin.Top + ellipse.Height / 2) + "), "
                   + "width=" + ellipse.Width + ", "
                   + "height=" + ellipse.Height + ", "
                   + "stroke=" + ellipse.StrokeThickness + ", "
                   + "brush=" + ellipse.Stroke + ", "
                   + "fill=" + ellipse.Fill;
        }

        public static double Perimeter(this Ellipse ellipse)
        {
            return 2 * Math.PI * Math.Sqrt((Math.Pow(ellipse.Height, 2) + Math.Pow(ellipse.Width, 2)) / 8);
        }

        public static void Resize(this Ellipse ellipse, Point p1, Point p2)
        {
            ellipse.Height = Math.Abs(p1.Y - p2.Y);
            ellipse.Width = Math.Abs(p1.X - p2.X);
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
            ellipse.Margin = new Thickness(leftMargin, topMargin, 0, 0);
        }

        #endregion
    }
}