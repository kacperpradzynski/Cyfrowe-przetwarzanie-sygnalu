using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperacjeNaDanych
{
    public class SignalCAOperations
    {
        public static List<double> Extrapolation(List<double> signal, double samplingFrequency, double signalFrequency, int points)
        {
            List<double> result = new List<double>();
            int space = (int)(signalFrequency / samplingFrequency);
            int n = 0;
            for (int i = 0; i < points; i++)
            {
                result.Add(signal[n]);
                if(i%space == space-1)
                {
                    n++;
                }
            }
            return result;
        }
        public static List<double> Interpolation(List<double> signal, double samplingFrequency, double signalFrequency,int points)
        {
            List<double> result = new List<double>();
            int time = points / (int)signalFrequency;
            int space = (int)(signalFrequency / samplingFrequency);
            double X = 0;
            int compare = 0;
            int n = 0;
            if(signal.Count%2 == 0)
            {
                signal.Add(signal[0]);
                for (int i = 0; i < points; i++)
                {
                    result.Add(LinearInterpolation(0, signal[n], 1 / samplingFrequency, signal[n + 1], X));
                    X += 1 / signalFrequency;
                    compare = (int)(X * signalFrequency);
                    if (compare == space)
                    {
                        X = 0;
                        n++;
                    }
                }
            } else
            {
                for (int j = 0; j < signal.Count - 1; j++)
                {
                    for (int i = 0; i < signalFrequency / samplingFrequency; i++)
                    {
                        result.Add(LinearInterpolation(0, signal[j], 1 / samplingFrequency, signal[j + 1], X));
                        X += 1 / signalFrequency;
                    }
                    X = 0;
                }
                result.Add(signal.ElementAt(signal.Count - 1));
            }
            return result;
        }
        public static List<double> Reconstruction(List<double> signal, double startTime, double samplingFrequency, double signalFrequency, int points, int N)
        {
            double Ts = 1 / samplingFrequency;
            List<double> result = new List<double>();
            int space = (int)(signalFrequency / samplingFrequency);
            double sum = 0.0;
            int n = 0;
            for (int i = 0; i < points; i++)
            {
                double t = startTime + i / signalFrequency;
                for(int j = (i/space)-N+1; j <= (i / space) + N; j++)
                {
                    if(j>=0 && j < signal.Count)
                    {
                        double tmp1 = t / Ts - j;
                        double tmp2 = Sinc(tmp1);
                        sum += signal[j] * tmp2;
                    }
                }
                //foreach (double sam in signal)
                //{
                //    double tmp1 = t / Ts - n;
                //    double tmp2 = Sinc(tmp1);
                //    sum += sam * tmp2;
                //    n++;
                //}
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
