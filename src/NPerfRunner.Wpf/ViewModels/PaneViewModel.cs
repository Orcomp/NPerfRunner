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

        #region IsSelected

        private bool isSelected;

        public bool IsSelected
        {
            get
            {
                return this.isSelected;
            }

            set
            {
                this.RaiseAndSetIfChanged(x => x.IsSelected, ref this.isSelected, value);
            }
        }

        #endregion // IsSelected

        #region IsActive

        private bool isActive;

        public bool IsActive
        {
            get
            {
                return this.isActive;
            }

            set
            {
                this.RaiseAndSetIfChanged(x => x.IsActive, ref this.isActive, value);
            }
        }

        #endregion // IsActive

        #region IsVisible

        private bool isVisible;

        public bool IsVisible
        {
            get
            {
                return this.isVisible;
            }

            set
            {
                this.RaiseAndSetIfChanged(x => x.IsVisible, ref this.isVisible, value);
            }
        }

        #endregion // IsVisible
        public abstract FrameworkElement View
        {
             get;
        }
    }
}
