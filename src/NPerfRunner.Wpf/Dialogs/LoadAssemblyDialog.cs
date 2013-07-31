namespace NPerfRunner.Wpf.Dialogs
{
    using System;
    using System.Reactive.Linq;
    using System.Reflection;
    using Microsoft.Win32;

    using NPerf.Core.Tools;

    using NPerfRunner.Dialogs;
    using System.IO;

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
            Assembly result = null;
            if (this.dialog.ShowDialog().Value)
            {
                var file = new FileInfo(this.dialog.FileName);
                result = AssembliesManager.LoadAssembly(this.dialog.FileName);
            }

            return result;
        }
    }
}
