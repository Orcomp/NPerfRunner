namespace NPerfRunner.ViewModels
{
    using System;
    using System.Collections.Generic;

    using NPerf.Core.Info;
    using OxyPlot;
    using ReactiveUI.Xaml;

    public interface IChartViewModel : IPaneViewModel
    {
        TestSuiteInfo TestSuiteInfo { get; }

        void Add(TestInfo testInfo);

        void Remove(TestInfo testInfo);

        PlotModel SpeedPlotModel { get; }

        PlotModel MemoryPlotModel { get; }

        bool IsLinear { get; set; }

        int StartValue { get; }

        int EndValue { get; }

        int StepValue { get; }

        string FeatureDescription { get; }

        double StartDescriptor { get; }

        double EndDescriptor { get; }

        double StepDescriptor { get; }

        ReactiveAsyncCommand StartSequential { get; }

        ReactiveAsyncCommand StartParallel { get; }

        ReactiveAsyncCommand Stop { get; }

        bool IsStarted { get; }

        IEnumerable<TestInfo> Tests { get; }
    }
}
