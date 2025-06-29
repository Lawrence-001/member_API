namespace MemberTestAPI.Repositories.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmail(string recipient, string subject, string body);
    }
}
