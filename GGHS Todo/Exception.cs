using RollingRess;
using Windows.UI.Xaml;

namespace GGHS_Todo
{
    internal static class ExceptionHandler
    {
        public static async void HandleException(object _, UnhandledExceptionEventArgs e)
        {
            e.Handled = true;

            RollingMailSender mail = new();
            mail.RegisterException("GGHS Todo", MainPage.Version, e.Exception);
            await mail.Send();
        }
    }
}
