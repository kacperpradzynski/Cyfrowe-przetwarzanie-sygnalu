using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperacjeNaDanych
{
    public class SignalFiltering
    {
        public static List<double> CalculateConvolution(List<double> signal1, List<double> signal2)
        {
            List<double> result = new List<double>();
            double value = 0.0;
            for (int i = 0; i < (signal1.Count + signal2.Count - 1); i++)
            {
                for (int j = i; j >= 0; j--)
                {
                    if (signal1.Count - j > 0 && signal2.Count - (i - j) > 0)
                    {
                        value += signal1[j] * signal2[i - j];
                    }
                }
                result.Add(value);
                value = 0.0;
            }

            return result;
        }

        public static List<double> CalculateCorrelation(List<double> signal1, List<double> signal2)
        {
            List<double> reversed = new List<double>(signal2);
            reversed.Reverse();

            return CalculateConvolution(signal1, reversed);
        }

        public static List<double> CalculateDistance(List<double> signal1, List<double> signal2, out int distancePoints)
        {
            List<double> reversed = new List<double>(signal2);
            reversed.Reverse();
            List<double> result = CalculateConvolution(signal1, reversed);
            List<double> correlation = new List<double>(result);
            int count = correlation.Count / 2;
            correlation.RemoveRange(0, count);
            distancePoints = correlation.IndexOf(correlation.Max());
            return result;
        }

        public static List<double> DelaySignal(List<double> signal, double distancePoints)
        {
            List<double> result = new List<double>(signal);
            for (int i = 0; i < distancePoints; i++)
            {
                result.Add(result[i]);
            }
            result.RemoveRange(0,(int)distancePoints);
            return result;
        }

        public static void ApplyWindow(int windowType, ref List<double> signal)
        {
            double[] window = new double[signal.Count];
            switch (windowType)
            {

                case 1:
                    //Hamming window
                    for (int i = 0; i < window.Length; i++)
                    {
                        window[i] = 0.53836 - (0.46164 * Math.Cos((2 * Math.PI * i) / window.Length));
                    }
                    break;
                case 2:
                    //Hanning window
                    for (int i = 0; i < window.Length; i++)
                    {
                        window[i] = 0.5 - (0.5 * Math.Cos((2 * Math.PI * i) / window.Length));
                    }
                    break;
                case 3:
                    //Blackman window
                    for (int i = 0; i < window.Length; i++)
                    {
                        window[i] = 0.42 - (0.5 * Math.Cos((2 * Math.PI * i) / window.Length)) + (0.08 * Math.Cos((4 * Math.PI * i) / window.Length));
                    }
                    break;
            }
            for (int i = 0; i < signal.Count; i++)
            {
                signal[i] = signal[i] * window[i];
            }
        }

        public static void ChangeFilterType(int filterType, ref List<double> signal)
        {
            switch (filterType)
            {
                case 1:
                    //High-pass filter
                    for (int i = 1; i < signal.Count; i = i + 2)
                    {
                        signal[i] = -1 * signal[i];
                    }
                    break;
                case 2:
                    //Band-pass filter
                    for (int i = 0; i < signal.Count; i++)
                    {
                        signal[i] = signal[i] * 2 * Math.Sin((Math.PI * i) / 2);
                    }
                    break;
            }
        }

        public static List<double> Filter(List<double> signal, int filterMagnitude, int filterFrequencyCoefficient, int windowType, int filterType)
        {
            List<double> result = new List<double>();
            for (int n = 0; n < filterMagnitude; n++)
            {
                if (n == (filterMagnitude - 1) / 2)
                {
                    result.Add(2.0 / filterFrequencyCoefficient);
                }
                else
                {
                    result.Add(Math.Sin((2 * Math.PI * (n - ((filterMagnitude - 1) / 2))) / filterFrequencyCoefficient) / ((n - ((filterMagnitude - 1) / 2)) * Math.PI));
                }
            }
            if (windowType != 0)
            {
                ApplyWindow(windowType, ref result);
            }
            if (filterType != 0)
            {
                ChangeFilterType(filterType, ref result);
            }
            result = CalculateConvolution(signal, result);
            result.RemoveRange(0, filterMagnitude / 2);
            result.RemoveRange(result.Count - (filterMagnitude / 2), filterMagnitude / 2);
            return result;
        }

    }
}
