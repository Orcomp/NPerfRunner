using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ReactiveUI.Xaml;

namespace NPerfRunner.Wpf
{
    internal class ErrorHandler
    {
        public IDisposable HandleErrors(ReactiveAsyncCommand command)
        {
            return command.ThrownExceptions.SelectMany(ex => UserError.Throw(ex.Message, ex.InnerException))
                .Subscribe(result =>
                {
                    switch(result)
                    {
                        case RecoveryOptionResult.RetryOperation:
                            command.Execute(null);
                            break;
                        case RecoveryOptionResult.FailOperation:
                            Application.Current.Shutdown();
                            break;
                    }
                });
        }

        public IObservable<RecoveryOptionResult> HandleError(UserError error)
        {
            var result = MessageBox.Show(error.ErrorMessage, "Error");
            return Observable.Return(result == MessageBoxResult.Yes ? RecoveryOptionResult.RetryOperation : RecoveryOptionResult.CancelOperation);
        }
    }
}
