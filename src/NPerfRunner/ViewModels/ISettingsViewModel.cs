namespace NPerfRunner.ViewModels
{
    using ReactiveUI.Routing;
    using ReactiveUI.Xaml;
    using System.Reflection;
    using NPerf.Lab;
    using ReactiveUI;

    public interface ISettingsViewModel : IToolViewModel
    {
        ReactiveAsyncCommand LoadTool { get; }

        ReactiveAsyncCommand LoadSubject { get; }

        ReactiveAsyncCommand DeleteSubject { get; }

        ReactiveAsyncCommand StartTesting { get; }

        IReactiveCommand StopTesting { get; }

        Assembly TesterAssembly { get; set; }

        PerfLab Lab { get; set; }

        IReactiveCollection Testers { get; }
    }
}
