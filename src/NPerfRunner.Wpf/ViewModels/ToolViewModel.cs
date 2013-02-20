using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPerfRunner.ViewModels;

namespace NPerfRunner.Wpf.ViewModels
{
    using ReactiveUI;

    public class ToolViewModel : PaneViewModel, IToolViewModel
    {
        public ToolViewModel(string name)
        {
            this.Name = name;
            this.Title = name;
        }

        public string Name { get; private set; }

        #region IsVisible

        private bool isVisible;

        public bool IsVisible
        {
            get
            {
                return this.isVisible;
            }

            set
            {
                this.RaiseAndSetIfChanged(x => x.IsVisible, ref this.isVisible, value);
            }
        }

        #endregion // IsVisible
    }
}
