using WebShop.Models;

namespace WebShop.Abastract
{
    public interface ISmtpEmailService
    {
        void Send(Message message);
    }
}
