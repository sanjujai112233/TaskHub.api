using System;
using System.Net;
using System.Net.Mail;

namespace TaskHub.api.Services;

public class EmailServices: IEmailServices
{
    private readonly IConfiguration _config;

    public EmailServices(IConfiguration config)
    {
        _config = config;
    }

    public async Task SendEmailAsync(string to, string subject,string body)
    {
        var smtpClient = new SmtpClient(_config["Email:SmtpServer"])
        {
          Port = int.Parse(_config["Email:Port"]),
          Credentials = new NetworkCredential(
            _config["Email:Username"],
            _config["Email:Password"]
          ),
          EnableSsl = true
        };
        

         var mail = new MailMessage(
            _config["Email:From"],
            to,
            subject,
            body
        );
        
        await smtpClient.SendMailAsync(mail);
    }

}
