namespace NPerfRunner.Dialogs
{
    using System;
    using System.Reflection;

    public interface ILoadAssemblyDialog
    {
        Assembly LoadAssembly(string dialogTitle);
    }
}
