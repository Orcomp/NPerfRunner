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
            this.Tools = new ReactiveCollection<IToolViewModel> { IoC.Instance.Resolve<ITestsTreeViewModel>(), IoC.Instance.Resolve<IConsoleViewModel>() };
            this.Charts = new ObservableCollection<IChartViewModel>();

            MessageBus.Current.Listen<TestCheckChanged>().Subscribe(OnTestCheckChanged);

            var errorHandler = IoC.Instance.Resolve<ErrorHandler>();

            this.LoadAssebmly = new ReactiveAsyncCommand();
            this.LoadAssebmly.RegisterAsyncAction(OnLoadAssembly, RxApp.DeferredScheduler);
            errorHandler.HandleErrors(this.LoadAssebmly);

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

        private static void OnLoadAssembly(object param)
        {
            var testedAssembly = IoC.Instance.Resolve<ILoadAssemblyDialog>().LoadAssembly("Load tested assembly");
            if (testedAssembly == null)
            {
                return;
            }

            var commonData = IoC.Instance.Resolve<CommonData>();

            var testedAssemblies = (ReactiveCollection<Assembly>)commonData.LoadedAssemblies;
            testedAssemblies.Add(testedAssembly);
            ReloadLab();
        }

        private static void ReloadLab()
        {
            var commonData = IoC.Instance.Resolve<CommonData>();

            commonData.Lab = null;
            if (commonData.LoadedAssemblies.Any())
            {
                commonData.Lab = new PerfLab(commonData.LoadedAssemblies.ToArray());
            }
        }

        private void OnTestCheckChanged(TestCheckChanged testCheckChanged)
        {
            var testNode = testCheckChanged.TreeItem as TestNodeViewModel;
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

        public ReactiveCollection<IToolViewModel> Tools { get; private set; }

        public ObservableCollection<IChartViewModel> Charts { get; private set; }

        public ReactiveAsyncCommand DocClosed { get; private set; }

        #region StatusText
        private string statusText;
        public string StatusText
        {
            get
            {
                return this.statusText;
            }
            set
            {
                this.RaiseAndSetIfChanged(x => x.StatusText, ref this.statusText, value);
            }
        }
        #endregion // StatusText
    }
}
