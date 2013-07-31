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
    using NPerf.Core.Info;

    public class TestNodeViewModel : TreeViewItemViewModel
    {
        public PerfLab PerfLab { get; private set; }

        public TestInfo[] Tests { get; private set; }

        public TestSuiteInfo TestSuite { get; private set; }

        public TestNodeViewModel(TesterNodeViewModel parent, PerfLab perfLab, TestSuiteInfo testSuite, string testMethodName)
            : base(parent)
        {
            this.PerfLab = perfLab;
            this.TestSuite = testSuite;
            this.Tests = testSuite.Tests.Where(x => x.TestMethodName == testMethodName).ToArray();
            this.Name = testMethodName;
            this.Children.AddRange(from testInfo in this.Tests
                                   select new TestedTypeNodeViewModel(this, perfLab, testInfo) {IsChecked = false});
            this.IsEnabled = this.Children.Count > 0;
        }        
    }
}
