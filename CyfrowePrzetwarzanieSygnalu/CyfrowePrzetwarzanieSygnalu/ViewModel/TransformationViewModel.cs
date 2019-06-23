using OperacjeNaDanych;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CyfrowePrzetwarzanieSygnalu
{
    class TransformationViewModel : BaseViewModel
    {
        private MainWindowViewModel MWViewModel { get; set; }
        public List<string> Operations { get; set; }
        public string SelectedOperation { get; set; }
        public ObservableCollection<TabViewModel> Tabs { get; set; }
        public List<string> OutputChart { get; set; }
        public TabViewModel SelectedSignal1Tab { get; set; }
        public string SelectedOutputChart { get; set; }
        public ICommand ComputeCommand { get; set; }

        public TransformationViewModel(MainWindowViewModel MWVM)
        {
            MWViewModel = MWVM;
            Tabs = MWVM.Tabs;
            SelectedSignal1Tab = Tabs[0];

            Operations = new List<string>()
            {
                "(F1.1) Transformacja Fouriera",
                "(F1.2) Szybka Transformacja Fouriera",
                "(F1.3) Transformacja Falkowa",
            };

            OutputChart = new List<string>()
            {
                "(W1) Część rzeczywista i urojona amplitudy w dziedzinie częstotliwości",
                "(W2) Moduł liczby zespolonej i argument liczby w funkcji częstotliwości"
            };

            SelectedOperation = Operations[0];
            SelectedOutputChart = OutputChart[0];

            ComputeCommand = new RelayCommand(Compute);
        }

        public void Compute()
        {
            Stopwatch timer = new Stopwatch();
            List<Complex> complex = new List<Complex>();
            timer.Start();
            string chartCase = SelectedOutputChart.Substring(1, 2);
            switch (SelectedOperation.Substring(1, 4))
            {
                case "F1.1":
                    complex = FourierTransform.Transform(SelectedSignal1Tab.TabContent.Data.Samples);
                    break;
                case "F1.2":
                    FastFourierTransform f = new FastFourierTransform();
                    complex = f.Transform(SelectedSignal1Tab.TabContent.Data.Samples);
                    break;
                case "F1.3":
                    complex = WaveletTransform.Transform(SelectedSignal1Tab.TabContent.Data.Samples);
                    break;
            }
            timer.Stop();
            var time = timer.ElapsedMilliseconds;
            if (chartCase.Contains("W1"))
            {
                ComplexChartsW1 window = new ComplexChartsW1(complex);
                window.Show();
                MessageBox.Show("Czas obliczeń: " + time.ToString() + " ms", "Czas", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                ComplexChartsW2 window = new ComplexChartsW2(complex);
                window.Show();
                MessageBox.Show("Czas obliczeń: " + time.ToString() + " ms", "Czas", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

    }
}

