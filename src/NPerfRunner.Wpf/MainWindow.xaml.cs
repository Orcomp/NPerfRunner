namespace NPerfRunner.Wpf
{
    using System;
    using System.Windows;
    using NPerfRunner.ViewModels;
    using NPerfRunner.Wpf.ViewModels;
    using ReactiveUI;
    using ReactiveUI.Xaml;
    using Xceed.Wpf.AvalonDock;

    public partial class MainWindow : Window, IViewFor<IMainWindowViewModel>
    {
        public MainWindow()
        {
            this.InitializeComponent();
            ViewModel = RxApp.GetService<IMainWindowViewModel>();
            this.DataContext = ViewModel;
            this.BindCommand(ViewModel, vm => vm.DocClosed, v => v.dockManager, "DocumentClosed");
        }

        public IMainWindowViewModel ViewModel
        {
            get { return (IMainWindowViewModel)this.GetValue(ViewModelProperty); }
            set { this.SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel", typeof(IMainWindowViewModel), typeof(MainWindow), new PropertyMetadata(null));

        object IViewFor.ViewModel
        {
            get { return this.ViewModel; }
            set { this.ViewModel = (IMainWindowViewModel)value; }
        }

       
    }
}
