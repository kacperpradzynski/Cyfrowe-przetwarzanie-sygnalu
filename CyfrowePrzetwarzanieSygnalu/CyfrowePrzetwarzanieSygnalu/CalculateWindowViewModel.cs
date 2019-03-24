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
    class CalculateWindowViewModel : BaseViewModel
    {
        private MainWindowViewModel MWViewModel { get; set; }
        public List<string> Operations { get; set; }
        public string SelectedOperation { get; set; }
        public ObservableCollection<TabViewModel> Tabs { get; set; }
        public TabViewModel SelectedSignal1Tab { get; set; }
        public TabViewModel SelectedSignal2Tab { get; set; }
        public TabViewModel SelectedResultTab { get; set; }
        public ICommand ComputeCommand { get; set; }

        public CalculateWindowViewModel(MainWindowViewModel MWVM)
        {
            MWViewModel = MWVM;
            Tabs = MWVM.Tabs;
            SelectedSignal1Tab = Tabs[0];
            SelectedSignal2Tab = Tabs[0];
            SelectedResultTab = Tabs[0];

            Operations = new List<string>()
            {
                "(D1) Dodawanie",
                "(D2) Odejmowanie",
                "(D3) Mnożenie",
                "(D4) Dzielenie",
            };
            SelectedOperation = Operations[0];
            ComputeCommand = new RelayCommand(Compute);
        }

        public void Compute()
        {
            SelectedSignal1Tab.TabContent.Data.FromSamples = true;
            SelectedSignal2Tab.TabContent.Data.FromSamples = true;
            if (SelectedSignal1Tab.TabContent.Data.HasData() && SelectedSignal2Tab.TabContent.Data.HasData())
            {
                if (!SelectedSignal2Tab.TabContent.Data.IsValid(SelectedSignal1Tab.TabContent.Data))
                {
                    MessageBox.Show("Given signals are not valid", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                DataHandler data = new DataHandler();
                List<double> pointsY = new List<double>();

                switch (SelectedOperation.Substring(1, 2))
                {
                    case "D1":
                        pointsY = SignalOperations.AddSignals(SelectedSignal1Tab.TabContent.Data.Samples,
                            SelectedSignal2Tab.TabContent.Data.Samples);
                        break;
                    case "D2":
                        pointsY = SignalOperations.SubtractSignals(SelectedSignal1Tab.TabContent.Data.Samples,
                            SelectedSignal2Tab.TabContent.Data.Samples);
                        break;
                    case "D3":
                        pointsY = SignalOperations.MultiplySignals(SelectedSignal1Tab.TabContent.Data.Samples,
                            SelectedSignal2Tab.TabContent.Data.Samples);
                        break;
                    case "D4":
                        pointsY = SignalOperations.DivideSignals(SelectedSignal1Tab.TabContent.Data.Samples,
                            SelectedSignal2Tab.TabContent.Data.Samples);
                        break;
                }

                data.StartTime = SelectedSignal1Tab.TabContent.Data.StartTime;
                data.Frequency = SelectedSignal1Tab.TabContent.Data.Frequency;
                data.Samples = pointsY;
                data.FromSamples = true;
                SelectedResultTab.TabContent.IsScattered = true;
                SelectedResultTab.TabContent.LoadData(data);
                SelectedResultTab.TabContent.DrawCharts();
                SelectedResultTab.TabContent.CalculateSignalInfo(isDiscrete: true, fromSamples: true);
            }

        }

    }
}
