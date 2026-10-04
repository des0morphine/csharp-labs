using System;
using System.Collections.Generic;

namespace InterpolationLab3.Models
{
    public abstract class Interpolation
    {
        public abstract double Calculate(IReadOnlyList<PointD> points, double x);

        protected void ValidatePoints(IReadOnlyList<PointD> points)
        {
            if (points == null || points.Count < 2)
            {
                throw new ArgumentException("Для інтерполяції потрібно щонайменше 2 точки");
            }

            for (int i = 0; i < points.Count - 1; i++)
            {
                if (points[i + 1].X <= points[i].X)
                {
                    throw new ArgumentException("Точки повинні бути впорядковані за X і не мати однакових значень");
                }
            }
        }

        protected int FindSegmentIndex(IReadOnlyList<PointD> points, double x)
        {
            int n = points.Count;

            if (x <= points[0].X)
            {
                return 0;
            }

            if (x >= points[n - 1].X)
            {
                return n - 2;
            }

            for (int i = 0; i < n - 1; i++)
            {
                if (x >= points[i].X && x <= points[i + 1].X)
                {
                    return i;
                }
            }

            return n - 2;
        }
    }
}
