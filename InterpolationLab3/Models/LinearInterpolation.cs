using System;
using System.Collections.Generic;

namespace InterpolationLab3.Models
{
    public sealed class LinearInterpolation : Interpolation
    {
        public override double Calculate(IReadOnlyList<PointD> points, double x)
        {
            ValidatePoints(points);

            int n = points.Count;

            if (x <= points[0].X)
            {
                return points[0].Y;
            }

            if (x >= points[n - 1].X)
            {
                return points[n - 1].Y;
            }

            int i = FindSegmentIndex(points, x);
            double x1 = points[i].X;
            double y1 = points[i].Y;
            double x2 = points[i + 1].X;
            double y2 = points[i + 1].Y;

            double t = (x - x1) / (x2 - x1);
            return y1 + t * (y2 - y1);
        }
    }
}
