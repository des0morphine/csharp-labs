using System;
using System.Collections.Generic;
using InterpolationLab3.Models;
using Xunit;

namespace InterpolationLab3.Tests
{
    public class InterpolationTests
    {
        [Fact]
        public void TestLinearAtNodes()
        {
            var points = new List<PointD>
            {
                new PointD(0.0, 0.0),
                new PointD(1.0, 2.0),
                new PointD(2.0, 1.0),
                new PointD(3.0, 4.0)
            };

            var interpolation = new LinearInterpolation();

            for (int i = 0; i < points.Count; i++)
            {
                double result = interpolation.Calculate(points, points[i].X);
                Assert.Equal(points[i].Y, result, 5);
            }
        }

        [Fact]
        public void TestHermiteAtNodes()
        {
            var points = new List<PointD>
            {
                new PointD(0.0, 0.0),
                new PointD(1.0, 2.0),
                new PointD(2.0, 1.0),
                new PointD(3.0, 4.0)
            };

            var interpolation = new HermiteInterpolation();

            for (int i = 0; i < points.Count; i++)
            {
                double result = interpolation.Calculate(points, points[i].X);
                Assert.Equal(points[i].Y, result, 5);
            }
        }

        [Fact]
        public void TestLinearMidpoint()
        {
            var points = new List<PointD>
            {
                new PointD(0.0, 0.0),
                new PointD(2.0, 4.0)
            };

            var interpolation = new LinearInterpolation();
            double result = interpolation.Calculate(points, 1.0);

            Assert.Equal(2.0, result, 5);
        }

        [Fact]
        public void TestHermiteAccuracyOnSin()
        {
            var points = new List<PointD>();
            for (double x = 0; x <= 3.14; x += 0.5)
            {
                points.Add(new PointD(x, Math.Sin(x)));
            }

            var linear = new LinearInterpolation();
            var hermite = new HermiteInterpolation();

            double testX = 1.25;
            double actual = Math.Sin(testX);

            double linVal = linear.Calculate(points, testX);
            double herVal = hermite.Calculate(points, testX);

            double linErr = Math.Abs(linVal - actual);
            double herErr = Math.Abs(herVal - actual);

            Assert.True(herErr < linErr);
        }

        [Fact]
        public void TestNonEquidistantPoints()
        {
            var points = new List<PointD>
            {
                new PointD(0.0, 0.0),
                new PointD(0.5, 1.0),
                new PointD(2.0, 0.5),
                new PointD(5.0, 3.0)
            };

            var linear = new LinearInterpolation();
            var hermite = new HermiteInterpolation();

            double linResult = linear.Calculate(points, 1.25);
            double herResult = hermite.Calculate(points, 1.25);

            Assert.False(double.IsNaN(linResult));
            Assert.False(double.IsNaN(herResult));
        }

        [Fact]
        public void TestThrowsOnDuplicateX()
        {
            var points = new List<PointD>
            {
                new PointD(1.0, 2.0),
                new PointD(1.0, 3.0)
            };

            var interpolation = new LinearInterpolation();
            Assert.Throws<ArgumentException>(() => interpolation.Calculate(points, 1.0));
        }

        [Fact]
        public void TestThrowsOnUnorderedPoints()
        {
            var points = new List<PointD>
            {
                new PointD(2.0, 2.0),
                new PointD(1.0, 3.0)
            };

            var interpolation = new HermiteInterpolation();
            Assert.Throws<ArgumentException>(() => interpolation.Calculate(points, 1.5));
        }

        [Fact]
        public void TestThrowsOnTooFewPoints()
        {
            var points = new List<PointD>
            {
                new PointD(1.0, 2.0)
            };

            var interpolation = new LinearInterpolation();
            Assert.Throws<ArgumentException>(() => interpolation.Calculate(points, 1.0));
        }

        [Fact]
        public void TestBoundaryClamp()
        {
            var points = new List<PointD>
            {
                new PointD(1.0, 5.0),
                new PointD(3.0, 10.0)
            };

            var interpolation = new LinearInterpolation();

            double left = interpolation.Calculate(points, 0.0);
            double right = interpolation.Calculate(points, 5.0);

            Assert.Equal(5.0, left);
            Assert.Equal(10.0, right);
        }
    }
}
