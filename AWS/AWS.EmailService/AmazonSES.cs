using Amazon;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AWS.EmailService
{
    public class AmazonSES : IAmazonSES
    {
        // Replace sender@example.com with your "From" address.
        // This address must be verified with Amazon SES.
        static readonly string senderAddress = "rohit.rustagi@daffodilsw.com";

        // Replace recipient@example.com with a "To" address. If your account
        // is still in the sandbox, this address must be verified.
        static readonly string receiverAddress = "sourabh.rustagi@daffodilsw.com";

        // The configuration set to use for this email. If you do not want to use a
        // configuration set, comment out the following property and the
        // ConfigurationSetName = configSet argument below. 
        static readonly string configSet = "TestConfig";

        // The subject line for the email.
        static readonly string Subject = "Amazon SES test (AWS SDK for .NET)";

        // The email body for recipients with non-HTML email clients.
        static readonly string TextBody = "Amazon SES Test (.NET)\r\n"
                                        + "This email was sent through Amazon SES "
                                        + "using the AWS SDK for .NET.";

        // The HTML body of the email.
        static readonly string HtmlBody = @"<html>
                                            <head></head>
                                            <body>
                                              <h1>Amazon SES Test (AWS SDK for .NET)</h1>
                                              <p>This email was sent with
                                                <a href='https://aws.amazon.com/ses/'>Amazon SES</a> using the
                                                <a href='https://aws.amazon.com/sdk-for-net/'>
                                                  AWS SDK for .NET</a>.</p>
                                            </body>
                                            </html>";



        public async Task SendEmailAsync(string subject, string htmlBody, string textBody)
        {
            // Replace USWest2 with the AWS Region you're using for Amazon SES.
            // Acceptable values are EUWest1, USEast1, and USWest2.
            using (var client = new AmazonSimpleEmailServiceClient(RegionEndpoint.USEast1))
            {
                var sendRequest = new SendEmailRequest
                {
                    Source = senderAddress,
                    Destination = new Destination
                    {
                        ToAddresses =
                        new List<string> { receiverAddress }
                    },
                    Message = new Message
                    {
                        Subject = new Content(subject),
                        Body = new Body
                        {
                            Html = new Content
                            {
                                Charset = "UTF-8",
                                Data = htmlBody
                            },
                            Text = new Content
                            {
                                Charset = "UTF-8",
                                Data = textBody
                            }
                        }
                    },
                    // If you are not using a configuration set, comment
                    // or remove the following line 
                    //ConfigurationSetName = configSet
                };
                try
                {
                    Console.WriteLine("Sending email using Amazon SES...");
                    var response = await client.SendEmailAsync(sendRequest);
                    Console.WriteLine("The email was sent successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("The email was not sent.");
                    Console.WriteLine("Error message: " + ex.Message);

                }
            }

        }

    }
}
