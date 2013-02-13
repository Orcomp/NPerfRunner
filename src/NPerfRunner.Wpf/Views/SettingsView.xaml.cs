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
using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI.Routing;
using System.Reactive.Disposables;
using ReactiveUI.Xaml;
using Microsoft.Win32;
using System.Reflection;

namespace NPerfRunner.Views
{
    /// <summary>
    /// Логика взаимодействия для SettingsView.xaml
    /// </summary>
    public partial class SettingsView : UserControl, IViewFor<ISettingsViewModel>
    {
        private OpenFileDialog openFileDialog;

        public SettingsView()
        {
            InitializeComponent();
            DataContext = RxApp.GetService<ISettingsViewModel>();
        }

        public ISettingsViewModel ViewModel
        {
            get { return (ISettingsViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(ISettingsViewModel), typeof(SettingsView), new PropertyMetadata(null));

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (ISettingsViewModel)value; }
        }
    }
}
