using ReactiveUI;
using ReactiveUI.Routing;
using ReactiveUI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPerfRunner.ViewModels
{
    public interface ISettingsViewModel : IRoutableViewModel
    {
        IReactiveCommand LoadTool { get; }
        IReactiveCommand LoadSubject { get; }
        IReactiveCommand StartTesting { get; }
        IReactiveCommand StopTesting { get; }

        string TesterFileName { get; set; }
    }
}
