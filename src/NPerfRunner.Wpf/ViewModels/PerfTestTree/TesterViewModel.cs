using NPerf.Lab;
using NPerfRunner.ViewModels;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPerfRunner.Wpf.ViewModels.PerfTestTree
{
    public class TesterViewModel : TreeViewItemViewModel//, ITesterViewModel
    {
        public TesterViewModel(PerfLab lab, Type testerType)
            : base(null)
        {          
            var descr = lab.TestSuites.First(x => x.TesterType == testerType).TestSuiteDescription;
            this.Name = string.IsNullOrEmpty(descr) ? testerType.FullName : descr;

            this.Children.AddRange(lab.TestSuites
                .Where(x => x.TesterType == testerType)
                .SelectMany(x => x.Tests)
                .Select(x => x.TestMethodName).Distinct()
                .OrderBy(x => x)
                .Select(x => new TestViewModel(this, lab, testerType, x)));
        }     
    }
}
