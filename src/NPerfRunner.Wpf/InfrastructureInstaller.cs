namespace NPerfRunner.Wpf
{
    using System;
    using System.Linq;
    using System.Reactive.Concurrency;
    using System.Windows;
    using NPerfRunner.Dialogs;
    using NPerfRunner.ViewModels;
    using NPerfRunner.Wpf.Dialogs;
    using NPerfRunner.Wpf.ViewModels;
    using NPerfRunner.Wpf.Views;
    using ReactiveUI;
    using ReactiveUI.Routing;

    internal class InfrastructureInstaller
    {
        public static void Install()
        {
            IoC.Instance.RegisterSingleton(typeof(ErrorHandler));
            IoC.Instance.RegisterSingleton(typeof(CommonData));

            IoC.Instance.RegisterSingleton(typeof(IMainWindowViewModel), typeof(MainWindowViewModel));
            IoC.Instance.RegisterSingleton(typeof(IViewFor<IMainWindowViewModel>), typeof(MainWindow));

            IoC.Instance.RegisterSingleton(typeof(ILoadAssemblyDialog), typeof(LoadAssemblyDialog));            

            IoC.Instance.RegisterSingleton(typeof(IViewFor<ITestsTreeViewModel>), typeof(TestsTreeView));
            IoC.Instance.RegisterSingleton(typeof(ITestsTreeViewModel), typeof(TestsTreeViewModel));
            
            RxApp.DeferredScheduler = new DispatcherScheduler(Application.Current.Dispatcher);                      

            RxRouting.ViewModelToViewFunc = str =>
                {
                    var viewModelType =
                        AppDomain.CurrentDomain.GetAssemblies()
                                 .SelectMany(x => x.GetTypes())
                                 .FirstOrDefault(type => type.AssemblyQualifiedName.Equals(str));

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
                (realClass, iface, contract) => IoC.Instance.RegisterType(iface, realClass, contract));
        }
    }
}
