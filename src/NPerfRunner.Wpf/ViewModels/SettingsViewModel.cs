using ReactiveUI;
using ReactiveUI.Routing;
using ReactiveUI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace NPerfRunner.ViewModels
{
    public class SettingsViewModel : ReactiveObject, ISettingsViewModel
    {
        public IReactiveCommand LoadTool { get; protected set; }
        public IReactiveCommand LoadSubject { get; protected set; }
        public IReactiveCommand StartTesting { get; protected set; }
        public IReactiveCommand StopTesting { get; protected set; }

        private string testerFileName;
        public string TesterFileName
        {
            get { return this.testerFileName; }
            set { this.RaiseAndSetIfChanged(x => x.TesterFileName, ref this.testerFileName, value); }
        }

        public IScreen HostScreen { get; protected set; }

        public string UrlPathSegment
        {
            get { return "settings"; }
        }

        public SettingsViewModel(IScreen screen)
        {
            
            HostScreen = screen;
            
            LoadTool = new ReactiveCommand();

            LoadTool.Subscribe(_ => MessageBox.Show(""));
        }
    }
}
