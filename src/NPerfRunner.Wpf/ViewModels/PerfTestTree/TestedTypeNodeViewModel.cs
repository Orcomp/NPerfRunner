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
    using NPerfRunner.Wpf.Messages;

    using ReactiveUI;

    public class TestedTypeNodeViewModel : TreeViewItemViewModel
    {
        private readonly PerfLab perfLab;

        private readonly TestNodeViewModel parent;

        public TestInfo TestInfo;

        public TestedTypeNodeViewModel(TestNodeViewModel parent, PerfLab lab, TestInfo testInfo)
            : base(parent)
        {
            this.parent = parent;
            this.perfLab = lab;
            this.TestInfo = testInfo;
            this.Name = testInfo.Suite.TestedType.FullName;
        }
    }
}
