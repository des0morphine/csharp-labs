using System;
using LissajousLab1.Models;
using Xunit;

namespace LissajousLab1.Tests
{
    public class LissajousCalculatorTests
    {
        private LissajousCalculator calculator = new LissajousCalculator();

        [Fact]
        public void TestCircle()
        {
            double ax = 100;
            double ay = 100;
            double fx = 1;
            double fy = 1;
            double phaseX = 0;
            double phaseY = 90;
            double totalTime = 1.0;
            double dt = 0.005;

            var points = calculator.Calculate(ax, ay, fx, fy, phaseX, phaseY, totalTime, dt);

            Assert.NotEmpty(points);
            foreach (var point in points)
            {
                double radius = Math.Sqrt(point.X * point.X + point.Y * point.Y);
                Assert.True(Math.Abs(radius - ax) < 1e-4);
            }
        }

        [Fact]
        public void TestLine()
        {
            double ax = 100;
            double ay = 100;
            double fx = 2;
            double fy = 2;
            double phaseX = 0;
            double phaseY = 0;
            double totalTime = 1.0;
            double dt = 0.005;

            var points = calculator.Calculate(ax, ay, fx, fy, phaseX, phaseY, totalTime, dt);

            Assert.NotEmpty(points);
            foreach (var point in points)
            {
                Assert.True(Math.Abs(point.X - point.Y) < 1e-4);
            }
        }

        [Theory]
        [InlineData(-10, 100, 1, 1, 0.005, 1.0)]
        [InlineData(100, -10, 1, 1, 0.005, 1.0)]
        [InlineData(100, 100, 0, 1, 0.005, 1.0)]
        [InlineData(100, 100, 1, -1, 0.005, 1.0)]
        [InlineData(100, 100, 1, 1, 0, 1.0)]
        [InlineData(100, 100, 1, 1, 0.5, 1.0)]
        public void TestErrors(double ax, double ay, double fx, double fy, double dt, double t)
        {
            Assert.Throws<ArgumentException>(() =>
                calculator.Calculate(ax, ay, fx, fy, 0, 0, t, dt));
        }
    }
}
