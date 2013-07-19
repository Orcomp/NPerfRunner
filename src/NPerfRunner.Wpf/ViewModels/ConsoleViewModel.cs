using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReactiveUI;
using NPerfRunner.ViewModels;

namespace NPerfRunner.Wpf.ViewModels
{
    using System.Windows;

    public class ConsoleViewModel : ToolViewModel, IConsoleViewModel
    {
        public ConsoleViewModel()
            :base("Console output")
        {

        }

        private FrameworkElement view;
        public override FrameworkElement View
        {
            get
            {
                return this.view ?? (this.view = (FrameworkElement)IoC.Instance.Resolve<IViewFor<IConsoleViewModel>>());
            }
        }
    }
}
