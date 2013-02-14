using ReactiveUI;
using ReactiveUI.Routing;
using ReactiveUI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using NPerfRunner.ViewModels;
using NPerfRunner.Dialogs;
using System.Threading.Tasks;
using System.Reactive.Linq;

namespace NPerfRunner.Wpf.ViewModels
{
    public class SettingsViewModel : ReactiveObject, ISettingsViewModel
    {
        public ReactiveAsyncCommand LoadTool { get; protected set; }
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
            
            LoadTool = new ReactiveAsyncCommand();

            LoadTool.RegisterAsyncAction(OnLoadTool, RxApp.DeferredScheduler);
            IoC.Instance.Resolve<ErrorHandler>().HandleErrors(LoadTool);
        }

        private void OnLoadTool(object param)
        {
            IoC.Instance.Resolve<ILoadAssemblyDialog>().LoadAssembly();
        }
    }
}
