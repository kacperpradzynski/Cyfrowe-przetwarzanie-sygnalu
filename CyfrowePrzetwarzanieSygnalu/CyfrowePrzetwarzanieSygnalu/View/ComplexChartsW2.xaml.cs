﻿using System;
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
    /// Logika interakcji dla klasy ComplexChartsW2.xaml
    /// </summary>
    public partial class ComplexChartsW2 : Window
    {
        public ComplexChartsW2(List<Complex> complex)
        {
            DataContext = new ComplexChartsW2ViewModel(complex);
            InitializeComponent();
        }
    }
}
