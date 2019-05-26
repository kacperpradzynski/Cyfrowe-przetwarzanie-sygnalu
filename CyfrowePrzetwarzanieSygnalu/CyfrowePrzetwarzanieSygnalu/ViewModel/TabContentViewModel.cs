using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using OperacjeNaDanych;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace CyfrowePrzetwarzanieSygnalu
{
    public class TabContentViewModel : BaseViewModel
    {
        private int _sliderValue;
        public SeriesCollection ChartSeries { get; set; }
        public bool IsScattered { get; set; }
        public bool AddOriginal { get; set; }
        public bool AddSamples { get; set; }
        public SeriesCollection HistogramSeries { get; set; }
        public int HistogramStep { get; set; }
        public string[] Labels { get; set; }

        public double AvgSignal { get; set; }
        public double AbsAvgSignal { get; set; }
        public double AvgSignalPower { get; set; }
        public double SignalVariance { get; set; }
        public double RMSSignal { get; set; }
        public List<double> PointsYOriginal { get; set; }
        public List<double> PointsYSamples { get; set; }
        public int Space { get; set; }

        public ICommand Histogram { get; set; }

        public int SliderValue
        {
            get => _sliderValue;
            set
            {
                _sliderValue = value;
                LoadHistogram(_sliderValue);
            }
        }

        public DataHandler Data { get; set; }
        public DataHandler AddedData { get; set; }

        public TabContentViewModel()
        {
            Data = new DataHandler();
            AddedData = new DataHandler();
            Histogram = new RelayCommand<int>(LoadHistogram);

        }

        public void AddDrawCharts()
        {
            if (AddedData.HasData())
            {
                var mapper = Mappers.Xy<PointXY>()
                    .X(value => value.X)
                    .Y(value => value.Y);
                ChartValues<PointXY> values = new ChartValues<PointXY>();
                ChartValues<PointXY> valuesOriginal = new ChartValues<PointXY>();
                ChartValues<PointXY> valuesSamples = new ChartValues<PointXY>();
                List<double> pointsX;
                List<double> pointsY;
                if (AddedData.FromSamples)
                {
                    pointsX = AddedData.SamplesX;
                    pointsY = AddedData.Samples;
                }
                else
                {
                    pointsX = AddedData.PointsX;
                    pointsY = AddedData.PointsY;
                }
                for (int i = 0; i < pointsX.Count; i++)
                {
                    values.Add(new PointXY(pointsX[i], pointsY[i]));
                }

                    ChartSeries.Add(
                    
                        new LineSeries()
                        {
                            LineSmoothness = 0,
                            StrokeThickness = 0.5,
                            Fill = Brushes.Transparent,
                            PointGeometry = null,
                            Values = values
                        }
                    );
                
            }
        }

        public void DrawCharts()
        {
            if (Data.HasData())
            {
                var mapper = Mappers.Xy<PointXY>()
                    .X(value => value.X)
                    .Y(value => value.Y);
                ChartValues<PointXY> values = new ChartValues<PointXY>();
                ChartValues<PointXY> valuesOriginal = new ChartValues<PointXY>();
                ChartValues<PointXY> valuesSamples = new ChartValues<PointXY>();
                List<double> pointsX;
                List<double> pointsY;
                if (Data.FromSamples)
                {
                    pointsX = Data.SamplesX;
                    pointsY = Data.Samples;
                }
                else
                {
                    pointsX = Data.PointsX;
                    pointsY = Data.PointsY;
                }
                int index = 0;
                for (int i = 0; i < pointsX.Count; i++)
                {
                    values.Add(new PointXY(pointsX[i], pointsY[i]));
                    if(AddOriginal)
                    {
                        valuesOriginal.Add(new PointXY(pointsX[i], PointsYOriginal[i]));
                    }
                    if (AddSamples && i%Space == 0)
                    {
              
                        valuesSamples.Add(new PointXY(pointsX[i], PointsYSamples[index]));
                        index++;
                    }

                }

                if (IsScattered || Data.FromSamples)
                {
                    ChartSeries = new SeriesCollection(mapper)
                    {
                        new LineSeries()
                        {
                            LineSmoothness = 0,
                            StrokeThickness = 0.5,
                            Fill = Brushes.Transparent,
                            PointGeometry = null,
                            Values = values
                        }
                    };
                    if (AddOriginal)
                    {
                        ChartSeries.Add(new LineSeries()
                        {
                            LineSmoothness = 0,
                            StrokeThickness = 0.5,
                            Fill = Brushes.Transparent,
                            PointGeometry = null,
                            Stroke = Brushes.Green,
                            Values = valuesOriginal
                        });
                    }
                    if (AddSamples)
                    {
                        ChartSeries.Add(new ScatterSeries()
                        {
                            PointGeometry = new EllipseGeometry(),
                            StrokeThickness = 8,
                            Stroke = Brushes.Red,
                            Values = valuesSamples
                        });
                    }
                }
                else
                {
                    ChartSeries = new SeriesCollection(mapper)
                    {
                        new LineSeries()
                        {
                            LineSmoothness = 0,
                            StrokeThickness = 0.5,
                            Fill = Brushes.Transparent,
                            PointGeometry = null,
                            Values = values
                        }
                    };
                }


                var histogramResults = Data.GetDataForHistogram(5);
                HistogramStep = 1;
                HistogramSeries = new SeriesCollection
                {
                    new ColumnSeries
                    {
                        Values = new ChartValues<int> (histogramResults.Select(n=>n.Item3)),
                        ColumnPadding = 0

                    }
                };
                Labels = histogramResults.Select(n => n.Item1 + " to " + n.Item2).ToArray();
            }

        }

        public void CalculateSignalInfo(double t1=0, double t2=0, bool isDiscrete = false, bool fromSamples = false)
        {
            List<double> points;
            if (fromSamples)
                points = Data.Samples;
            else
                points = Data.PointsY;
            if(points!=null)
            {
                AvgSignal = SignalOperations.AvgSignal(points, t1, t2, isDiscrete);
                AbsAvgSignal = SignalOperations.AbsAvgSignal(points, t1, t2, isDiscrete);
                AvgSignalPower = SignalOperations.AvgSignalPower(points, t1, t2, isDiscrete);
                SignalVariance = SignalOperations.SignalVariance(points, t1, t2, isDiscrete);
                RMSSignal = SignalOperations.RMSSignal(points, t1, t2, isDiscrete);
            }
        }

        public void LoadHistogram(int c)
        {
            if (Data.HasData())
            {
                var histogramResults = Data.GetDataForHistogram(c);
                HistogramStep = (int)Math.Ceiling(c / 20.0);
                HistogramSeries = new SeriesCollection
                {
                    new ColumnSeries
                    {
                        Values = new ChartValues<int> (histogramResults.Select(n=>n.Item3)),
                        ColumnPadding = 0,

                    }
                };
                Labels = histogramResults.Select(n => n.Item1 + " to " + n.Item2).ToArray();
            }

        }

        public void LoadData(DataHandler data)
        {
            Data = data;
        }

        public void LoadData(List<double> x, List<double> y, bool fromSamples)
        {
            if (fromSamples)
            {
                Data.FromSamples = true;
                Data.Samples = y;
            }
            else
            {
                Data.FromSamples = false;
                Data.PointsX = x;
                Data.PointsY = y;
            }


        }
        public void AddLoadData(List<double> x, List<double> y, bool fromSamples)
        {
            if (fromSamples)
            {
                AddedData.FromSamples = true;
                AddedData.Samples = y;
            }
            else
            {
                AddedData.FromSamples = false;
                AddedData.PointsX = x;
                AddedData.PointsY = y;
            }


        }

        public void SaveDataToFile(string path)
        {
            Data.SaveToFile(path);
        }
        public void LoadDataFromFile(string path)
        {
            Data.FromSamples = true;
            Data.LoadFromFile(path);
        }
    }
}
