namespace NPerfRunner.ViewModels
{
    using ReactiveUI.Routing;
    using ReactiveUI.Xaml;

    public interface ISettingsViewModel : IRoutableViewModel
    {
        ReactiveAsyncCommand LoadTool { get; }

        IReactiveCommand LoadSubject { get; }

        IReactiveCommand StartTesting { get; }

        IReactiveCommand StopTesting { get; }

        string TesterFileName { get; set; }
    }
}
