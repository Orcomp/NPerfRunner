namespace NPerfRunner.Wpf.ViewModels
{
    using System.Windows;
    using System.Windows.Media;

    using NPerfRunner.ViewModels;

    using ReactiveUI;

    public abstract class PaneViewModel : ReactiveObject, IPaneViewModel
    {
        public ImageSource IconSource
        {
            get;
            protected set;
        }

        #region Title

        private string title;

        public string Title
        {
            get
            {
                return this.title;
            }

            set
            {
                this.RaiseAndSetIfChanged(x => x.Title, ref this.title, value);
            }
        }

        #endregion // Title

        #region ContentId

        private string contentId;

        public string ContentId
        {
            get
            {
                return contentId;
            }

            set
            {
                this.RaiseAndSetIfChanged(x => x.ContentId, ref this.contentId, value);
            }
        }

        #endregion // ContentId


        public abstract FrameworkElement View
        {
             get;
        }
    }
}
