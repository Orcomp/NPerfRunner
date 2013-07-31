namespace NPerfRunner.Wpf.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using NPerf.Core.Info;
    using NPerf.Core.PerfTestResults;
    using NPerf.Lab;
    using NPerfRunner.ViewModels;
    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;
    using ReactiveUI;
    using ReactiveUI.Xaml;
    using Views;
    using Fasterflect;

    public class ChartViewModel : PaneViewModel, IChartViewModel
    {
        private readonly List<TestInfo> tests = new List<TestInfo>();

        private FrameworkElement view;

        public ChartViewModel(PerfLab lab, TestSuiteInfo suiteInfo)
        {
            this.Title = suiteInfo.TestSuiteDescription;

            Lab = lab;

            TestSuiteInfo = suiteInfo;

            this.StartValue = 1;

            this.EndValue = suiteInfo.DefaultTestCount;

            this.StepValue = 1;

            SpeedPlotModel =
                new PlotModel(string.Format("\"{0}\": Time characteristics", suiteInfo.TestSuiteDescription));

            SpeedPlotModel.Axes.Clear();
            SpeedPlotModel.Axes.Add(new LinearAxis(AxisPosition.Bottom, suiteInfo.FeatureDescription));

            MemoryPlotModel =
                new PlotModel(string.Format("\"{0}\": Memory usage", suiteInfo.TestSuiteDescription));

            MemoryPlotModel.Axes.Clear();
            MemoryPlotModel.Axes.Add(new LinearAxis(AxisPosition.Bottom, suiteInfo.FeatureDescription));

            memorySeries = new Dictionary<TestInfo, LineSeries>();
            speedSeries = new Dictionary<TestInfo, LineSeries>();

            IsLinear = true;
            IsStarted = false;

            var errorHandler = IoC.Instance.Resolve<ErrorHandler>();

            var whenStarted = this.WhenAny(x => x.IsStarted, x => x.Value);

            this.StartSequential = new ReactiveAsyncCommand(whenStarted.Select(x => !x));
            StartSequential.RegisterAsyncAction(OnStartSequential, RxApp.DeferredScheduler);
            errorHandler.HandleErrors(this.StartSequential);

            this.StartParallel = new ReactiveAsyncCommand(whenStarted.Select(x => !x));
            StartParallel.RegisterAsyncAction(OnStartParallel, RxApp.DeferredScheduler);
            errorHandler.HandleErrors(this.StartParallel);

            this.Stop = new ReactiveAsyncCommand(whenStarted);
            Stop.RegisterAsyncAction(OnStop, RxApp.DeferredScheduler);
            errorHandler.HandleErrors(this.Stop);

            this.WhenAny(x => x.IsLinear, x => x.Value ? EAxisType.Linear : EAxisType.Logarithmic)
                .Subscribe(SetAxisType);

            MessageBus.Current.Listen<PerfTestResult>()
                      .Where(x => tests.FirstOrDefault(t => t.TestId.Equals(x.TestId)) != null)
                      .Subscribe(
                          res =>
                              {
                                  var nextRes = res as NextResult;
                                  var errorRes = res as ExperimentError;
                                  if (nextRes != null)
                                  {
                                      AddPoint(memorySeries, MemoryPlotModel,
                                               tests.First(x => x.TestId.Equals(res.TestId)), nextRes.Descriptor,
                                               nextRes.MemoryUsage);

                                      AddPoint(speedSeries, SpeedPlotModel,
                                               tests.First(x => x.TestId.Equals(res.TestId)), nextRes.Descriptor,
                                               nextRes.Duration);
                                  }

                                  if (errorRes != null)
                                  {
                                      var test = tests.FirstOrDefault(x => x.TestId.Equals(errorRes.TestId));
                                      Task.Factory.StartNew(() => errorHandler.ReportExperimentError(errorRes, test));
                                  }
                              }, errorHandler.ReportException);

            ConnectIterationAndDescriptors();
        }

        private void ConnectIterationAndDescriptors()
        {

            this.ObservableForProperty(x => x.StartValue, false, false).Subscribe(x => this.RefreshDescriptors());

            this.ObservableForProperty(x => x.EndValue, false, false).Subscribe(x => this.RefreshDescriptors());

            this.ObservableForProperty(x => x.StepValue, false, false).Subscribe(x => this.RefreshDescriptors());
        }

        private void RefreshDescriptors()
        {
            this.StartDescriptor = this.GetDescriptor(this.StartValue);
            this.EndDescriptor = this.GetDescriptor(this.EndValue);

            this.StepDescriptor = (EndDescriptor - StartDescriptor) * this.StepValue
                                  / (this.EndValue - this.StartValue);
        }

        private object tester;

        private double GetDescriptor(int iteration)
        {
            if (tester == null)
            {
                tester = TestSuiteInfo.TesterType.CreateInstance();
            }

            if (string.IsNullOrEmpty(TestSuiteInfo.GetDescriptoMethodName))
            {
                return (double)iteration;
            }

            return (double)tester.CallMethod(TestSuiteInfo.GetDescriptoMethodName, iteration);
        }

        private IDisposable subscription = null;

        private void SetAxisType(EAxisType axisType)
        {
            SetAxisType(SpeedPlotModel, axisType);
            SetAxisType(MemoryPlotModel, axisType);
        }

        private static void SetAxisType(PlotModel plotModel, EAxisType axisType)
        {
            var oldAxis = plotModel.Axes.FirstOrDefault(x => x.Position == AxisPosition.Left);
            if (oldAxis != null)
            {
                plotModel.Axes.Remove(oldAxis);
            }

            if (axisType == EAxisType.Linear)
            {
                plotModel.Axes.Add(new LinearAxis(AxisPosition.Left));
            }
            else
            {
                plotModel.Axes.Add(new LogarithmicAxis(AxisPosition.Left));
            }

            plotModel.RefreshPlot(true);
        }

        private void OnStartSequential(object param)
        {
            PrepareStart();
            var errorHandler = IoC.Instance.Resolve<ErrorHandler>();
            Task.Factory.StartNew(
                () =>
                    {
                        subscription = this.Lab.Run(tests.Select(x => x.TestId)
                                                         .ToArray(), StartValue, StepValue, EndValue, false)
                                           .Subscribe(
                                               res => MessageBus.Current.SendMessage(res),
                                               ex =>
                                                   {
                                                       errorHandler.ReportException(ex);
                                                       IsStarted = false;
                                                   },
                                               () => { IsStarted = false; });
                    });
        }

        private void OnStartParallel(object param)
        {
            PrepareStart();

            Task.Factory.StartNew(
                () =>
                    {
                        subscription = this.Lab.Run(tests.Select(x => x.TestId)
                                                         .ToArray(), StartValue, StepValue, EndValue, true)
                                           .Subscribe(
                                               res => MessageBus.Current.SendMessage(res),
                                               ex => { IsStarted = false; },
                                               () => { IsStarted = false; });
                    });
        }

        private void PrepareStart()
        {
            IsStarted = true;

            foreach (var series in memorySeries.Select(m => m.Value)
                                               .Union(speedSeries.Select(s => s.Value)))
            {
                series.Points.Clear();
            }

            SpeedPlotModel.RefreshPlot(true);
            MemoryPlotModel.RefreshPlot(true);
        }

        private void OnStop(object param)
        {
            subscription.Dispose();
            subscription = null;
            IsStarted = false;
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
                }

                return view;
            }
        }

        public IEnumerable<TestInfo> Tests
        {
            get { return this.tests.AsEnumerable(); }
        }

        public PlotModel SpeedPlotModel { get; private set; }

        public PlotModel MemoryPlotModel { get; private set; }

        public ReactiveAsyncCommand StartSequential { get; private set; }

        public ReactiveAsyncCommand StartParallel { get; private set; }

        public ReactiveAsyncCommand Stop { get; private set; }

        public TestSuiteInfo TestSuiteInfo { get; private set; }

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
            if (!dict.ContainsKey(test))
            {
                var series = new LineSeries(test.ToString());
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
                plotModel.RefreshPlot(true);
            }
        }

        #region IsLinear

        private bool isLinear;

        public bool IsLinear
        {
            get { return this.isLinear; }
            set { this.RaiseAndSetIfChanged(x => x.IsLinear, ref this.isLinear, value); }
        }

        #endregion // IsLinear

        #region IsStarted

        private bool isStarted;

        public bool IsStarted
        {
            get { return this.isStarted; }
            set { this.RaiseAndSetIfChanged(x => x.IsStarted, ref this.isStarted, value); }
        }

        #endregion // IsStarted

        #region Lab

        private PerfLab lab;

        public PerfLab Lab
        {
            get { return this.lab; }
            set { this.RaiseAndSetIfChanged(x => x.Lab, ref this.lab, value); }
        }

        #endregion // Lab

        #region StartValue

        private int startValue;

        public int StartValue
        {
            get { return this.startValue; }
            set { this.RaiseAndSetIfChanged(x => x.StartValue, ref this.startValue, value); }
        }

        #endregion // StartValue

        #region EndValue

        private int endValue;

        public int EndValue
        {
            get { return this.endValue; }
            set { this.RaiseAndSetIfChanged(x => x.EndValue, ref this.endValue, value); }
        }

        #endregion // EndValue

        #region StepValue

        private int stepValue;

        public int StepValue
        {
            get { return this.stepValue; }
            set { this.RaiseAndSetIfChanged(x => x.StepValue, ref this.stepValue, value); }
        }

        #endregion // StepValue
        

        public string FeatureDescription
        {
            get
            {
                return this.TestSuiteInfo.FeatureDescription;
            }
        }

        
        #region StartDescriptor

        private double startDescriptor;

        public double StartDescriptor
        {
            get { return this.startDescriptor; }
            set { this.RaiseAndSetIfChanged(x => x.StartDescriptor, ref this.startDescriptor, value); }
        }

        #endregion // StartDescriptor
        
        #region EndDescriptor

        private double endDescriptor;

        public double EndDescriptor
        {
            get { return this.endDescriptor; }
            set { this.RaiseAndSetIfChanged(x => x.EndDescriptor, ref this.endDescriptor, value); }
        }

        #endregion // EndDescriptor
        
        #region StepDescriptor

        private double stepDescriptor;

        public double StepDescriptor
        {
            get { return this.stepDescriptor; }
            set { this.RaiseAndSetIfChanged(x => x.StepDescriptor, ref this.stepDescriptor, value); }
        }

        #endregion // StepDescriptor
    }
}