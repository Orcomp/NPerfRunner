using NPerfRunner.ViewModels;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPerfRunner.Wpf.ViewModels
{
    public abstract class TreeViewItemViewModel : ReactiveObject, ITreeViewItemViewModel
    {
        private readonly TreeViewItemViewModel parent;

        public IReactiveCollection<ITreeViewItemViewModel> Children
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
                this.SetChechedState(this, value ?? false);
            }
        }

        protected TreeViewItemViewModel(TreeViewItemViewModel parent)
        {            
            this.parent = parent;
            this.Children = new ReactiveCollection<ITreeViewItemViewModel>();
            this.IsChecked = false;
        }

        private void SetChechedState(TreeViewItemViewModel sender, bool value)
        {
            foreach (TreeViewItemViewModel child in this.Children)
            {
                child.SetChechedState(sender, value);
            }

            this.RaiseAndSetIfChanged(x => x.IsChecked, ref this.isChecked, value);

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
                this.RaiseAndSetIfChanged(x => x.IsChecked, ref this.isChecked, true);
            }

            if (notCheckedCount == totalCount)
            {
                this.RaiseAndSetIfChanged(x => x.IsChecked, ref this.isChecked, false);
            }

            if (totalCount != 0 && checkedCount < totalCount && notCheckedCount < totalCount)
            {
                this.RaiseAndSetIfChanged(x => x.IsChecked, ref this.isChecked, null);
            }

            if (parent != null)
            {
                parent.UpdateCheckedState();
            }
        }
    }
}
