using MailKit.Net.Smtp;
using MailService.Models.Dtos;
using MimeKit;

namespace MailService.Services
{
        public class MailsService   //deals with sending emails
        {
            private readonly string _email;
            private readonly string _password;
            private readonly IConfiguration _configuration;

            public MailsService(IConfiguration configuration)
            {
                _configuration = configuration;
                _email = _configuration.GetValue<string>("EmailSettings:Email");
                _password = _configuration.GetValue<string>("EmailSettings:Password");
            }

            public async Task sendEmail(UserMessageDto user, string Message, string? subject = "Welcome to Suzie's Ecommerce App") //q
            {
                MimeMessage message1 = new MimeMessage();

                message1.From.Add(new MailboxAddress("My blogs ", _email));  //where email is coming from

                message1.To.Add(new MailboxAddress(user.Name, user.Email)); //where email is going to. We'll read this from the queue

                message1.Subject = subject;

                var body = new TextPart("html") // string Message q will be of type html
                {
                    Text = Message.ToString()
                };

                message1.Body = body;


                ///

                var client = new SmtpClient();  //install MailKit to use SmtpClient
                client.Connect("smtp.gmail.com", 587, false);  //server for gmail

                client.Authenticate(_email, _password);  //authenticate using email andpassword passed in appsettings

                await client.SendAsync(message1);

                await client.DisconnectAsync(true);  //similar to dispose
            }
        }
    }
