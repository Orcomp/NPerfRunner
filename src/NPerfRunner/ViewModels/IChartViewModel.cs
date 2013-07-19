namespace NPerfRunner.ViewModels
{
    using System;
    using NPerf.Lab.Info;
    using OxyPlot;

    public interface IChartViewModel : IPaneViewModel
    {
        string TestName { get; }

        Type TesterType { get; }

        void Add(TestInfo testInfo);

        void Remove(TestInfo testInfo);

        PlotModel SpeedPlotModel { get; }

        PlotModel MemoryPlotModel { get; }

        EAxisType AxisType { get; }

        double StartValue { get; }

        double EndValue { get; }

        double StepValue { get; }
    }
}
