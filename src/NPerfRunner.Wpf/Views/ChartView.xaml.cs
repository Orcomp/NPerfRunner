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
    using NPerfRunner.ViewModels;

    using ReactiveUI;

    /// <summary>
    /// Логика взаимодействия для ChartView.xaml
    /// </summary>
    public partial class ChartView : UserControl, IViewFor<IChartViewModel>
    {
        public ChartView()
        {
            InitializeComponent();                   
        }

        public void BindViewModel(IChartViewModel viewModel)
        {
            DataContext = viewModel;
            
        }

        public IChartViewModel ViewModel
        {
            get { return (IChartViewModel)this.GetValue(ViewModelProperty); }
            set { this.SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(IChartViewModel), typeof(ChartView), new PropertyMetadata(null));

        object IViewFor.ViewModel
        {
            get { return this.ViewModel; }
            set { this.ViewModel = (IChartViewModel)value; }
        }
    }
}
