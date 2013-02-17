namespace NPerfRunner.Wpf.Dialogs
{
    using System;
    using System.Reactive.Linq;
    using System.Reflection;
    using Microsoft.Win32;
    using NPerfRunner.Dialogs;

    public class LoadAssemblyDialog : ILoadAssemblyDialog
    {
        private readonly OpenFileDialog dialog;

        public LoadAssemblyDialog()
        {
            this.dialog = new OpenFileDialog();
        }

        public Assembly LoadAssembly(string dialogTitle)
        {
            this.dialog.Title = dialogTitle;
            return !this.dialog.ShowDialog().Value ? null : Assembly.LoadFile(this.dialog.FileName);
        }
    }
}
