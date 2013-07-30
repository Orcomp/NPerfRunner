using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReactiveUI;

namespace NPerfRunner.ViewModels
{
    using System.Windows;
    using System.Windows.Media;

    public interface IPaneViewModel : IReactiveNotifyPropertyChanged
    {
        ImageSource IconSource { get; }

        string Title { get; set; }

        string ContentId { get; set; }

        FrameworkElement View { get; }
    }
}
