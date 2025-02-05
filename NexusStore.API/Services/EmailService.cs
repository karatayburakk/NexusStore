namespace NexusStore.API.Services
{

  public class EmailService(IUserService userService) : IEmailService
  {
    private readonly IUserService _userService = userService;

    public async Task SendDailyEmailAsync()
    {
      var users = await _userService.GetAllUsersAsync();
      foreach (var user in users)
      {
        // Implement your email sending logic here
        // For example, using SMTP or a third-party email service
        await SendEmailAsync(user.Email, "Daily Update", "Here is your daily update!");
      }
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
      // Implement your email sending logic here
      // For example, using SMTP or a third-party email service
      await Task.CompletedTask;
    }
  }
}