namespace NPerfRunner.Wpf.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Reactive;
    using System.Reactive.Linq;
    using System.Reflection;
    using System.Windows;

    using NPerf.Core.Info;
    using NPerf.Lab;
    using NPerfRunner.Dialogs;
    using NPerfRunner.ViewModels;
    using System;
    using System.Linq;

    using NPerfRunner.Wpf.ViewModels.PerfTestTree;

    using ReactiveUI;
    using NPerfRunner.Wpf.Messages;
    using ReactiveUI.Xaml;
    using Xceed.Wpf.AvalonDock;

    public class MainWindowViewModel : ReactiveObject, IMainWindowViewModel
    {
        public MainWindowViewModel()
        {
            this.Tools = new ReactiveCollection<IToolViewModel> { IoC.Instance.Resolve<ITestsTreeViewModel>() };
            this.Charts = new ObservableCollection<IChartViewModel>();

            MessageBus.Current.Listen<TestCheckChanged>().Subscribe(OnTestCheckChanged);

            var errorHandler = IoC.Instance.Resolve<ErrorHandler>();

            MessageBus.Current.Listen<LoadAssembly>()
                      .Subscribe(OnLoadAssembly);

            MessageBus.Current.Listen<ClearAssembliesList>().Subscribe(OnClearAssembliesList);

            DocClosed = new ReactiveAsyncCommand();
            DocClosed.RegisterAsyncAction(OnDocumentClosed, RxApp.DeferredScheduler);
            errorHandler.HandleErrors(this.DocClosed);
        }

        public ReactiveAsyncCommand LoadAssebmly { get; protected set; }

        private void OnDocumentClosed(object param)
        {
            var eventArgs = ((EventPattern<object, DocumentClosedEventArgs>) (param)).EventArgs;
            var chart = (IChartViewModel) eventArgs.Document.Content;
            Charts.Remove(chart);
            MessageBus.Current.SendMessage(new ChartRemoved(chart));
        }

        private void OnClearAssembliesList(ClearAssembliesList param)
        {
            var commonData = IoC.Instance.Resolve<CommonData>();
            commonData.Lab = null;
            ClearCharts();
            ((ReactiveCollection<Assembly>)commonData.LoadedAssemblies).Clear();
        }

        private void OnLoadAssembly(LoadAssembly param)
        {
            var assembly = IoC.Instance.Resolve<ILoadAssemblyDialog>().LoadAssembly("Load assembly");
            if (assembly == null)
            {
                return;
            }

            ReloadLab(assembly);
        }

        private void ClearCharts()
        {
            foreach (var chart in this.Charts.Where(x => x.IsStarted))
            {
                chart.Stop.Publish();
            }

            this.Charts.Clear();
        }

        private void ReloadLab(params Assembly[] assemblies)
        {
            var commonData = IoC.Instance.Resolve<CommonData>();

            ClearCharts();

            if (!assemblies.Any() && !commonData.LoadedAssemblies.Any())
            {
                commonData.Lab = null;
            }

            if (assemblies.Any() && commonData.Lab == null)
            {
                commonData.Lab = new PerfLab(commonData.LoadedAssemblies.Union(assemblies).ToArray());
            }

            if (assemblies.Any() && commonData.Lab != null)
            {
                commonData.Lab.AddAssemblies(assemblies);
            }

            foreach (var assembly in assemblies)
            {
                if (!((ReactiveCollection<Assembly>) commonData.LoadedAssemblies).Contains(assembly))
                {
                    ((ReactiveCollection<Assembly>) commonData.LoadedAssemblies).Add(assembly);
                }
            }
        }

        private void OnTestCheckChanged(TestCheckChanged testCheckChanged)
        {
            var testerNode = testCheckChanged.TreeItem as TesterNodeViewModel;
            var testedNode = testCheckChanged.TreeItem as TestedTypeNodeViewModel;

            IChartViewModel chart = null;

            if (testerNode != null)
            {
                chart = this.Charts.FirstOrDefault(x => x.TestSuiteInfo.Equals(testerNode.TestSuiteInfo));
            }

            if (chart != null && !(testerNode.IsChecked ?? true))
            {
                this.Charts.Remove(chart);                
            }

            if (chart == null && testerNode != null && (testerNode.IsChecked ?? true))
            {
                this.Charts.Add(new ChartViewModel(testerNode.Lab, testerNode.TestSuiteInfo));
            }

            chart = null;
            TestInfo test = null;

            if (testedNode != null)
            {
                chart = (from ch in this.Charts
                         where Equals(ch.TestSuiteInfo, testedNode.TestInfo.Suite)
                         select ch).FirstOrDefault();
            }

            if (chart != null)
            {
                test = (from t in chart.Tests where t.TestId == testedNode.TestInfo.TestId select t).FirstOrDefault();
            }

            if (chart != null && test != null && !(testedNode.IsChecked ?? true))
            {
                chart.Remove(testedNode.TestInfo);
            }

            if (chart != null && test == null && (testedNode.IsChecked ?? true))
            {
                chart.Add(testedNode.TestInfo);
            }

        }

        public ReactiveCollection<IToolViewModel> Tools { get;  set; }

        public ObservableCollection<IChartViewModel> Charts { get; set; }

        public ReactiveAsyncCommand DocClosed { get; private set; }
    }
}
