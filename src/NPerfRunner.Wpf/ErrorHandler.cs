namespace NPerfRunner.Wpf
{
    using System;
    using System.Reactive.Linq;
    using System.Text;
    using System.Windows;
    using NPerf.Core.Info;
    using NPerf.Core.PerfTestResults;
    using ReactiveUI.Xaml;

    internal class ErrorHandler
    {
        public IDisposable HandleErrors(IReactiveCommand command)
        {
            return command.ThrownExceptions.SelectMany(ex => UserError.Throw(ex.Message, ex.InnerException))
                .Subscribe(result =>
                    {
                        switch (result)
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

        public void ReportException(Exception ex)
        {
            new ExceptionReporting.ExceptionReporter().Show(ex);
        }

        public void ReportExperimentError(ExperimentError error, TestInfo testInfo)
        {
            var errorMessage = new StringBuilder()
                .AppendFormat("Error in {0} for {1}:", testInfo.Suite.TestSuiteDescription, testInfo.TestedType)
                .AppendLine().AppendFormat("    {0}: {1}", testInfo.Suite.FeatureDescription, error.Descriptor)
                .AppendLine().AppendFormat("    {0}", error.ExceptionType)
                .AppendLine().AppendFormat("    {0}", error.Message);

            MessageBox.Show(errorMessage.ToString(),"Error in test", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
