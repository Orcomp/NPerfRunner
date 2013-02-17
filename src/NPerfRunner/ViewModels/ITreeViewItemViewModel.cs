using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPerfRunner.ViewModels
{
    public interface ITreeViewItemViewModel : IReactiveNotifyPropertyChanged
    {
        IReactiveCollection<ITreeViewItemViewModel> Children { get; }
        string Name { get; }
        bool? IsChecked { get; set; }
    }
}
