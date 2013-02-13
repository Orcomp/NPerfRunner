using Ninject;
using NPerfRunner.Views;
using ReactiveUI;
using ReactiveUI.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Threading;
using System.Reactive.Concurrency;
using System.Windows;

namespace NPerfRunner.ViewModels
{
    public class MainWindowViewModel : ReactiveObject, IScreen
    {

        public IRoutingState Router { get; private set; }

        public MainWindowViewModel()
        {
            this.Router = new RoutingState();
        }
    }
}
