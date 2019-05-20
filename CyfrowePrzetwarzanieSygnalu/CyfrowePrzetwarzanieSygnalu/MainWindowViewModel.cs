using Microsoft.Win32;
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
    public class MainWindowViewModel : BaseViewModel
    {
        public ObservableCollection<TabViewModel> Tabs { get; set; }
        public TabViewModel SelectedTab { get; set; }
        public ICommand PlotCommand { get; set; }
        public ICommand AddPageCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand LoadCommand { get; set; }
        public ICommand CalculateCommand { get; set; }
        public ICommand SamplingAndQuantization { get; set; }
        public ICommand Filtering { get; set; }
        public MainWindowViewModel()
        {
            Tabs = new ObservableCollection<TabViewModel>() { new TabViewModel("Sygnał 0") };
            SelectedTab = Tabs[0];
            AddPageCommand = new RelayCommand(AddPage);
            PlotCommand = new RelayCommand(OpenSignalWindow);
            SaveCommand = new RelayCommand(Save);
            LoadCommand = new RelayCommand(Load);
            CalculateCommand = new RelayCommand(OpenCalculateWindow);
            SamplingAndQuantization = new RelayCommand(OpenSamplingAndQuantization);
            Filtering = new RelayCommand(OpenFiltering);
        }
 
        public void OpenSamplingAndQuantization()
        {
            SamplingAndQuantization samplingAndQuantization = new SamplingAndQuantization(this);
            samplingAndQuantization.Show();
        }

        public void OpenFiltering()
        {
            Filtering filtering = new Filtering(this);
            filtering.Show();
        }

        public void OpenSignalWindow()
        {
            NewSignal newSignal = new NewSignal(this);
            newSignal.Show();
        }

        public void OpenCalculateWindow()
        {
            CalculateWindow calculateWindow = new CalculateWindow(this);
            calculateWindow.Show();
        }

        public void AddPage()
        {
            Tabs.Add(new TabViewModel("Sygnał " + Tabs.Count));
        }

        public void Save()
        {
            SelectedTab.TabContent.SaveDataToFile(LoadPath(false));
        }

        public void Load()
        {
            string LoadedPath = LoadPath(true);
            if(LoadedPath != null)
            {
                SelectedTab.TabContent.LoadDataFromFile(LoadedPath);
                SelectedTab.TabContent.AddOriginal = false;
                SelectedTab.TabContent.AddSamples = false;
                SelectedTab.TabContent.DrawCharts();
                SelectedTab.TabContent.CalculateSignalInfo(isDiscrete: true, fromSamples: true);
            }
        }
        public string LoadPath(bool loadMode)
        {
            if (loadMode)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "Bin File(*.bin)| *.bin",
                    RestoreDirectory = true
                };
                openFileDialog.ShowDialog();
                if (openFileDialog.FileName.Length == 0)
                {
                    MessageBox.Show("No files selected", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }

                return openFileDialog.FileName;
            }
            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                Filter = "Bin File(*.bin)| *.bin",
                RestoreDirectory = true
            };
            saveFileDialog.ShowDialog();
            if (saveFileDialog.FileName.Length == 0)
            {
                MessageBox.Show("No files selected","Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }

            return saveFileDialog.FileName;
        }

    }
}
