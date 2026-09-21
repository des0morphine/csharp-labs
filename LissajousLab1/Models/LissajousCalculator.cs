using System;
using System.Collections.Generic;

namespace LissajousLab1.Models
{
    public class LissajousCalculator
    {
        public List<Point2D> Calculate(
            double ax,
            double ay,
            double fx,
            double fy,
            double phaseX,
            double phaseY,
            double totalTime,
            double dt)
        {
            if (ax <= 0 || ay <= 0)
            {
                throw new ArgumentException("Амплітуди мають бути більшими за нуль.");
            }

            if (fx <= 0 || fy <= 0)
            {
                throw new ArgumentException("Частоти мають бути більшими за нуль.");
            }

            if (dt <= 0)
            {
                throw new ArgumentException("Крок часу має бути більшим за нуль.");
            }

            if (totalTime <= 0)
            {
                throw new ArgumentException("Тривалість має бути більшою за нуль.");
            }

            int kolvo = (int)(totalTime / dt);
            if (kolvo < 20)
            {
                throw new ArgumentException("Кількість точок має бути щонайменше 20.");
            }

            double radX = phaseX * Math.PI / 180.0;
            double radY = phaseY * Math.PI / 180.0;

            List<Point2D> tochki = new List<Point2D>();

            for (double t = 0; t <= totalTime + dt / 2; t += dt)
            {
                double x = ax * Math.Sin(2 * Math.PI * fx * t + radX);
                double y = ay * Math.Sin(2 * Math.PI * fy * t + radY);
                tochki.Add(new Point2D(x, y));
            }

            return tochki;
        }
    }
}
