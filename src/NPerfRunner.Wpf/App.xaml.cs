namespace NPerfRunner
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Windows;

    using NPerf.Core.Tools;

    using NPerfRunner.Wpf;

    using ReactiveUI.Xaml;

    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {            
            InfrastructureInstaller.Install();
            UserError.RegisterHandler((Func<UserError, IObservable<RecoveryOptionResult>>)IoC.Instance.Resolve<ErrorHandler>().HandleError);
            base.OnStartup(e);
            AppDomain.CurrentDomain.AssemblyLoad += AssembliesManager.Loaded;
            AppDomain.CurrentDomain.AssemblyResolve += AssembliesManager.Resolve;

            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;

            if (ex == null)
            {
                return;
            }

            var errorHandler = IoC.Instance.Resolve<ErrorHandler>();
            errorHandler.ReportException(ex);
        }
    }
}
