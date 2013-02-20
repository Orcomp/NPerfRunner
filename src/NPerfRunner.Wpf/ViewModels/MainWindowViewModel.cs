namespace NPerfRunner.Wpf.ViewModels
{
    using NPerfRunner.ViewModels;

    using ReactiveUI;
    using ReactiveUI.Routing;

    public class MainWindowViewModel : ReactiveObject, IMainWindowViewModel
    {
        public MainWindowViewModel()
        {
            this.Tools = new ReactiveCollection<IToolViewModel> { IoC.Instance.Resolve<ISettingsViewModel>() };
            this.Charts = new ReactiveCollection<IChartViewModel>();
        }

        public ReactiveCollection<IToolViewModel> Tools { get; private set; }

        public ReactiveCollection<IChartViewModel> Charts { get; private set; }
    }
}
