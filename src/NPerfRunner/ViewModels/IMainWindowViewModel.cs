using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPerfRunner.ViewModels
{
    using System.Collections.ObjectModel;
    using ReactiveUI;
    using ReactiveUI.Xaml;

    public interface IMainWindowViewModel : IReactiveNotifyPropertyChanged
    {
        ReactiveCollection<IToolViewModel> Tools { get; }

        ObservableCollection<IChartViewModel> Charts { get; }

        ReactiveAsyncCommand DocClosed { get; }
    }
}
