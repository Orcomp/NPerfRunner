using NPerf.Lab;
using NPerfRunner.ViewModels;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPerfRunner.Wpf.ViewModels.PerfTestTree
{
    using NPerf.Core.Info;

    using NPerfRunner.Wpf.Messages;

    public class TesterNodeViewModel : TreeViewItemViewModel
    {
        public TesterNodeViewModel(PerfLab lab, TestSuiteInfo testSuiteInfo)
            : base(null)
        {
            var descr = testSuiteInfo.TestSuiteDescription;
            this.Name = string.IsNullOrEmpty(descr) ? testSuiteInfo.TesterType.FullName : descr;

            this.TestSuiteInfo = testSuiteInfo;

            this.Lab = lab;

            this.Children.AddRange(
                testSuiteInfo.Tests.GroupBy(x => x.TestMethodName)
                             .Select(x => new TestNodeViewModel(this, lab, testSuiteInfo, x.Key)));

            MessageBus.Current.Listen<ChartRemoved>().Subscribe(OnChartRemoved);

            this.IsEnabled = this.Children.Count > 0;
        }

        private void OnChartRemoved(ChartRemoved chart)
        {
            if (chart.Chart.TestSuiteInfo.Equals(this.TestSuiteInfo))
            {
                IsChecked = false;
            }
        }

        public TestSuiteInfo TestSuiteInfo { get; private set; }

        public PerfLab Lab { get; private set; }
    }
}
