using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPerfRunner.ViewModels
{
    using ReactiveUI;

    public interface IMainWindowViewModel : IReactiveNotifyPropertyChanged
    {
        ReactiveCollection<IToolViewModel> Tools { get; }

        ReactiveCollection<IChartViewModel> Charts { get; }
    }
}
