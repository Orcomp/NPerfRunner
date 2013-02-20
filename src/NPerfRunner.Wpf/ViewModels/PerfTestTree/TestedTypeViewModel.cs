using NPerf.Framework.Interfaces;
using NPerf.Lab;
using NPerf.Lab.Info;
using NPerfRunner.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPerfRunner.Wpf.ViewModels.PerfTestTree
{
    public class TestedTypeViewModel : TreeViewItemViewModel//, ITestedTypeViewModel
    {
        public TestedTypeViewModel(TestViewModel parent, PerfLab lab, TestInfo testInfo)
            : base(parent)
        {
            this.Name = testInfo.Suite.TestedType.FullName;
        }
    }
}
