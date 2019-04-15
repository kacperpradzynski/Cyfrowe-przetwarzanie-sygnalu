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
    class SamplingAndQuantizationViewModel
    {
        private MainWindowViewModel MWViewModel { get; set; }
        public List<string> SignalOperation { get; set; }
        public string SelectedSignalOperation { get; set; }
        public List<string> SignalReconstrutions { get; set; }
        public string SelectedSignalReconstrution { get; set; }
        public ObservableCollection<TabViewModel> Tabs { get; set; }
        public TabViewModel SelectedTab { get; set; }
        public TabViewModel SelectedResultTab { get; set; }
        public ICommand SAQCommand { get; set; }
        public double F { get; set; }
        public int N { get; set; }
        public SamplingAndQuantizationViewModel(MainWindowViewModel MWVM)
        {
            MWViewModel = MWVM;
            Tabs = MWVM.Tabs;
            SelectedTab = MWVM.SelectedTab;
            SelectedResultTab = MWVM.SelectedTab;
            SignalOperation = new List<string>()
            {
                "(S1) Próbkowanie równomierne",
                "(Q1) Kwantyzacja równomierna z obcięciem",
                "(Q2) Kwantyzacja równomierna z zaokrągleniem",
            };
            SignalReconstrutions = new List<string>()
            {
                "(R1) Ekstrapolacja zerowego rzędu",
                "(R2) Interpolacja pierwszego rzędu",
                "(R3) Rekonstrukcja w oparciu o funkcję sinc",
            };
            SelectedSignalOperation = SignalOperation[0];
            SelectedSignalReconstrution = SignalReconstrutions[0];
            SAQCommand = new RelayCommand(SAQ);
        }

        public void SAQ()
        {
            SelectedTab.TabContent.Data.FromSamples = true;
            if (SelectedTab.TabContent.Data.HasData())
            {
                if (SelectedTab.TabContent.Data.Frequency % F != 0 && SelectedSignalOperation.Substring(1, 2).Contains("S1"))
                {
                    MessageBox.Show("Given frequency is not valid", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                DataHandler data = new DataHandler();
                List<double> pointsY = new List<double>();
                List<double> pointsYSamples = new List<double>();
                switch (SelectedSignalOperation.Substring(1, 2))
                {
                    case "S1":
                        pointsY = SignalACOperations.Sampling(SelectedTab.TabContent.Data.Samples, SelectedTab.TabContent.Data.Frequency/F);
                        pointsYSamples = pointsY;
                        SelectedResultTab.TabContent.AddSamples = true;
                        SelectedResultTab.TabContent.Space = (int)(SelectedTab.TabContent.Data.Frequency / F);
                        SelectedResultTab.TabContent.PointsYSamples = pointsYSamples;
                        switch (SelectedSignalReconstrution.Substring(1, 2))
                        {
                            case "R1":
                                pointsY = SignalCAOperations.Extrapolation(pointsY, F, SelectedTab.TabContent.Data.Frequency);
                                break;
                            case "R2":
                                pointsY = SignalCAOperations.Interpolation(pointsY, F, SelectedTab.TabContent.Data.Frequency);
                                break;
                            case "R3":
                                pointsY = SignalCAOperations.Reconstruction(pointsY, SelectedTab.TabContent.Data.StartTime, F, SelectedTab.TabContent.Data.Frequency);
                                break;
                        }
                        break;
                    case "Q1":
                        pointsY = SignalACOperations.QuantizationCut(SelectedTab.TabContent.Data.Samples, N);
                        SelectedResultTab.TabContent.AddSamples = false;
                        break;
                    case "Q2":
                        pointsY = SignalACOperations.QuantizationRound(SelectedTab.TabContent.Data.Samples, N);
                        SelectedResultTab.TabContent.AddSamples = false;
                        break;
                }

                data.StartTime = SelectedTab.TabContent.Data.StartTime;
                data.Frequency = SelectedTab.TabContent.Data.Frequency;
                data.Samples = pointsY;
                data.FromSamples = true;
                SelectedResultTab.TabContent.IsScattered = true;
                SelectedResultTab.TabContent.LoadData(data);
                SelectedResultTab.TabContent.AddOriginal = true;
                SelectedResultTab.TabContent.PointsYOriginal = SelectedTab.TabContent.Data.Samples;
                SelectedResultTab.TabContent.DrawCharts();
                SelectedResultTab.TabContent.CalculateSignalInfo(isDiscrete: true, fromSamples: true);
                double MSE = MeasuresOfSimilarity.MSE(SelectedTab.TabContent.Data.Samples, pointsY);
                double SNR = MeasuresOfSimilarity.SNR(SelectedTab.TabContent.Data.Samples, pointsY);
                double PSNR = MeasuresOfSimilarity.PSNR(SelectedTab.TabContent.Data.Samples, pointsY);
                double MD = MeasuresOfSimilarity.MD(SelectedTab.TabContent.Data.Samples, pointsY);
                MessageBox.Show("MSE: " + MSE + Environment.NewLine + "SNR: " + SNR + "dB" + Environment.NewLine + "PSNR: " + PSNR + "dB" + Environment.NewLine + "MD: " + MD, "Miary podobieństwa sygnałów", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
