namespace NPerfRunner.Wpf.Dialogs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using Microsoft.Win32;
    using NPerfRunner.Dialogs;

    public class LoadAssemblyDialog : ILoadAssemblyDialog
    {
        private readonly OpenFileDialog dialog;

        public LoadAssemblyDialog()
        {
            this.dialog = new OpenFileDialog();
        }

        public Assembly LoadAssembly()
        {
            if (!this.dialog.ShowDialog().Value)
            {
                return null;
            }

            return Assembly.LoadFile(this.dialog.FileName);
        }
    }
}
