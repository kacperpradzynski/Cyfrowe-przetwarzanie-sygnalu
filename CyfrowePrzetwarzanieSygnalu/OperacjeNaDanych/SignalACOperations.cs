using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperacjeNaDanych
{
    public class SignalACOperations
    {
        public static List<double> Sampling(List<double> signal, double space)
        {
            List<double> result = new List<double>();
            for (int i = 0; i < signal.Count; i+=(int)space)
            {
                result.Add(signal[i]);
            }
            return result;
        }
        public static List<double> QuantizationCut(List<double> signal, int compartments)
        {
            List<double> result = new List<double>();
            List<double> compartment = new List<double>(compartments);
            double space = (signal.Max() - signal.Min()) / compartments;
            double value = signal.Min();
            for(int i = 0; i < compartment.Capacity; i++)
            {
                compartment.Add(value);
                value += space;
            }
            compartment.Reverse();
            foreach (double point in signal)
            {
                foreach (double level in compartment)
                {
                    if(point >= level)
                    {
                        result.Add(level);
                        break;
                    }
                }
            }
            return result;
        }
        public static List<double> QuantizationRound(List<double> signal, int compartments)
        {
            List<double> result = new List<double>();
            List<double> compartment = new List<double>(compartments);
            double space = (signal.Max() - signal.Min()) / compartments;
            double value = signal.Min();
            for (int i = 0; i < compartment.Capacity; i++)
            {
                compartment.Add(value);
                value += space;
            }
            compartment.Reverse();
            foreach (double point in signal)
            {
                foreach (double level in compartment)
                {
                    if (point >= level-space/2)
                    {
                        result.Add(level);
                        break;
                    }
                }
            }
            return result;
        }
    }
}
