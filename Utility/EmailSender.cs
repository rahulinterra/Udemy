using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Identity.UI.Services;
using Newtonsoft.Json.Linq;

namespace CRUD.Utility
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
           return Execute(email, subject, htmlMessage);
        }
        public async Task Execute (string email, string subject, string body)
        {
            MailjetClient client = new MailjetClient(Environment.GetEnvironmentVariable("e633c1aba54749961582b11cadabc478"), Environment.GetEnvironmentVariable("a161c5aa5447fcefadc84db90099429a"))
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
       new JObject {
        {"Email", "tellmerahul680@gmail.com"},
        {"Name", "Rahul"}
       }
      }, {
       "To",
       new JArray {
        new JObject {
         {
          "Email",
          email
         }, {
          "Name",
          "CRUD Project"
         }
        }
       }
      }, {
       "Subject",
      subject
      }
      , {
       "HTMLPart",
       body
      },
     }
             });
           await client.PostAsync(request);
        }
    }
}
