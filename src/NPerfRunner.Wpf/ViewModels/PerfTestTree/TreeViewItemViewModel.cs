namespace NPerfRunner.Wpf.ViewModels.PerfTestTree
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using NPerfRunner.ViewModels;
    using NPerfRunner.Wpf.Messages;

    using ReactiveUI;

    public abstract class TreeViewItemViewModel : ReactiveObject
    {
        private readonly TreeViewItemViewModel parent;

        public ReactiveCollection<TreeViewItemViewModel> Children
        {
            get;
            private set;
        }

        private string name;
        public string Name
        {
            get { return this.name; }
            protected set { this.RaiseAndSetIfChanged(x => x.Name, ref this.name, value); }
        }

        private bool? isChecked;
        public bool? IsChecked
        {
            get { return isChecked; }
            set 
            {
                this.SetChangedState(this, value ?? false);
            }
        }

        protected TreeViewItemViewModel(TreeViewItemViewModel parent)
        {            
            this.parent = parent;
            this.Children = new ReactiveCollection<TreeViewItemViewModel>();
          //  this.IsChecked = false;
        }

        private void SetChangedState(TreeViewItemViewModel sender, bool value)
        {
            foreach (var child in this.Children)
            {
                child.SetChangedState(sender, value);
            }

            // this.RaiseAndSetIfChanged(x => x.IsChecked, ref this.isChecked, value);
            this.SetIsChanged(value);

            if (parent != null && object.ReferenceEquals(this, sender))
            {
                parent.UpdateCheckedState();
            }
        }

        private void UpdateCheckedState()
        {
            int checkedCount = this.Children.Count(x => x.IsChecked != null && x.IsChecked.Value);
            int notCheckedCount = this.Children.Count(x => x.IsChecked != null && !x.IsChecked.Value);
            int totalCount = this.Children.Count();

            if (totalCount != 0 && checkedCount == totalCount)
            {
                this.SetIsChanged(true);
            }

            if (notCheckedCount == totalCount)
            {
                this.SetIsChanged(false);
            }

            if (totalCount != 0 && checkedCount < totalCount && notCheckedCount < totalCount)
            {
                this.SetIsChanged(null);
            }

            if (parent != null)
            {
                parent.UpdateCheckedState();
            }
        }

        protected void SetIsChanged(bool? value)
        {
            var shouldRaise = this.IsChecked != value;
            this.RaiseAndSetIfChanged(x => x.IsChecked, ref this.isChecked, value);
            if (shouldRaise)
            {
                MessageBus.Current.SendMessage(new TestCheckChanged(this)/*this.perfLab, this.testInfo, this.IsChecked)*/);
            }
        }
    }
}
