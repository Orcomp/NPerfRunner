namespace NPerfRunner.Wpf
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using NPerf.Lab;
    using NPerfRunner.Wpf.ViewModels.PerfTestTree;
    using ReactiveUI;

    public class CommonData : ReactiveObject
    {
        public CommonData()
        {
            this.LoadedAssemblies = new ReactiveCollection<Assembly>();   
        }

        #region Lab
        private PerfLab lab;
        public PerfLab Lab
        {
            get
            {
                return this.lab;
            }
            set
            {
                this.RaiseAndSetIfChanged(x => x.Lab, ref this.lab, value);
            }
        }
        #endregion // Lab

        public IReactiveCollection<Assembly> LoadedAssemblies { get; private set; }
    }
}
