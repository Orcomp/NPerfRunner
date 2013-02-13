using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Text;
using System.Windows;
using NPerfRunner.ViewModels;
using NPerfRunner.Views;
using ReactiveUI;
using ReactiveUI.Routing;

namespace NPerfRunner.Wpf
{
    internal class InfrastructureInstaller
    {
        public static void Install()
        {
            

            IoC.Instance.RegisterSingleton(typeof(IScreen), typeof(MainWindowViewModel));
            IoC.Instance.RegisterSingleton(typeof(ISettingsViewModel), typeof(SettingsViewModel));
            IoC.Instance.RegisterSingleton(typeof(IViewFor<ISettingsViewModel>), typeof(SettingsView));

            RxApp.DeferredScheduler = new DispatcherScheduler(Application.Current.Dispatcher);

            RxApp.ConfigureServiceLocator(
                (iface, contract) => IoC.Instance.Resolve(iface, contract),
                (iface, contract) => IoC.Instance.ResolveAll(iface, contract),
                (realClass, iface, contract) => IoC.Instance.RegisterType(realClass, iface, contract));
        }
    }
}
