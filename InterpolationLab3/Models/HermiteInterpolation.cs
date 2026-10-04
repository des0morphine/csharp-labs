using System;
using System.Collections.Generic;

namespace InterpolationLab3.Models
{
    public sealed class HermiteInterpolation : Interpolation
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

            double dx = x2 - x1;
            double t = (x - x1) / dx;

            double m1 = ComputeDerivative(points, i);
            double m2 = ComputeDerivative(points, i + 1);

            double t2 = t * t;
            double t3 = t2 * t;

            double h00 = 2.0 * t3 - 3.0 * t2 + 1.0;
            double h10 = t3 - 2.0 * t2 + t;
            double h01 = -2.0 * t3 + 3.0 * t2;
            double h11 = t3 - t2;

            return h00 * y1 + h10 * dx * m1 + h01 * y2 + h11 * dx * m2;
        }

        private double ComputeDerivative(IReadOnlyList<PointD> points, int index)
        {
            int n = points.Count;

            if (index == 0)
            {
                return (points[1].Y - points[0].Y) / (points[1].X - points[0].X);
            }

            if (index == n - 1)
            {
                return (points[n - 1].Y - points[n - 2].Y) / (points[n - 1].X - points[n - 2].X);
            }

            return (points[index + 1].Y - points[index - 1].Y) / (points[index + 1].X - points[index - 1].X);
        }
    }
}
