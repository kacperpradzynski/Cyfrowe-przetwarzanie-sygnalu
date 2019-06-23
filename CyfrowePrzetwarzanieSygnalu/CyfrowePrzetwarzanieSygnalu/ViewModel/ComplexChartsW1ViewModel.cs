using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CyfrowePrzetwarzanieSygnalu
{
    class ComplexChartsW1ViewModel
    {
        public SeriesCollection RealSeries { get; set; }
        public SeriesCollection ImaginarySeries { get; set; }

        public ComplexChartsW1ViewModel(List<Complex> points)
        {
            RealSeries = new SeriesCollection();
            ImaginarySeries = new SeriesCollection();
            ChartValues<double> valuesReal = new ChartValues<double>();
            ChartValues<double> valuesImaginary = new ChartValues<double>();

            for (int i = 0; i < points.Count; i++)
            {
                valuesReal.Add(points[i].Real);
                valuesImaginary.Add(points[i].Imaginary);
            }

                RealSeries.Add(new LineSeries()
                {
                    LineSmoothness = 0,
                    StrokeThickness = 1,
                    Fill = Brushes.Transparent,
                    PointGeometry = new EllipseGeometry(),
                    PointGeometrySize = 5,
                    Values = valuesReal,
                    Title = "Real"
                });
                ImaginarySeries.Add(new LineSeries()
                {
                    LineSmoothness = 0,
                    StrokeThickness = 1,
                    Fill = Brushes.Transparent,
                    PointGeometry = new EllipseGeometry(),
                    PointGeometrySize = 5,
                    Values = valuesImaginary,
                    Title = "Imaginary"
                });
        }
    }
}
