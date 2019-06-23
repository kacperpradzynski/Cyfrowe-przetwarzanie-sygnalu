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
    class ComplexChartsW2ViewModel
    {
        public SeriesCollection MagnitudeSeries { get; set; }
        public SeriesCollection PhaseSeries { get; set; }

        public ComplexChartsW2ViewModel(List<Complex> points)
        {
            MagnitudeSeries = new SeriesCollection();
            PhaseSeries = new SeriesCollection();
            ChartValues<double> valuesMagnitude = new ChartValues<double>();
            ChartValues<double> valuesPhase = new ChartValues<double>();

            for (int i = 0; i < points.Count; i++)
            {
                valuesMagnitude.Add(points[i].Magnitude);
                valuesPhase.Add(points[i].Phase);
            }

                MagnitudeSeries.Add(new LineSeries()
                {
                    LineSmoothness = 0,
                    StrokeThickness = 1,
                    Fill = Brushes.Transparent,
                    PointGeometry = new EllipseGeometry(),
                    PointGeometrySize = 5,
                    Values = valuesMagnitude,
                    Title = "Magnitude"
                });
                PhaseSeries.Add(new LineSeries()
                {
                    LineSmoothness = 0,
                    StrokeThickness = 1,
                    Fill = Brushes.Transparent,
                    PointGeometry = new EllipseGeometry(),
                    PointGeometrySize = 5,
                    Values = valuesPhase,
                    Title = "Phase"
                });
        }
    }
}
