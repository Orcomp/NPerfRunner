using NPerf.Framework.Interfaces;
using NPerf.Lab;
using NPerfRunner.ViewModels;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPerfRunner.Wpf.ViewModels
{
    public class TestViewModel : TreeViewItemViewModel, ITestViewModel
    {
        public TestViewModel(TesterViewModel parent, PerfLab perfLab, Type testerType, string testName)
            : base(parent)
        {
            this.Name = testName;
            var children = (ReactiveCollection<ITreeViewItemViewModel>)this.Children;
            
            children.AddRange(perfLab.TestSuites
                .Where(x => x.TesterType.Equals(testerType))
                .SelectMany(x => x.Tests).Where(x => x.TestMethodName == testName).Distinct()
                .Select(x => new TestedTypeViewModel(this, perfLab, x) as ITreeViewItemViewModel));
        }

    }
}
