using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CyfrowePrzetwarzanieSygnalu
{
    /// <summary>
    /// Logika interakcji dla klasy ComplexChartsW1.xaml
    /// </summary>
    public partial class ComplexChartsW1 : Window
    {
        public ComplexChartsW1(List<Complex> complex)
        {
            DataContext = new ComplexChartsW1ViewModel(complex);
            InitializeComponent();
        }
    }
}
