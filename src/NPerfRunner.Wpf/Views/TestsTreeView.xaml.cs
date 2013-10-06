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

namespace NPerfRunner.Wpf.Views
{
    using NPerf.Lab;

    using NPerfRunner.ViewModels;
    using NPerfRunner.Wpf.Properties;

    using ReactiveUI;

    /// <summary>
    /// Логика взаимодействия для TestsView.xaml
    /// </summary>
    public partial class TestsTreeView : UserControl, IViewFor<ITestsTreeViewModel>
    {
        public PerfTestConfiguration PerfTestConfiguration { get; set; }

        public TestsTreeView()
        {
            this.InitializeComponent();
            this.DataContext = RxApp.GetService<ITestsTreeViewModel>();
            this.PerfTestConfiguration = this.GetPerfTestConfigurationFromSettings();
        }

        public ITestsTreeViewModel ViewModel
        {
            get { return (ITestsTreeViewModel)this.GetValue(ViewModelProperty); }
            set { this.SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(ITestsTreeViewModel), typeof(TestsTreeView), new PropertyMetadata(null));

        object IViewFor.ViewModel
        {
            get { return this.ViewModel; }
            set { this.ViewModel = (ITestsTreeViewModel)value; }
        }

        private void LaunchConfigurationWindow(object sender, RoutedEventArgs e)
        {
            var configWindow = new ConfigurationWindow(this);
            configWindow.SetPerfTestConfiguration(PerfTestConfiguration);
            LaunchConfigurationButton.IsEnabled = false;
            configWindow.Closed += this.OnClosedConfigurationWindow;
            configWindow.Show();
        }

        public void OnClosedConfigurationWindow(object sender, EventArgs e)
        {
            LaunchConfigurationButton.IsEnabled = true;
        }

        public void SavePerfTestConfigurationToSettings(PerfTestConfiguration configuration)
        {
            Settings.Default.IgnoreFirstRunDueToJITting = configuration.IgnoreFirstRunDueToJITting;
            Settings.Default.TriggerGCBeforeEachTest = configuration.TriggerGCBeforeEachTest;
            Settings.Default.Save();
        }

        public PerfTestConfiguration GetPerfTestConfigurationFromSettings()
        {
            return new PerfTestConfiguration(Settings.Default.IgnoreFirstRunDueToJITting, Settings.Default.TriggerGCBeforeEachTest);
        }
    }

}
