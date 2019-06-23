﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperacjeNaDanych
{
    public class SignalGenerator
    {
        private static Random random = new Random();
        public double Amplitude { get; set; }
        public double StartTime { get; set; }
        public double Period { get; set; }
        public double JumpTime { get; set; }
        public double JumpN { get; set; }
        public double FillFactor { get; set; }
        public double Probability { get; set; }
        public Func<double, double> Func { get; set; }

        public double GenerateSignalForTransformS1(double time)
        {
            return (2 * Math.Sin((2 * Math.PI / 2 * time) + (Math.PI / 2))) + 5*Math.Sin((2 * Math.PI / 0.5 * time) + (Math.PI/2));
        }
        public double GenerateSignalForTransformS2(double time)
        {
            return (2 * Math.Sin(2 * Math.PI / 2 * time)) + Math.Sin(2 * Math.PI / 1 * time) + 5*Math.Sin(2 * Math.PI / 0.5 * time);
        }
        public double GenerateSignalForTransformS3(double time)
        {
            return (5 * Math.Sin(2 * Math.PI / 2 * time)) + Math.Sin(2 * Math.PI / 0.25 * time);
         }
        public double GenerateUniformDistributionNoise(double time = 0)
        {
            return random.NextDouble() * 2 * Amplitude - Amplitude;
        }

        public double GenerateSinusoidalSignal(double time)
        {
            return Amplitude * Math.Sin((2 * Math.PI / Period) * (time - StartTime));
        }

        public double GenerateSinusoidal1PSignal(double time)
        {
            return 0.5 * Amplitude * (Math.Sin((2 * Math.PI / Period) * (time - StartTime)) +
                       Math.Abs(Math.Sin((2 * Math.PI / Period) * (time - StartTime))));
        }

        public double GenerateSinusoidal2PSignal(double time)
        {
            return Amplitude * Math.Abs(Math.Sin((2 * Math.PI / Period) * (time - StartTime)));
        }

        public double GenerateRectangularSignal(double time)
        {
            int k = (int)((time / Period) - (StartTime / Period));
            if (time >= (k * Period + StartTime) && time < (FillFactor * Period + k * Period + StartTime))
                return Amplitude;
            //else if(time >= FillFactor * Period - k * Period + StartTime && time < Period + k * Period + StartTime)

            return 0;
        }

        public double GenerateRectangularSymmetricalSignal(double time)
        {
            int k = (int)((time / Period) - (StartTime / Period));
            if (time >= k * Period + StartTime && time < FillFactor * Period + k * Period + StartTime)
                return Amplitude;

            return -Amplitude;
        }

        public double GenerateTriangularSignal(double time)
        {
            int k = (int)((time / Period) - (StartTime / Period));
            if (time >= k * Period + StartTime && time < FillFactor * Period + k * Period + StartTime)
                return (Amplitude / (FillFactor * Period)) * (time - k * Period - StartTime);

            return -Amplitude / (Period * (1 - FillFactor)) * (time - k * Period - StartTime) + (Amplitude / (1 - FillFactor));
        }

        public double GenerateUnitJump(double time)
        {
            if (time > JumpTime)
                return Amplitude;
            if (time.Equals(JumpTime))
                return 0.5 * Amplitude;
            return 0;
        }

        public double GenerateGaussianNoise(double time = 0)
        {
            //double mean = 2 * Amplitude;
            double stdDev = Amplitude / 3;

            //nuget version - simpler
            //Normal normalDist = new Normal(mean, stdDev);
            //return normalDist.Sample();

            double u1 = 1.0 - random.NextDouble(); //to avoid log(0)=Inf
            double u2 = 1.0 - random.NextDouble();
            double normal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                                   Math.Sin(2.0 * Math.PI * u2);
            return normal * stdDev;
        }

        public double GenerateUnitPulse(double time)
        {
            if (time.Equals(JumpN)) return Amplitude;
            return 0;
        }

        public double GenerateImpulseNoise(double time = 0)
        {
            double temp = random.NextDouble();
            if (Probability > temp)
            {
                return Amplitude;
            }

            return 0;
        }

    }
}
