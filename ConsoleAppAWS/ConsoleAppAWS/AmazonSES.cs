
using Amazon.SimpleEmailV2;
using Amazon.SimpleEmailV2.Model;

namespace ConsoleAppAWS
{
    public class AmazonSES
    {
        private readonly IAmazonSimpleEmailServiceV2 _sesClient;

        public AmazonSES()
        {
            _sesClient = new AmazonSimpleEmailServiceV2Client();
        }

        public async Task SendEmailAsync(string fromAddress, string toAddresses, string subject, string body)
        {
            var sendEmailRequest = new SendEmailRequest
            {
                FromEmailAddress = fromAddress,                 
                ReplyToAddresses = new List<string> { toAddresses },
                Content = new EmailContent
                {
                    Simple = new Message
                    {
                        Subject = new Content
                        {
                            Charset = "UTF-8",
                            Data = subject
                        },
                        Body = new Body
                        {
                            Html = new Content
                            {
                                Charset = "UTF-8",
                                Data = body
                            }
                        }
                    }
                }
            };

            await _sesClient.SendEmailAsync(sendEmailRequest);
        }
    }
}