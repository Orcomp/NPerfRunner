namespace NPerfRunner.Wpf.Views
{
    using System.Windows;
    using System.Windows.Controls;
    using NPerfRunner.ViewModels;
    using ReactiveUI;

    /// <summary>
    /// Логика взаимодействия для SettingsView.xaml
    /// </summary>
    public partial class SettingsView : UserControl, IViewFor<ISettingsViewModel>
    {
        public SettingsView()
        {
            this.InitializeComponent();
            this.DataContext = RxApp.GetService<ISettingsViewModel>();
        }

        public ISettingsViewModel ViewModel
        {
            get { return (ISettingsViewModel)this.GetValue(ViewModelProperty); }
            set { this.SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(ISettingsViewModel), typeof(SettingsView), new PropertyMetadata(null));

        object IViewFor.ViewModel
        {
            get { return this.ViewModel; }
            set { this.ViewModel = (ISettingsViewModel)value; }
        }
    }
}
