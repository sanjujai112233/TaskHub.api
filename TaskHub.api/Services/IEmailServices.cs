using System;

namespace TaskHub.api.Services;

public interface IEmailServices
{
    Task SendEmailAsync(string to, string subject, string body);
}
