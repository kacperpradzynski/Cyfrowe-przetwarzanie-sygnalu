using OperacjeNaDanych;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CyfrowePrzetwarzanieSygnalu
{
    class FilteringViewModel : BaseViewModel
    {
        private MainWindowViewModel MWViewModel { get; set; }
        public List<string> Operations { get; set; }
        public string SelectedOperation { get; set; }
        public List<string> Filters { get; set; }
        public string SelectedFilter { get; set; }
        public List<string> Windows { get; set; }
        public string SelectedWindow { get; set; }
        public double t { get; set; }
        public double v { get; set; }
        public int M { get; set; }
        public int K { get; set; }
        public ObservableCollection<TabViewModel> Tabs { get; set; }
        public TabViewModel SelectedSignal1Tab { get; set; }
        public TabViewModel SelectedSignal2Tab { get; set; }
        public TabViewModel SelectedResultTab { get; set; }
        public ICommand ComputeCommand { get; set; }

        public FilteringViewModel(MainWindowViewModel MWVM)
        {
            MWViewModel = MWVM;
            Tabs = MWVM.Tabs;
            SelectedSignal1Tab = Tabs[0];
            SelectedSignal2Tab = Tabs[0];
            SelectedResultTab = Tabs[0];

            Operations = new List<string>()
            {
                "(O1) Splot",
                "(O2) Filtr",
                "(O3) Korelacja",
                "(O4) Radar - Pomiar odległości"
            };
            SelectedOperation = Operations[0];

            Filters = new List<string>()
            {
                "0. Dolnoprzepsutowy",
                "1. Górnoprzepustowy",
                "2. Środkowoprzepustowy"
            };
            SelectedFilter = Filters[0];

            Windows = new List<string>()
            {
                "0. Brak okna",
                "1. Okno Hamminga",
                "2. Okno Hanninga",
                "3. Okno Blackmana"
            };
            SelectedWindow = Windows[0];

            ComputeCommand = new RelayCommand(Compute);
        }

        public void Compute()
        {
            int distancePoints = 0;
            double distance = 0;
            bool radar = false;
            SelectedSignal1Tab.TabContent.Data.FromSamples = true;
            SelectedSignal2Tab.TabContent.Data.FromSamples = true;
            if (SelectedSignal1Tab.TabContent.Data.HasData() && SelectedSignal2Tab.TabContent.Data.HasData())
            {
                DataHandler data = new DataHandler();
                List<double> pointsY = new List<double>();

                switch (SelectedOperation.Substring(1, 2))
                {
                    case "O1":
                        if (!SelectedSignal2Tab.TabContent.Data.IsValidConvulution(SelectedSignal1Tab.TabContent.Data))
                        {
                            MessageBox.Show("Given signals are not valid", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        pointsY = SignalFiltering.CalculateConvolution(SelectedSignal1Tab.TabContent.Data.Samples,
                            SelectedSignal2Tab.TabContent.Data.Samples);
                        break;
                    case "O2":
                        int windowType = int.Parse(SelectedWindow.Substring(0, 1));
                        int filterType = int.Parse(SelectedFilter.Substring(0, 1));
                        pointsY = SignalFiltering.Filter(SelectedSignal1Tab.TabContent.Data.Samples, M, K, windowType, filterType);
                        break;
                    case "O3":
                        if (!SelectedSignal2Tab.TabContent.Data.IsValidConvulution(SelectedSignal1Tab.TabContent.Data))
                        {
                            MessageBox.Show("Given signals are not valid", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        pointsY = SignalFiltering.CalculateCorrelation(SelectedSignal1Tab.TabContent.Data.Samples,
                           SelectedSignal2Tab.TabContent.Data.Samples);
                        break;
                    case "O4":
                        pointsY = SignalFiltering.CalculateDistance(SelectedSignal1Tab.TabContent.Data.Samples,
                            SignalFiltering.DelaySignal(SelectedSignal1Tab.TabContent.Data.Samples, t * SelectedSignal1Tab.TabContent.Data.Frequency), out distancePoints);
                        distance = distancePoints * v / SelectedSignal1Tab.TabContent.Data.Frequency;
                        radar = true;
                        break;
                }

                data.StartTime = SelectedSignal1Tab.TabContent.Data.StartTime;
                data.Frequency = SelectedSignal1Tab.TabContent.Data.Frequency;
                data.Samples = pointsY;
                data.FromSamples = true;
                SelectedResultTab.TabContent.IsScattered = true;
                SelectedResultTab.TabContent.LoadData(data);
                SelectedResultTab.TabContent.AddOriginal = false;
                SelectedResultTab.TabContent.AddSamples = false;
                SelectedResultTab.TabContent.DrawCharts();
                SelectedResultTab.TabContent.CalculateSignalInfo(isDiscrete: true, fromSamples: true);
                if(radar)
                    MessageBox.Show("Odległość: " + distance.ToString() + " m", "Radar", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

    }
}

