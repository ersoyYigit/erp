using ArdaManager.Application.Requests.Mail;
using System.Threading.Tasks;

namespace ArdaManager.Application.Interfaces.Services
{
    public interface IMailService
    {
        Task SendAsync(MailRequest request);
    }
}