namespace NPerfRunner.Wpf.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Media;

    using ReactiveUI;
    using NPerfRunner.ViewModels;

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
                return this.contentId;
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
    }
}
