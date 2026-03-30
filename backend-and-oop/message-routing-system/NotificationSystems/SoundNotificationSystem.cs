namespace Lab2.NotificationSystems;

public class SoundNotificationSystem : INotificationSystem
{
    public void Notify()
    {
        Console.Beep();
        Console.WriteLine("🔊 BEEP! (Sound notification)");
    }
}