namespace Lab2.NotificationSystems;

public class TextNotificationSystem : INotificationSystem
{
    private readonly string _message;

    public TextNotificationSystem(string message = "ALERT: Suspicious content detected!")
    {
        _message = message;
    }

    public void Notify()
    {
        Console.WriteLine(_message);
    }
}