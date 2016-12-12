﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace MMDB.Extensions
{
    public static class ShapeExtensions
    {
        public static void Resize(this Shape shape, Point p1, Point p2)
        {
            if (shape is Line) (shape as Line).Resize(p1, p2);
            if (shape is Ellipse) (shape as Ellipse).Resize(p1, p2);
            if (shape is Rectangle) (shape as Rectangle).Resize(p1, p2);
            if (shape is Polygon) (shape as Polygon).Resize(p1, p2);
        }
        public static Point GetP1(this Shape shape)
        {
            if (shape is Line) return (shape as Line).GetP1();
            if (shape is Ellipse) return (shape as Ellipse).GetP1();
            if (shape is Rectangle) return (shape as Rectangle).GetP1();
            if (shape is Polygon) return (shape as Polygon).GetP1();
            return new Point();
        }

        public static Point GetP2(this Shape shape)
        {
            if (shape is Line) return (shape as Line).GetP2();
            if (shape is Ellipse) return (shape as Ellipse).GetP2();
            if (shape is Rectangle) return (shape as Rectangle).GetP2();
            if (shape is Polygon) return (shape as Polygon).GetP2();
            return new Point();
        }
        public static String ParametersToString(this Shape shape)
        {
            if (shape is Line) return (shape as Line).ParametersToString();
            if (shape is Ellipse) return (shape as Ellipse).ParametersToString();
            if (shape is Rectangle) return (shape as Rectangle).ParametersToString();
            if (shape is Polygon) return (shape as Polygon).ParametersToString();
            return "none";
        }
    }
}