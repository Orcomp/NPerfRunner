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

        public ReactiveAsyncCommand DeleteSubject { get; protected set; }

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

        public IReactiveCollection<Assembly> TestedAssemblies { get; private set; }

        #region SelectedTestedAssembly
        private Assembly selectedTestedAssembly;
        public Assembly SelectedTestedAssembly
        { 
            get
            {
                return this.selectedTestedAssembly;
            }
            set
            {
                this.RaiseAndSetIfChanged(x => x.SelectedTestedAssembly, ref this.selectedTestedAssembly, value);
            }
        }
        #endregion // SelectedTestedAssembly

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
            this.TestedAssemblies = new ReactiveCollection<Assembly>();

            this.LoadTool = new ReactiveAsyncCommand();
            this.LoadTool.RegisterAsyncAction(this.OnLoadTool, RxApp.DeferredScheduler);
            errorHandler.HandleErrors(this.LoadTool);

            this.LoadSubject = new ReactiveAsyncCommand();
            this.LoadSubject.RegisterAsyncAction(this.OnLoadSubject, RxApp.DeferredScheduler);
            errorHandler.HandleErrors(this.LoadSubject);

            this.DeleteSubject = new ReactiveAsyncCommand(this.WhenAny(x => x.SelectedTestedAssembly, x => x.Value != null));
            this.DeleteSubject.RegisterAsyncAction(this.OnDeleteSubject, RxApp.DeferredScheduler);
            errorHandler.HandleErrors(this.DeleteSubject);

            var whenAssembliesLoaded = this.WhenAny(
                    x => x.TestedAssemblies, 
                    x => x.TesterAssembly, 
                    (tested, tester) => tested.Value.Count() != 0 && tester.Value != null);
 
            var whenLabLoaded = this.WhenAny(x => x.Lab, x => x.Value != null);

            this.StartTesting = new ReactiveAsyncCommand(whenLabLoaded);
            this.StartTesting.RegisterAsyncAction(this.OnStartTesting, RxApp.DeferredScheduler);
            errorHandler.HandleErrors(this.StartTesting);

            whenLabLoaded.Subscribe(labsLoaded =>
                {
                    try
                    {
                        var testers = ((ReactiveCollection<ITesterViewModel>)this.Testers);
                        testers.Clear();

                        if (labsLoaded)
                        {
                            testers.AddRange(
                                this.Lab.TestSuites.Select(x => x.TesterType)
                                .Distinct().Select(x => new TesterViewModel(this.Lab, x) as ITesterViewModel));
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
            var selectedAssembly = IoC.Instance.Resolve<ILoadAssemblyDialog>().LoadAssembly("Load tester assembly");
            if(selectedAssembly == null)
            {
                return;
            }

            this.TesterAssembly = selectedAssembly;
            this.ReloadLab();
        }

        private void OnDeleteSubject(object param)
        { 
            var subjects = (ReactiveCollection<Assembly>)this.TestedAssemblies;
            subjects.Remove(this.SelectedTestedAssembly);
            this.ReloadLab();
        }

        private void ReloadLab()
        {
            this.Lab = null;
            if(this.TestedAssemblies.Count() > 0 && this.TesterAssembly != null)
            {
                this.Lab = new PerfLab(this.TesterAssembly, this.TestedAssemblies.ToArray());
            }            
        }

        private void OnLoadSubject(object param)
        {
            var testedAssembly = IoC.Instance.Resolve<ILoadAssemblyDialog>().LoadAssembly("Load tested assembly");
            if(testedAssembly == null)
            {
                return;
            }

            var testedAssemblies = (ReactiveCollection<Assembly>)this.TestedAssemblies;
            testedAssemblies.Add(testedAssembly);
            this.ReloadLab();
        }

        private void OnStartTesting(object param)
        {
            this.Lab.Run();
        }        
    }
}
