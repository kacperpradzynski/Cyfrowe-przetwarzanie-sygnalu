using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CyfrowePrzetwarzanieSygnalu
{
    public class MainWindowViewModel : BaseViewModel
    {
        public ObservableCollection<TabViewModel> Tabs { get; set; }
        public TabViewModel SelectedTab { get; set; }
        public ICommand PlotCommand { get; set; }
        public ICommand AddPageCommand { get; set; }
        public MainWindowViewModel()
        {
            Tabs = new ObservableCollection<TabViewModel>() { new TabViewModel("Tab0") };
            SelectedTab = Tabs[0];
            AddPageCommand = new RelayCommand(AddPage);
            PlotCommand = new RelayCommand(OpenSignalWindow);
        }
        public void OpenSignalWindow()
        {
            NewSignal newSignal = new NewSignal(this);
            newSignal.Show();
        }

        public void AddPage()
        {
            Tabs.Add(new TabViewModel("Tab" + Tabs.Count));
        }

    }
}
