namespace NPerfRunner.Wpf.Dialogs
{
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

        public Assembly LoadAssembly()
        {
            return !this.dialog.ShowDialog().Value ? null : Assembly.LoadFile(this.dialog.FileName);
        }
    }
}
