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

            RxRouting.ViewModelToViewFunc = str =>
               {
                   var viewModelType = AppDomain.CurrentDomain
                       .GetAssemblies().SelectMany(x => x.GetTypes())
                       .Where(type => type.AssemblyQualifiedName.Equals(str)).FirstOrDefault();

                   if (viewModelType == null)
                   {
                       return string.Empty;
                   }

                   var interfaceName = string.Format("I{0}", viewModelType.Name);
                   var viewModelInterface = viewModelType.GetInterface(interfaceName);

                   var viewType = typeof(IViewFor<>).MakeGenericType(viewModelInterface);

                   return viewType.AssemblyQualifiedName;
               };

            RxApp.ConfigureServiceLocator(
                (iface, contract) => IoC.Instance.Resolve(iface, contract),
                (iface, contract) => IoC.Instance.ResolveAll(iface, contract),
                (realClass, iface, contract) => IoC.Instance.RegisterType(realClass, iface, contract));
        }
    }
}
