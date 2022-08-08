using System;
using System.Windows;
using PrismApp.Resources;

namespace PrismApp
{
    public static class AppUnhandledException
    {
        public static void HandlingUnhandledExceptions()
        {
            // Catch exceptions from all threads in the AppDomain.
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
                ShowUnhandledException(args.ExceptionObject as Exception, ExceptionRes.AppDomainException, true);

            // Catch exceptions from a single specific UI dispatcher thread.
            Application.Current.Dispatcher.UnhandledException += (sender, args) =>
            {
                args.Handled = true;
                ShowUnhandledException(args.Exception, ExceptionRes.DispatcherUnhandledException, false);
            };
        }

        private static void ShowUnhandledException(Exception exception, string unhandledExceptionType, bool promptUserForShutdown)
        {
            var messageBoxTitle = string.Format(ExceptionRes.UnExpectedError, unhandledExceptionType);
            var messageBoxMessage = string.Format(ExceptionRes.ExceptionOccurred, exception);
            var messageBoxButtons = MessageBoxButton.OK;

            if (promptUserForShutdown)
            {
                messageBoxMessage += Global.AppWillDie;
                messageBoxButtons = MessageBoxButton.YesNo;
            }

            // Let the user decide if the app should die or not (if applicable).
            if (MessageBox.Show(messageBoxMessage, messageBoxTitle, messageBoxButtons) == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }
    }
}