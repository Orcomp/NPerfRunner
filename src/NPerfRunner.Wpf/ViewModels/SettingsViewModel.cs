namespace NPerfRunner.Wpf.ViewModels
{
    using NPerfRunner.Dialogs;
    using NPerfRunner.ViewModels;
    using ReactiveUI;
    using ReactiveUI.Routing;
    using ReactiveUI.Xaml;

    public class SettingsViewModel : ReactiveObject, ISettingsViewModel
    {
        public ReactiveAsyncCommand LoadTool { get; protected set; }

        public IReactiveCommand LoadSubject { get; protected set; }

        public IReactiveCommand StartTesting { get; protected set; }

        public IReactiveCommand StopTesting { get; protected set; }

        private string testerFileName;

        public string TesterFileName
        {
            get { return this.testerFileName; }
            set { this.RaiseAndSetIfChanged(x => x.TesterFileName, ref this.testerFileName, value); }
        }

        public IScreen HostScreen { get; protected set; }

        public string UrlPathSegment
        {
            get { return "settings"; }
        }

        public SettingsViewModel(IScreen screen)
        {
            this.HostScreen = screen;
            
            this.LoadTool = new ReactiveAsyncCommand();

            this.LoadTool.RegisterAsyncAction(this.OnLoadTool, RxApp.DeferredScheduler);
            IoC.Instance.Resolve<ErrorHandler>().HandleErrors(this.LoadTool);
        }

        private void OnLoadTool(object param)
        {
            IoC.Instance.Resolve<ILoadAssemblyDialog>().LoadAssembly();
        }
    }
}
