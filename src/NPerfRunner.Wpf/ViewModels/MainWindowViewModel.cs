namespace NPerfRunner.Wpf.ViewModels
{
    using ReactiveUI;
    using ReactiveUI.Routing;

    public class MainWindowViewModel : ReactiveObject, IScreen
    {

        public IRoutingState Router { get; private set; }

        public MainWindowViewModel()
        {
            this.Router = new RoutingState();
        }
    }
}
