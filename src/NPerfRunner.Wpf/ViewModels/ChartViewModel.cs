namespace NPerfRunner.Wpf.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive.Linq;
    using System.Windows;
    using NPerf.Core.PerfTestResults;
    using NPerf.Lab;
    using NPerf.Lab.Info;
    using NPerfRunner.ViewModels;
    using OxyPlot;
    using OxyPlot.Series;
    using ReactiveUI;
    using Views;

    public class ChartViewModel : PaneViewModel, IChartViewModel
    {
        private readonly List<TestInfo> tests = new List<TestInfo>();

        private FrameworkElement view;

        public ChartViewModel(PerfLab lab, Type testerType, string testMethodName)
        {
            TestName = testMethodName;
            TesterType = testerType;           

            SpeedPlotModel =
                new PlotModel(string.Format("\"{0}\": Time characteristics", testMethodName));

            MemoryPlotModel =
                new PlotModel(string.Format("\"{0}\": Memory usage", testMethodName));

            memorySeries = new Dictionary<TestInfo, LineSeries>();
            speedSeries = new Dictionary<TestInfo, LineSeries>();

            MessageBus.Current.Listen<PerfTestResult>()
                      .Where(x => tests.FirstOrDefault(t => t.TestId.Equals(x.TestId)) != null)
                      .Subscribe(
                          res =>
                              {
                                  var nextRes = res as NextResult;
                                  if (nextRes != null)
                                  {
                                      AddPoint(memorySeries, MemoryPlotModel,
                                               tests.First(x => x.TestId.Equals(res.TestId)), nextRes.Descriptor,
                                               nextRes.MemoryUsage);

                                      AddPoint(speedSeries, SpeedPlotModel,
                                               tests.First(x => x.TestId.Equals(res.TestId)), nextRes.Descriptor,
                                               nextRes.Duration);
                                  }
                              });
        }

        private static void AddPoint(IDictionary<TestInfo, LineSeries> dict, PlotModel plotModel, TestInfo test,
                                     double xValue, double yValue)
        {
            if (dict.ContainsKey(test))
            {
                dict[test].Points.Add(new DataPoint(xValue, yValue));
                plotModel.RefreshPlot(true);
            }
        }

        public override FrameworkElement View
        {
            get
            {
                if (view == null)
                {
                    view = new ChartView();
                    ((ChartView) view).BindViewModel(this);
                    view.ObservableForProperty(v => v.IsVisible, false, false)
                        .Select(x => x.Value)
                        .BindTo(this, x => x.IsVisible);
                }

                return view;
            }
        }

        public PlotModel SpeedPlotModel { get; private set; }

        public PlotModel MemoryPlotModel { get; private set; }
        public EAxisType AxisType { get; private set; }
        public double StartValue { get; private set; }
        public double EndValue { get; private set; }
        public double StepValue { get; private set; }

        public string TestName { get; private set; }

        public Type TesterType { get; private set; }

        private IDictionary<TestInfo, LineSeries> memorySeries;
        private IDictionary<TestInfo, LineSeries> speedSeries;

        public void Add(TestInfo test)
        {
            tests.Add(test);
            Add(memorySeries, MemoryPlotModel, test);
            Add(speedSeries, SpeedPlotModel, test);
        }

        public void Remove(TestInfo test)
        {
            tests.Remove(test);
            Remove(memorySeries, MemoryPlotModel, test);
            Remove(speedSeries, SpeedPlotModel, test);
        }

        private static void Add(IDictionary<TestInfo, LineSeries> dict, PlotModel plotModel, TestInfo test)
        {
            var testedTypeName = test.Suite.TestedType.ToString();

            if (!dict.ContainsKey(test))
            {
                var series = new LineSeries(testedTypeName);
                dict.Add(test, series);
                plotModel.Series.Add(series);
                plotModel.RefreshPlot(true);
            }
        }

        private static void Remove(IDictionary<TestInfo, LineSeries> dict, PlotModel plotModel, TestInfo test)
        {
            if (dict.ContainsKey(test))
            {
                plotModel.Series.Remove(dict[test]);
                dict.Remove(test);
            }
        }
    }
}