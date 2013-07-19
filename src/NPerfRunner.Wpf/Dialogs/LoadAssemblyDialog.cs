namespace NPerfRunner.Wpf.Dialogs
{
    using System;
    using System.Reactive.Linq;
    using System.Reflection;
    using Microsoft.Win32;
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
                var newFile = Environment.CurrentDirectory + "\\" + file.Name;
                try
                {
                    // TODO: think how to make this more elegant
                    File.Copy(this.dialog.FileName, newFile, true);
                }
                catch (IOException)
                {
                }

                result = Assembly.LoadFile(newFile);
            }

            return result;
        }
    }
}
