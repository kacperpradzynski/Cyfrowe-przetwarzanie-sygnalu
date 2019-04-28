using OperacjeNaDanych;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CyfrowePrzetwarzanieSygnalu
{
    public class NewSignalViewModel : BaseViewModel
    {
        private MainWindowViewModel MWViewModel{ get; set; }
        public List<string> SignalTypes { get; set; }
        public string SelectedSignalType { get; set; }
        public ObservableCollection<TabViewModel> Tabs { get; set; }
        public TabViewModel SelectedTab { get; set; }
        public ICommand DrawCommand { get; set; }
        public ICommand AddDrawCommand { get; set; }

        public double A { get; set; }
        public double T1 { get; set; }
        public double Ts { get; set; }
        public double D { get; set; }
        public double T { get; set; }
        public double Kw { get; set; }
        public double F { get; set; }
        public double N { get; set; }
        public double N1 { get; set; }
        public double Ns { get; set; }
        public double P { get; set; }
        public double Fp { get; set; }
        public NewSignalViewModel(MainWindowViewModel MWVM)
        {
            MWViewModel = MWVM;
            Tabs = MWVM.Tabs;
            SelectedTab = MWVM.SelectedTab;
            SignalTypes = new List<string>()
            {
                "(S01) Szum o rozkładzie jednostajnym",
                "(S02) Szum Gaussowski",
                "(S03) Sygnał sinusoidalny",
                "(S04) Sygnał sinusoidalny wyprostowany jednopołówkowo",
                "(S05) Sygnał sinusoidalny wyprostowany dwupołówkowo",
                "(S06) Sygnał prostokątny",
                "(S07) Sygnał prostokątny symetryczny",
                "(S08) Sygnał trójkątny",
                "(S09) Skok jednostkowy",
                "(S10) Impuls jednostkowy",
                "(S11) Szum impulsowy"
            };
            SelectedSignalType = SignalTypes[0];
            DrawCommand = new RelayCommand(Draw);
            AddDrawCommand = new RelayCommand(AddDraw);
        }

        public void Draw()
        {
            MWViewModel.SelectedTab = SelectedTab;
            SignalGenerator generator = new SignalGenerator()
            {
                Amplitude = A,
                FillFactor = Kw,
                Period = T,
                StartTime = T1,
                JumpTime = Ts,
                JumpN = Ns,
                Probability = P
            };
            List<double> pointsX = new List<double>();
            List<double> pointsY = new List<double>();
            List<double> samples = new List<double>();

            Func<double, double> func = null;
            switch (SelectedSignalType.Substring(1, 3))
            {
                case "S01":
                    func = generator.GenerateUniformDistributionNoise;
                    break;
                case "S02":
                    func = generator.GenerateGaussianNoise;
                    break;
                case "S03":
                    func = generator.GenerateSinusoidalSignal;
                    break;
                case "S04":
                    func = generator.GenerateSinusoidal1PSignal;
                    break;
                case "S05":
                    func = generator.GenerateSinusoidal2PSignal;
                    break;
                case "S06":
                    func = generator.GenerateRectangularSignal;
                    break;
                case "S07":
                    func = generator.GenerateRectangularSymmetricalSignal;
                    break;
                case "S08":
                    func = generator.GenerateTriangularSignal;
                    break;
                case "S09":
                    func = generator.GenerateUnitJump;
                    break;
                case "S10":
                    func = generator.GenerateUnitPulse;
                    break;
                case "S11":
                    func = generator.GenerateImpulseNoise;
                    break;

            }


            if (func != null)
            {
                generator.Func = func;
                bool isScattered = false;
                if (func.Method.Name.Contains("GenerateUnitPulse"))
                {
                    isScattered = true;
                    for (double i = N1 * F; i < (D + N1) * F; i++)
                    {
                        pointsX.Add(i / F);
                        pointsY.Add(func(i / F));
                    }
                    MWViewModel.SelectedTab.TabContent.Data.Samples = pointsY;
                    MWViewModel.SelectedTab.TabContent.Data.Frequency = F;
                    MWViewModel.SelectedTab.TabContent.Data.StartTime = N1;
                    MWViewModel.SelectedTab.TabContent.LoadData(pointsX, pointsY, false);
                    MWViewModel.SelectedTab.TabContent.CalculateSignalInfo(N1, N1 + D, true);
                }
                else if (func.Method.Name.Contains("GenerateImpulseNoise"))
                {
                    isScattered = true;
                    for (double i = N1; i < D + N1; i += 1 / F)
                    {
                        pointsX.Add(i);
                        pointsY.Add(func(0));
                    }
                    MWViewModel.SelectedTab.TabContent.Data.Samples = pointsY;
                    MWViewModel.SelectedTab.TabContent.Data.Frequency = F;
                    MWViewModel.SelectedTab.TabContent.Data.StartTime = N1;
                    MWViewModel.SelectedTab.TabContent.LoadData(pointsX, pointsY, false);
                    MWViewModel.SelectedTab.TabContent.CalculateSignalInfo(N1, N1 + D, true);
                }
                else
                {
                    for (double i = T1; i < T1 + D; i += 1 / Fp)
                    {
                        samples.Add(func(i));
                    }
                    for (double i = T1; i < T1 + D; i += D / 5000)
                    {
                        pointsX.Add(i);
                        pointsY.Add(func(i));
                    }

                    MWViewModel.SelectedTab.TabContent.Data.Samples = samples;
                    MWViewModel.SelectedTab.TabContent.Data.Frequency = Fp;
                    MWViewModel.SelectedTab.TabContent.Data.StartTime = T1;
                    MWViewModel.SelectedTab.TabContent.LoadData(pointsX, pointsY, false);
                    MWViewModel.SelectedTab.TabContent.CalculateSignalInfo(T1, T1 + D);
                }

                MWViewModel.SelectedTab.TabContent.IsScattered = isScattered;
                SelectedTab.TabContent.AddOriginal = false;
                SelectedTab.TabContent.AddSamples = false;
                MWViewModel.SelectedTab.TabContent.DrawCharts();
            }
            //TODO
        }
        public void AddDraw()
        {
            MWViewModel.SelectedTab = SelectedTab;
            SignalGenerator generator = new SignalGenerator()
            {
                Amplitude = A,
                FillFactor = Kw,
                Period = T,
                StartTime = T1,
                JumpTime = Ts,
                JumpN = Ns,
                Probability = P
            };
            List<double> pointsX = new List<double>();
            List<double> pointsY = new List<double>();
            List<double> samples = new List<double>();

            Func<double, double> func = null;
            switch (SelectedSignalType.Substring(1, 3))
            {
                case "S01":
                    func = generator.GenerateUniformDistributionNoise;
                    break;
                case "S02":
                    func = generator.GenerateGaussianNoise;
                    break;
                case "S03":
                    func = generator.GenerateSinusoidalSignal;
                    break;
                case "S04":
                    func = generator.GenerateSinusoidal1PSignal;
                    break;
                case "S05":
                    func = generator.GenerateSinusoidal2PSignal;
                    break;
                case "S06":
                    func = generator.GenerateRectangularSignal;
                    break;
                case "S07":
                    func = generator.GenerateRectangularSymmetricalSignal;
                    break;
                case "S08":
                    func = generator.GenerateTriangularSignal;
                    break;
                case "S09":
                    func = generator.GenerateUnitJump;
                    break;
                case "S10":
                    func = generator.GenerateUnitPulse;
                    break;
                case "S11":
                    func = generator.GenerateImpulseNoise;
                    break;

            }


            if (func != null)
            {
                generator.Func = func;
                bool isScattered = false;
                if (func.Method.Name.Contains("GenerateUnitPulse"))
                {
                    isScattered = true;
                    for (double i = N1 * F; i < (D + N1) * F; i++)
                    {
                        pointsX.Add(i / F);
                        pointsY.Add(func(i / F));
                    }
                    MWViewModel.SelectedTab.TabContent.AddedData.Samples = pointsY;
                    MWViewModel.SelectedTab.TabContent.AddedData.Frequency = F;
                    MWViewModel.SelectedTab.TabContent.AddedData.StartTime = N1;
                    MWViewModel.SelectedTab.TabContent.AddLoadData(pointsX, pointsY, false);
                }
                else if (func.Method.Name.Contains("GenerateImpulseNoise"))
                {
                    isScattered = true;
                    for (double i = N1; i < D + N1; i += 1 / F)
                    {
                        pointsX.Add(i);
                        pointsY.Add(func(0));
                    }
                    MWViewModel.SelectedTab.TabContent.AddedData.Samples = pointsY;
                    MWViewModel.SelectedTab.TabContent.AddedData.Frequency = F;
                    MWViewModel.SelectedTab.TabContent.AddedData.StartTime = N1;
                    MWViewModel.SelectedTab.TabContent.AddLoadData(pointsX, pointsY, false);
                }
                else
                {
                    for (double i = T1; i < T1 + D; i += 1 / Fp)
                    {
                        samples.Add(func(i));
                    }
                    for (double i = T1; i < T1 + D; i += D / 5000)
                    {
                        pointsX.Add(i);
                        pointsY.Add(func(i));
                    }

                    MWViewModel.SelectedTab.TabContent.AddedData.Samples = samples;
                    MWViewModel.SelectedTab.TabContent.AddedData.Frequency = Fp;
                    MWViewModel.SelectedTab.TabContent.AddedData.StartTime = T1;
                    MWViewModel.SelectedTab.TabContent.AddLoadData(pointsX, pointsY, false);
                }

                MWViewModel.SelectedTab.TabContent.IsScattered = isScattered;
                SelectedTab.TabContent.AddOriginal = false;
                SelectedTab.TabContent.AddSamples = false;
                MWViewModel.SelectedTab.TabContent.AddDrawCharts();
            }
            //TODO
        }
    }
}
