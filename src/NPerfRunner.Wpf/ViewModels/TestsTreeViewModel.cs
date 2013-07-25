using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPerfRunner.ViewModels;

namespace NPerfRunner.Wpf.ViewModels
{
    using System.Windows;
    using NPerfRunner.Wpf.ViewModels.PerfTestTree;
    using ReactiveUI;

    public class TestsTreeViewModel : ToolViewModel, ITestsTreeViewModel, ICreatesObservableForProperty
    {
        public TestsTreeViewModel()
            : base("Tests")
        {
            this.Testers = new ReactiveCollection<TesterNodeViewModel>();

            var commonData = IoC.Instance.Resolve<CommonData>();

            var whenLabLoaded = commonData.WhenAny(x => x.Lab, x => x.Value != null);

            whenLabLoaded.Subscribe(labsLoaded =>
            {
                try
                {
                    var testers = (ReactiveCollection<TesterNodeViewModel>)this.Testers;
                    testers.Clear();

                    if (labsLoaded)
                    {
                        testers.AddRange(
                            commonData.Lab.TestSuites.OrderBy(x => x.TestSuiteDescription)
                                      .Distinct()
                                      .Select(x => new TesterNodeViewModel(commonData.Lab, x) { IsChecked = false }));
                    }
                }
                catch (Exception ex)
                {
                    throw new NotImplementedException();
                }
            });
        }

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
