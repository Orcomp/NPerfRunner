using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPerfRunner.ViewModels;

namespace NPerfRunner.Wpf.ViewModels
{
    using ReactiveUI;

    public abstract class ToolViewModel : PaneViewModel, IToolViewModel
    {
        protected ToolViewModel(string name)
        {
            this.Name = name;
            this.Title = name;
        }

        public string Name { get; private set; }

        
    }
}
