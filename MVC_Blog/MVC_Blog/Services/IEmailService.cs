using System.Threading.Tasks;

namespace MVC_Blog.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
