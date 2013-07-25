namespace NPerfRunner.Wpf.Messages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using NPerf.Lab;
    using NPerfRunner.Wpf.ViewModels.PerfTestTree;

    public class TestCheckChanged
    {
        public TestCheckChanged(TreeViewItemViewModel threeItem/*PerfLab lab, TestInfo testInfo, bool? isChecked*/)
        {
           /* this.Lab = lab;
            this.TestInfo = testInfo;
            this.Checked = isChecked;*/

            TreeItem = threeItem;
        }

        public TreeViewItemViewModel TreeItem { get; private set; }

        /*public PerfLab Lab { get; private set; }

        public bool? Checked { get; private set; }

        public TestInfo TestInfo { get; private set; }*/
    }
}
