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
using System.Windows.Shapes;

namespace NPerfRunner.Wpf.Views
{
    using NPerf.Lab;

    /// <summary>
    /// Interaction logic for ConfigurationWindow.xaml
    /// </summary>
    public partial class ConfigurationWindow : Window
    {
        public PerfTestConfiguration PerfTestConfiguration { get; set; }
        public PerfTestConfiguration PerfTestConfigurationTemp { get; set; }

        public TestsTreeView TestsTreeView { get; set; }

        public ConfigurationWindow(TestsTreeView view)
        {
            InitializeComponent();
            this.TestsTreeView = view;
        }

        public void SetPerfTestConfiguration(PerfTestConfiguration configuration)
        {
            PerfTestConfigurationTemp = configuration;
            PerfTestConfiguration = configuration.Clone();
            _propertyGrid.SelectedObject = PerfTestConfiguration;
        }

        private void Save_Button_Click(object sender, RoutedEventArgs e)
        {
            this.TestsTreeView.SavePerfTestConfigurationToSettings(PerfTestConfiguration);
            this.PerfTestConfigurationTemp.Copy(this.PerfTestConfiguration);
            this.Close();
        }

        private void Close_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
