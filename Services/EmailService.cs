using Mailjet.Client;
using Mailjet.Client.TransactionalEmails;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using webApplication.DTO.Account;

namespace webApplication.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration config)
        {
            _configuration = config;
        }

        public async Task<bool> SendEmailAsync(EmailSendDTO sendDTO)
        {
            try
            {
                // Use _configuration instead of config
                var apiKey = _configuration["MailJet:ApiKey"];
                var secretKey = _configuration["MailJet:SecretKey"]; // Fixed typo: SecertKey → SecretKey
                var fromEmail = _configuration["Email:From"];
                var applicationName = _configuration["Email:ApplicationName"];

                // Validate required configuration
                if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(secretKey))
                {
                    throw new System.Exception("MailJet configuration is missing");
                }

                MailjetClient client = new MailjetClient(apiKey, secretKey);

                var email = new TransactionalEmailBuilder()
                    .WithFrom(new SendContact(fromEmail, applicationName))
                    .WithSubject(sendDTO.Subject)
                    .WithHtmlPart(sendDTO.Body)
                    .WithTo(new SendContact(sendDTO.To)) // Assuming sendDTO.To is email address
                    .Build();

                var response = await client.SendTransactionalEmailAsync(email);

                // Better response checking
                if (response.Messages != null && response.Messages.Length > 0)
                {
                    return response.Messages[0].Status == "success";
                }

                return false;
            }
            catch (System.Exception ex)
            {
                // Log the exception here
                // You might want to inject ILogger<EmailService> for proper logging
                System.Console.WriteLine($"Email sending failed: {ex.Message}");
                return false;
            }
        }
    }
}