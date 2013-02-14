using NPerfRunner.ViewModels;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ReactiveUI.Routing;
using NPerfRunner.Wpf;
using NPerfRunner.Wpf.ViewModels;
using ReactiveUI.Xaml;
using System.Threading.Tasks;
using System.Reactive.Linq;

namespace NPerfRunner.Wpf
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IViewFor<MainWindowViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();

            InfrastructureInstaller.Install();
            this.SettingsHost.ViewModel = RxApp.GetService<ISettingsViewModel>();

            UserError.RegisterHandler((Func<UserError, IObservable<RecoveryOptionResult>>)IoC.Instance.Resolve<ErrorHandler>().HandleError);  
        }
        
        public MainWindowViewModel ViewModel
        {
            get { return (MainWindowViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(MainWindowViewModel), typeof(MainWindow), new PropertyMetadata(null));

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (MainWindowViewModel)value; }
        }
    }
}
