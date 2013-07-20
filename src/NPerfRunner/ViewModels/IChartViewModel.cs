namespace NPerfRunner.ViewModels
{
    using System;
    using NPerf.Lab.Info;
    using OxyPlot;
    using ReactiveUI.Xaml;

    public interface IChartViewModel : IPaneViewModel
    {
        string TestName { get; }

        Type TesterType { get; }

        void Add(TestInfo testInfo);

        void Remove(TestInfo testInfo);

        PlotModel SpeedPlotModel { get; }

        PlotModel MemoryPlotModel { get; }

        bool IsLinear { get; set; }

        double StartValue { get; }

        double EndValue { get; }

        double StepValue { get; }

        ReactiveAsyncCommand Start { get; }

        ReactiveAsyncCommand Stop { get; }

        bool IsStarted { get; }
    }
}
