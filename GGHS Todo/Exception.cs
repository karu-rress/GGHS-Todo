using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using Windows.UI.Popups;
using Thrd = System.Threading.Tasks;


namespace GGHS_Todo
{
    static class SmtpExtension
    {
        public static Thrd.Task SendAsync(this SmtpClient smtp, MailMessage msg) => Thrd.Task.Run(() => smtp.Send(msg));
    }

    class ExceptionHandler
    {
        public static async void HandleException(object sender, Windows.UI.Xaml.UnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            var exception = e.Exception;
            string errorMsg = $"에러가 발생했습니다. \n{exception}";
            var smtp = PrepareSendMail(errorMsg,
                $"GGHS Todo EXCEPTION OCCURED in V{MainPage.Version}", out var msg);

            try
            {
                await smtp.SendAsync(msg);
            }
            catch
            {
                MessageDialog messageDialog = new(errorMsg, "An error has occured.");
                await messageDialog.ShowAsync();
            }
        }

        public static SmtpClient PrepareSendMail(string body, string subject, out MailMessage msg)
        {
            MailAddress send = new("rollingress1388@gmail.com");
            MailAddress to = new("nsun527@naver.com");
            SmtpClient smtp = new()
            {
                Host = "smtp.gmail.com",
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(send.Address, "hxfmthqwllaqccxv"),
                Timeout = 20_000
            };
            msg = new(send, to)
            {
                Subject = subject,
                Body = body
            };
            return smtp;
        }
    }
}
