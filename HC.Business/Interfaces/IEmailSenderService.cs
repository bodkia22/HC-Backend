using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SendGrid;

namespace HC.Business.Interfaces
{
    public interface IEmailSenderService
    {
        public Task SendEmailAsync(string email, string subject, string message);
        public Task<Response> Execute(string apiKey, string subject, string message, string email);
    }
}
