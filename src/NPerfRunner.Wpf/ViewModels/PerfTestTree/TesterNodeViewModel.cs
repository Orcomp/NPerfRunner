using NPerf.Lab;
using NPerfRunner.ViewModels;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPerfRunner.Wpf.ViewModels.PerfTestTree
{
    public class TesterNodeViewModel : TreeViewItemViewModel//, ITesterViewModel
    {
        public TesterNodeViewModel(PerfLab lab, Type testerType)
            : base(null)
        {          
            var descr = lab.TestSuites.First(x => x.TesterType == testerType).TestSuiteDescription;
            this.Name = string.IsNullOrEmpty(descr) ? testerType.FullName : descr;

            this.Children.AddRange(lab.TestSuites
                .Where(x => x.TesterType == testerType)
                .SelectMany(x => x.Tests)
                ./*Select(x => x.TestMethodName).*/Distinct()
                .GroupBy(x => x.TestMethodName)
                .Select(x => new TestNodeViewModel(this, lab, testerType, x.ToArray())));
        }     
    }
}
