using Amazon.SimpleEmail.Model;
using PensionFund.Domain.Constants;
using PensionFund.Domain.Exceptions;
using PensionFund.Infrastructure.Interfaces.Clients;
using PensionFund.Infrastructure.Interfaces.Repositories;
using Serilog;
using System.Net;

namespace PensionFund.Infrastructure.Repositories
{
    public class EmailRepository : IEmailRepository
    {
        private readonly IEmailClient _emailClient;
        public EmailRepository(IEmailClient emailClient)
        {
            _emailClient = emailClient;
        }

        public async Task SendEmailNotification(string email)
        {
            try
            {
                var connection = await _emailClient.GetConnection();
                var sendEmailRequest = new SendEmailRequest
                {
                    Source = EmailConstants.SOURCE,
                    Destination = new Destination { ToAddresses = { email } },
                    Message = new Message
                    {
                        Body = new Body
                        {
                            Html = new Content { Data = EmailConstants.BODY },
                            Text = new Content { Data = EmailConstants.BODY }
                        },
                        Subject = new Content { Data = EmailConstants.SUBJECT },

                    }
                };
                var response = await connection.SendEmailAsync(sendEmailRequest);
                if (response.HttpStatusCode != HttpStatusCode.OK)
                {
                    Log.Error($"Notification send - StatusCode: {response.HttpStatusCode} - To: {email}");
                    throw new NotificationException($"Notification send - StatusCode: {response.HttpStatusCode} - To: {email}");
                }
            }
            catch (NotificationException)
            {
                throw;
            }
            catch (Exception)
            {
                Log.Error("There was an error when trying to send email");
                throw;
            }
        }
    }
}
