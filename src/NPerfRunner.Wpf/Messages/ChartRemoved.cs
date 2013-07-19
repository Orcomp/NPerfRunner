using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPerfRunner.Wpf.Messages
{
    using NPerfRunner.ViewModels;

    public class ChartRemoved
    {
        public ChartRemoved(IChartViewModel chart)
        {
            this.Chart = chart;
        }

        public IChartViewModel Chart { get; private set; }
    }
}
