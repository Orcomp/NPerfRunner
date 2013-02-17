namespace NPerfRunner.ViewModels
{
    using ReactiveUI.Routing;
    using ReactiveUI.Xaml;
    using System.Reflection;
    using NPerf.Lab;
    using ReactiveUI;

    public interface ISettingsViewModel : IReactiveNotifyPropertyChanged
    {
        ReactiveAsyncCommand LoadTool { get; }

        ReactiveAsyncCommand LoadSubject { get; }

        ReactiveAsyncCommand StartTesting { get; }

        IReactiveCommand StopTesting { get; }

        Assembly TesterAssembly { get; set; }

        Assembly TestedAssembly { get; set; }

        PerfLab Lab { get; set; }

        IReactiveCollection<ITesterViewModel> Testers { get; }
    }
}
