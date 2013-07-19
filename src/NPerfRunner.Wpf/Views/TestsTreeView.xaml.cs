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
    /// Логика взаимодействия для TestsView.xaml
    /// </summary>
    public partial class TestsTreeView : UserControl, IViewFor<ITestsTreeViewModel>
    {
        public TestsTreeView()
        {
            this.InitializeComponent();
            this.DataContext = RxApp.GetService<ITestsTreeViewModel>();
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
    }
}
