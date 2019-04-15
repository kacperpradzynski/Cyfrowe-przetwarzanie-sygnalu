using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperacjeNaDanych
{
    public class MeasuresOfSimilarity
    {
        public static double MSE(List<double> originalSignal, List<double> reconstructedSignal)
        {
            double sum = 0;
            for(int i = 0; i < originalSignal.Count; i++)
            {
                sum += (originalSignal[i] - reconstructedSignal[i]) * (originalSignal[i] - reconstructedSignal[i]);
            }
            return sum / originalSignal.Count;
        }

        public static double MD(List<double> originalSignal, List<double> reconstructedSignal)
        {
            double max = 0;
            double tmp = 0;
            for (int i = 0; i < originalSignal.Count; i++)
            {
                tmp = Math.Abs(originalSignal[i] - reconstructedSignal[i]);
                if (tmp > max)
                    max = tmp;
            }
            return max;
        }

        public static double SNR(List<double> originalSignal, List<double> reconstructedSignal)
        {
            double sum = 0;
            for (int i = 0; i < originalSignal.Count; i++)
            {
                sum += originalSignal[i] * originalSignal[i];
            }
            return 10 * Math.Log10(sum / (MSE(originalSignal, reconstructedSignal) * originalSignal.Count));
        }

        public static double PSNR(List<double> originalSignal, List<double> reconstructedSignal)
        {
            return 10 * Math.Log10(originalSignal.Max() / MSE(originalSignal, reconstructedSignal));
        }
    }
}
