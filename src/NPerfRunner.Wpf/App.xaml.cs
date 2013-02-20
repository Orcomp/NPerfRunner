namespace NPerfRunner
{
    using System;
    using System.Windows;

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
        }
    }
}
