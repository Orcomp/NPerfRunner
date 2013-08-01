using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPerfRunner.ViewModels;

namespace NPerfRunner.Wpf.ViewModels
{
    using System.Reflection;
    using System.Windows;
    using NPerfRunner.Wpf.Messages;
    using NPerfRunner.Wpf.ViewModels.PerfTestTree;
    using ReactiveUI;
    using ReactiveUI.Xaml;

    public class TestsTreeViewModel : ToolViewModel, ITestsTreeViewModel, ICreatesObservableForProperty
    {
        public TestsTreeViewModel()
            : base("Tests")
        {
            this.Testers = new ReactiveCollection<TesterNodeViewModel>();

            var commonData = IoC.Instance.Resolve<CommonData>();

            var errorHandler = IoC.Instance.Resolve<ErrorHandler>();

            this.LoadAssembly = new ReactiveAsyncCommand();
            this.LoadAssembly.RegisterAsyncAction(OnLoadAssembly, RxApp.DeferredScheduler);
            errorHandler.HandleErrors(this.LoadAssembly);

            this.ClearAssembliesList = new ReactiveAsyncCommand();
            this.ClearAssembliesList.RegisterAsyncAction(OnClearList, RxApp.DeferredScheduler);
            errorHandler.HandleErrors(this.ClearAssembliesList);

            commonData.LoadedAssemblies.CollectionCountChanged.Subscribe(count =>
            {
                try
                {
                    var testers = (ReactiveCollection<TesterNodeViewModel>)this.Testers;
                    testers.Clear();

                    if (commonData.Lab != null)
                    {
                        testers.AddRange(
                            commonData.Lab.TestSuites.OrderBy(x => x.TestSuiteDescription)
                                      .Distinct()
                                      .Select(x => new TesterNodeViewModel(commonData.Lab, x) { IsChecked = false }));
                    }
                }
                catch (Exception ex)
                {
                    errorHandler.ReportException(ex);
                }
            });
        }


        private static void OnLoadAssembly(object param)
        {
            MessageBus.Current.SendMessage(new LoadAssembly());
        }

        private static void OnClearList(object param)
        {
            MessageBus.Current.SendMessage(new ClearAssembliesList());
        }

        public IReactiveCollection LoadedAssemblies
        {
            get
            {
                return IoC.Instance.Resolve<CommonData>()
                          .LoadedAssemblies;
            }

        }

        public ReactiveAsyncCommand LoadAssembly { get; private set; }

        public ReactiveAsyncCommand ClearAssembliesList { get; private set; }

        public IReactiveCollection Testers
        {
            get;
            private set;
        }

        private FrameworkElement view;
        public override System.Windows.FrameworkElement View
        {
            get
            {
                return this.view
                       ?? (this.view = (FrameworkElement)IoC.Instance.Resolve<IViewFor<ITestsTreeViewModel>>());
            }
        }

        public int GetAffinityForObject(Type type, bool beforeChanged = false)
        {
            throw new NotImplementedException();
        }

        public IObservable<IObservedChange<object, object>> GetNotificationForProperty(object sender, string propertyName, bool beforeChanged = false)
        {
            throw new NotImplementedException();
        }
    }
}
