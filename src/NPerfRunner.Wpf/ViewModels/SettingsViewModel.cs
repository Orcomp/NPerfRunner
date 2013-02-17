namespace NPerfRunner.Wpf.ViewModels
{
    using System;
    using System.Linq;
    using System.Reactive;
    using System.Reactive.Linq;
    using System.Reflection;
    using NPerf.Lab;
    using NPerfRunner.Dialogs;
    using NPerfRunner.ViewModels;
    using ReactiveUI;
    using ReactiveUI.Routing;
    using ReactiveUI.Xaml;

    public class SettingsViewModel : ReactiveObject, ISettingsViewModel
    {
        public ReactiveAsyncCommand LoadTool { get; protected set; }

        public ReactiveAsyncCommand LoadSubject { get; protected set; }

        public ReactiveAsyncCommand StartTesting { get; protected set; }

        public IReactiveCommand StopTesting { get; protected set; }

        #region TesterAssembly
        private Assembly testerAssembly;
        public Assembly TesterAssembly
        {
            get
            {
                return this.testerAssembly;
            }
            
            set
            {
                this.RaiseAndSetIfChanged(x => x.TesterAssembly, ref this.testerAssembly, value);
            }
        }
        #endregion // TesterAssembly

        #region TestedAssembly
        private Assembly testedAssembly;
        public Assembly TestedAssembly
        {
            get
            {
                return this.testedAssembly;
            }

            set
            {
                this.RaiseAndSetIfChanged(x => x.TestedAssembly, ref this.testedAssembly, value);
            }
        }
        #endregion // TestedAssembly

        #region Lab
        private PerfLab lab;
        public PerfLab Lab
        {
            get
            {
                return this.lab;
            }
            set
            {
                this.RaiseAndSetIfChanged(x => x.Lab, ref this.lab, value);
            }
        }
        #endregion // Lab

        public SystemInfo SysInfo
        {
            get
            {
                return SystemInfo.Instance;
            }
        }

        public IReactiveCollection<ITesterViewModel> Testers
        {
            get;
            private set;
        }

        public SettingsViewModel()
        {
            var errorHandler = IoC.Instance.Resolve<ErrorHandler>();

            this.Testers = new ReactiveCollection<ITesterViewModel>();

            this.LoadTool = new ReactiveAsyncCommand();
            this.LoadTool.RegisterAsyncAction(this.OnLoadTool, RxApp.DeferredScheduler);
            errorHandler.HandleErrors(this.LoadTool);

            this.LoadSubject = new ReactiveAsyncCommand();
            this.LoadSubject.RegisterAsyncAction(this.OnLoadSubject, RxApp.DeferredScheduler);
            errorHandler.HandleErrors(this.LoadSubject);            

            var whenAssembliesLoaded = this.WhenAny(
                    x => x.TestedAssembly, 
                    x => x.TesterAssembly, 
                    (tested, tester) => tested.Value != null && tester.Value != null);

            whenAssembliesLoaded.Where(x => x).Subscribe(_ => 
                {
                    try
                    {
                        this.Lab = new PerfLab(this.TesterAssembly, this.TestedAssembly);
                    }
                    catch (Exception ex)
                    {
                        throw new NotImplementedException();
                    }
                });

            var whenLabLoaded = this.WhenAny(x => x.Lab, x => x.Value != null);

            this.StartTesting = new ReactiveAsyncCommand(whenLabLoaded);
            this.StartTesting.RegisterAsyncAction(this.OnStartTesting, RxApp.DeferredScheduler);
            errorHandler.HandleErrors(this.StartTesting);

            whenLabLoaded.Where(x => x).Subscribe(_ =>
                {
                    try
                    {
                        var testers = ((ReactiveCollection<ITesterViewModel>)this.Testers);
                        testers.Clear();
                        testers.AddRange(
                            this.Lab.TestSuites.Select(x => x.TesterType)
                            .Distinct().Select(x => new TesterViewModel(this.Lab, x) as ITesterViewModel));
                       
                        foreach (var testerType in this.Lab.TestSuites.Select(x => x.TesterType).Distinct())
                        {
                            testers.Add(new TesterViewModel(this.Lab, testerType));
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new NotImplementedException();
                    }
                });
        }
        
        private void OnLoadTool(object param)
        {
            this.TesterAssembly = IoC.Instance.Resolve<ILoadAssemblyDialog>().LoadAssembly("Load tester assembly");
        }

        private void OnLoadSubject(object param)
        {
            this.TestedAssembly = IoC.Instance.Resolve<ILoadAssemblyDialog>().LoadAssembly("Load tested assembly");
        }

        private void OnStartTesting(object param)
        {
            this.Lab.Run();
        }        
    }
}
