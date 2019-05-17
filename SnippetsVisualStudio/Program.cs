using System;
using System.ComponentModel.DataAnnotations;

namespace SnippetsVisualStudio
{
    class Program
    {
        /// <summary>
        /// Testing snippets to create
        /// https://www.youtube.com/watch?v=r1TQX2Uweno
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");



            var emailSenderSendGrid = new EmailSenderSendGrid("SG.8diV4ZC0------------SOME-KEY------------------8PoU");
            emailSenderSendGrid.SendEmailViaApi(
                emailSubject: "Testing sendgrid",
                plainTextContent: "plain text content test",
                htmlContent: "",
                senderEmail: "ginello@gmail.com",
                senderName: "Gino Solito Omini",
                receiverEmail: "jamesbond@gmail.com",
                receiverName: "James Bond"
                );


        }








        

    }


    /// <summary>
    /// I've tested it with gmail but had no success due to security issues raised by gmail servers
    /// https://stackoverflow.com/questions/29958229/send-email-using-gmail-error-the-server-response-was-5-5-1-authentication-req/34894275#34894275
    /// https://www.technical-recipes.com/2018/how-to-send-an-e-mail-via-google-smtp-using-c/
    /// https://stackoverflow.com/questions/52829188/sending-mail-through-c-sharp-with-gmail-is-not-working-after-deploying-to-host
    /// To enable sending emails from google remember to
    /// sign up Gmail and go to https://www.google.com/settings/security/lesssecureapps where you can see settings. Access for less secure apps
    /// Turn off (default) ==> Turn On
    /// 
    /// 
    /// Google's smtp--> smtp.gmail.com
    /// SmtpServer.Port = 25;
    /// Note: When using SSL the port needs to be 443
    /// 
    /// EXAMPLE CODE:
    /// -------------
    ///     var emailSender = new EmailSender("smtp.gmail.com", 443, "sender@gmail.com", "mypassword");
    ///     emailSender.SendEmail("sender@gmail.com", "destination@gmail.com", "test email", "email body");
    /// </summary>
    public class EmailSender
    {
        private string _smtp { get; set; }
        private int _port { get; set; }
        private string _userName { get; set; }
        private string _password { get; set; }


        public EmailSender(string smtp, int port, string username, string password)
        {
            this._smtp = smtp;
            this._port = port;
            this._userName = username;
            this._password = password;
        }





        public void SendEmail(string emailFrom, string emailTo, string subject, string body)
        {
            var client = new System.Net.Mail.SmtpClient(this._smtp, this._port)
            {
                Credentials = new System.Net.NetworkCredential(this._userName, this._password),
                EnableSsl = true,
                DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false
            };

            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object s,
                System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                System.Security.Cryptography.X509Certificates.X509Chain chain,
                System.Net.Security.SslPolicyErrors sslPolicyErrors)
            {
                return true;
            };



            client.Send(emailFrom, emailTo, subject, body);
        }







    }



    public class EmailSenderSendGrid
    {

        private string _apikey { get; set; }


        public EmailSenderSendGrid(string apikey)
        {
            this._apikey = apikey;
        }

        /// <summary>
        /// Install-Package SendGrid
        /// </summary>
        public void SendEmailViaApi(string emailSubject, string plainTextContent, string htmlContent, string senderEmail, string senderName, string receiverEmail, string receiverName)
        {
            var client = new SendGrid.SendGridClient(this._apikey);
            var from = new SendGrid.Helpers.Mail.EmailAddress(senderEmail, senderName);
            var to = new SendGrid.Helpers.Mail.EmailAddress(receiverEmail, receiverName);
            //var plainTextContent = "and easy to do anywhere, even with C#";
            //var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
            var msg = SendGrid.Helpers.Mail.MailHelper.CreateSingleEmail(from, to, emailSubject, plainTextContent, htmlContent);
            var response = client.SendEmailAsync(msg).GetAwaiter().GetResult();
        }
    }


    






}




