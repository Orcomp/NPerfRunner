using NPerf.Framework.Interfaces;
using NPerf.Lab;
using NPerfRunner.ViewModels;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPerfRunner.Wpf.Messages;

namespace NPerfRunner.Wpf.ViewModels.PerfTestTree
{
    using NPerf.Lab.Info;

    public class TestNodeViewModel : TreeViewItemViewModel
    {
        public PerfLab PerfLab { get; private set; }

        public Type TesterType { get; private set; }

        public TestInfo[] Tests { get; private set; }

        public TestNodeViewModel(TesterNodeViewModel parent, PerfLab perfLab, Type testerType, IEnumerable<TestInfo> tests)
            : base(parent)
        {
            this.PerfLab = perfLab;
            this.TesterType = testerType;
            this.Tests = tests.ToArray();
            this.Name = this.Tests.First().TestMethodName;
            this.Children.AddRange(from testInfo in Tests
                                   select new TestedTypeNodeViewModel(this, perfLab, testInfo) {IsChecked = false});            
/*             
                perfLab.TestSuites.Where(x => x.TesterType == this.TesterType)
                       .SelectMany(x => x.Tests)
                       .Where(x => x.TestMethodName == Tests.TestMethodName)
                       .Distinct().GroupBy(x => x.TestMethodName)
                       .Select(x => new TestedTypeNodeViewModel(this, perfLab, x) { IsChecked = false }));*/

            MessageBus.Current.Listen<ChartRemoved>().Subscribe(OnChartRemoved);
        }

        private void OnChartRemoved(ChartRemoved chart)
        {
            if (chart.Chart.TestName == this.Name)
            {
                IsChecked = false;
            }         
        }
    }
}
