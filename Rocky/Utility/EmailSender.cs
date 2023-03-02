using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Identity.UI.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace Rocky.Utility
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Execute(email, subject, htmlMessage);
        }

        public async Task Execute(string email, string subject, string body)
        {
            MailjetClient client = new MailjetClient("14d9ed3dad954a3ada3073d3e8607a56", "d49a66f44f4865c8209093ac7311ab74")
            {
                Version = ApiVersion.V3_1,
            };
            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
             .Property(Send.Messages, new JArray {
                 new JObject {
                     {
                         "From",
                         new JObject 
                         {
                             {"Email", "sanya.kvitko.1347@gmail.com"},
                             {"Name", "Alex"}
                         }
                     }, 
                     {
                         "To",
                         new JArray 
                         {
                             new JObject 
                             {
                                 {
                                     "Email",
                                     email
                                 }, 
                                 {
                                     "Name",
                                     "DotNetMastery"
                                 }
                             }
                         }
                     }, 
                     {
                         "Subject",
                         subject
                     }, 
                     {
                         "HTMLPart",
                         body
                     }
                 }
             });
            MailjetResponse response = await client.PostAsync(request);
        }
    }
}
