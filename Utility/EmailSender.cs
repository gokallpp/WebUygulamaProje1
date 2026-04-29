using Microsoft.AspNetCore.Identity.UI.Services;

namespace WebUygulamaProje1.Utility
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // Burada gerçek bir e-posta gönderme işlemi buraya ekleyip gerçekleştirebilirsiniz.
            return Task.CompletedTask;
        }
    }
}
