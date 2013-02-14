namespace NPerfRunner.Wpf
{
    using System;
    using System.Windows;
    using NPerfRunner.ViewModels;
    using NPerfRunner.Wpf.ViewModels;
    using ReactiveUI;
    using ReactiveUI.Xaml;

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IViewFor<MainWindowViewModel>
    {
        public MainWindow()
        {
            this.InitializeComponent();

            InfrastructureInstaller.Install();
            this.SettingsHost.ViewModel = RxApp.GetService<ISettingsViewModel>();

            UserError.RegisterHandler((Func<UserError, IObservable<RecoveryOptionResult>>)IoC.Instance.Resolve<ErrorHandler>().HandleError);  
        }
        
        public MainWindowViewModel ViewModel
        {
            get { return (MainWindowViewModel)this.GetValue(ViewModelProperty); }
            set { this.SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel", typeof(MainWindowViewModel), typeof(MainWindow), new PropertyMetadata(null));

        object IViewFor.ViewModel
        {
            get { return this.ViewModel; }
            set { this.ViewModel = (MainWindowViewModel)value; }
        }
    }
}
