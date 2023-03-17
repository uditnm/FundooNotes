using Experimental.System.Messaging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace CommonLayer.Models
{
    public class MSMQ
    {
        MessageQueue messageQ = new MessageQueue();

        public void sendData2Queue(string token)
        {

            messageQ.Path = @".\private$\FundooNotes";

            if(!MessageQueue.Exists(messageQ.Path))
{
                MessageQueue.Create(messageQ.Path);
            }
           

            messageQ.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });
            messageQ.ReceiveCompleted += MessageQ_ReceiveCompleted;
            messageQ.Send(token);
            messageQ.BeginReceive();
            messageQ.Close();
        }

        private void MessageQ_ReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
        {
            try
            {
                var msg = messageQ.EndReceive(e.AsyncResult);
                string token = msg.Body.ToString();
                string subject = "FundooNotes Reset Link";
                string body = $"Fundoo Notes Reset Password: <a href=http://localhost:4200/resetPassword/{token}> Click Here</a>";
                var smtp = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("udittesting123@gmail.com", "hdjfngklmacktbsu"),
                    EnableSsl = true,
                };

                smtp.Send("udittesting123@gmail.com", "udittesting123@gmail.com", subject, body);
                messageQ.BeginReceive();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
