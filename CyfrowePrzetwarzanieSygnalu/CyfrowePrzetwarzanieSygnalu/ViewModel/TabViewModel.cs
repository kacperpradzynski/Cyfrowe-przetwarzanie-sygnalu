﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyfrowePrzetwarzanieSygnalu
{
    public class TabViewModel : BaseViewModel
    {
        public string Header { get; set; }
        public TabContentViewModel TabContent { get; set; }

        public TabViewModel(string header)
        {
            Header = header;
            TabContent = new TabContentViewModel();
        }

        public override string ToString()
        {
            return Header;
        }
    }
}
