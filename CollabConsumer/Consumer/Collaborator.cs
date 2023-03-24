using System.Net.Mail;
using System.Net.Sockets;
using System.Net;
using System.Threading.Tasks;
using MassTransit;
using CommonLayer.Models;
using Microsoft.Extensions.Configuration;

namespace Consumer.Collaborator
{
    public class Collaborator: IConsumer<CollabModel>
    {
        private string SenderMail, key;

        public Collaborator(IConfiguration config)
        {
            SenderMail = config["AccountSettings:Mail"];
            key = config["AccountSettings:Key"];
        }
        public async Task Consume(ConsumeContext<CollabModel> context)
        {
            var data = context.Message;
            string subject = "RabbitMQ Collaborator Confirmation";
            string body = $"{data.collabMail} has been added as a collaborator";
            var smtp = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(SenderMail, key),
                EnableSsl = true,
            };

            smtp.Send(SenderMail, data.collabMail, subject, body);
        }
    }
}
