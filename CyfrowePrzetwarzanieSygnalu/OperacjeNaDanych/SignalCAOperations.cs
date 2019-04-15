using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperacjeNaDanych
{
    public class SignalCAOperations
    {
        public static List<double> Extrapolation(List<double> signal, double samplingFrequency, double signalFrequency)
        {
            List<double> result = new List<double>();
            foreach(double value in signal)
            {
                for(int i = 0; i < signalFrequency / samplingFrequency; i++)
                {
                    result.Add(value);
                }
            }
            result.RemoveRange(result.Count + 1 - (int)(signalFrequency / samplingFrequency), (int)(signalFrequency / samplingFrequency) - 1);
            return result;
        }
        public static List<double> Interpolation(List<double> signal, double samplingFrequency, double signalFrequency)
        {
            List<double> result = new List<double>();
            int points = ((int)(signalFrequency / samplingFrequency) * signal.Count) - ((int)(signalFrequency / samplingFrequency) - 1 );
            int time = points / (int)signalFrequency;
            double X = 0;
            for (int j = 0; j < signal.Count-1; j++)
            {
                for (int i = 0; i < signalFrequency / samplingFrequency; i++)
                {
                    result.Add(LinearInterpolation(0,signal[j], 1/samplingFrequency ,signal[j+1],X));
                    X += 1 / signalFrequency;
                }
                X = 0;
            }
            result.Add(signal.ElementAt(signal.Count-1));
            return result;
        }
        public static List<double> Reconstruction(List<double> signal, double startTime, double samplingFrequency, double signalFrequency)
        {
            double Ts = 1 / samplingFrequency;
            int N = 3;
            List<double> result = new List<double>();
            double points = ((int)(signalFrequency / samplingFrequency) * signal.Count) - ((int)(signalFrequency / samplingFrequency) - 1);
            double sum = 0.0;
            int n = 0;
            for (int i = 0; i < points; i++)
            {
                double t = startTime + i / signalFrequency;
                foreach (double sam in signal)
                {
                    double tmp1 = t / Ts - n;
                    double tmp2 = Sinc(tmp1);
                    sum += sam * tmp2;
                    n++;
                }
                result.Add(sum);
                sum = 0.0;
                n = 0;
            }
            return result;
        }

        private static double LinearInterpolation(double X1, double Y1, double X2, double Y2, double X)
        {
            double a = (Y2 - Y1) / (X2 - X1);
            double b = ((-X1) * (Y2 - Y1) - (X2 - X1) * (-Y1)) / (X2 - X1);
            return a * X + b;
        }

        private static double Sinc(double t)
        {
            if(t == 0)
            {
                return 1;
            } else
            {
                return (Math.Sin(Math.PI * t) / (Math.PI * t));
            }
        }
    }
}
