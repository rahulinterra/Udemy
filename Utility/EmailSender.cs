using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Identity.UI.Services;
using Newtonsoft.Json.Linq;

namespace CRUD.Utility
{
   
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;
        public MailJetSettings _mailjetsettings { get; set; }
        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Execute(email, subject, htmlMessage);
        }
        public async Task Execute(string email, string subject, string body)
        {
            _mailjetsettings = _configuration.GetSection("MailJet").Get<MailJetSettings>();
            MailjetClient client = new MailjetClient(_mailjetsettings.ApiKey, _mailjetsettings.SecretKey)
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
        {"Email", "rahulsharma09072000@gmail.com"},
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
          "Rahul"
         }
        }
       }
      }, {
       "Subject",
       subject
      },  {
       "HTMLPart",
       body
      },
     }
      
     
             });
           await client.PostAsync(request);
        }
    }
}