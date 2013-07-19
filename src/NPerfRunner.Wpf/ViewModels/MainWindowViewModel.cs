namespace NPerfRunner.Wpf.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Reactive;
    using System.Reactive.Linq;
    using System.Reflection;
    using System.Windows;
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

    /*        this.LoadTool = new ReactiveAsyncCommand();
            this.LoadTool.RegisterAsyncAction(OnLoadTool, RxApp.DeferredScheduler);
            errorHandler.HandleErrors(this.LoadTool);*/

            this.LoadAssebmly = new ReactiveAsyncCommand();
            this.LoadAssebmly.RegisterAsyncAction(OnLoadAssembly, RxApp.DeferredScheduler);
            errorHandler.HandleErrors(this.LoadAssebmly);

            DocClosed = new ReactiveAsyncCommand();
            DocClosed.RegisterAsyncAction(OnDocumentClosed, RxApp.DeferredScheduler);
            errorHandler.HandleErrors(this.DocClosed);
        }
/*
        public ReactiveAsyncCommand LoadTool { get; protected set; }*/

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

 /*       private void OnLoadTool(object param)
        {
            var selectedAssembly = IoC.Instance.Resolve<ILoadAssemblyDialog>().LoadAssembly("Load tester assembly");
            if (selectedAssembly == null)
            {
                return;
            }
            var commonData = IoC.Instance.Resolve<CommonData>();

            commonData.TesterAssembly = selectedAssembly;

            ReloadLab();

            StatusText = string.Format("Loaded tester: {0}", selectedAssembly);
        }*/

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

            if (testNode != null)
            {
                chart =
                    this.Charts.FirstOrDefault(x => x.TestName == testNode.Name && x.TesterType == testNode.TesterType);
            }            

            if (chart != null && testNode != null && !(testNode.IsChecked ?? true))
            {
                this.Charts.Remove(chart);
                chart = null;
            }

            if (chart == null && testNode != null && (testNode.IsChecked ?? true))
            {
                this.Charts.Add(new ChartViewModel(testNode.PerfLab, testNode.TesterType, testNode.Tests.First().TestMethodName));
            }

            if (testedNode != null)
            {
                chart =
                    this.Charts.FirstOrDefault(
                        x =>
                        x.TestName == testedNode.TestInfo.TestMethodName
                        && x.TesterType == testedNode.TestInfo.Suite.TesterType);
            }

            if (chart != null && testedNode != null && (testedNode.IsChecked ?? false))
            {
                chart.Add(testedNode.TestInfo);
            }

            if (chart != null && testedNode != null && !(testedNode.IsChecked ?? false))
            {
                chart.Remove(testedNode.TestInfo);
            }

           /* var existed =
                this.Charts.FirstOrDefault(
                    x => x.TestName == testCheckChanged.TestInfo.TestMethodName && x.TesterType == testCheckChanged.TestInfo.Suite.TesterType);

            var isChecked = testCheckChanged.Checked == null || testCheckChanged.Checked.Value;

            if (isChecked && existed == null)
            {
                this.Charts.Add(new ChartViewModel(testCheckChanged.Lab, testCheckChanged.TestInfo));
            }

            if (isChecked && existed != null)
            {
                existed.Add(testCheckChanged.TestInfo);
            }

            if (!isChecked && existed != null)
            {
                existed.Remove(testCheckChanged.TestInfo);
                this.Charts.Remove(existed);
            }*/
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
