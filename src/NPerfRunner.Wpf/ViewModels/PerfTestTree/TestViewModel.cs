using NPerf.Framework.Interfaces;
using NPerf.Lab;
using NPerfRunner.ViewModels;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPerfRunner.Wpf.ViewModels.PerfTestTree
{
    public class TestViewModel : TreeViewItemViewModel//, ITestViewModel
    {
        public TestViewModel(TesterViewModel parent, PerfLab perfLab, Type testerType, string testName)
            : base(parent)
        {
            this.Name = testName;
            this.Children.AddRange(perfLab.TestSuites
                .Where(x => x.TesterType == testerType)
                .SelectMany(x => x.Tests).Where(x => x.TestMethodName == testName).Distinct()
                .Select(x => new TestedTypeViewModel(this, perfLab, x)));
        }

    }
}
