using Lab2.Archivers;
using Lab2.Core;
using Lab2.Decorators;
using Lab2.Formatters;
using Lab2.Logging;
using Lab2.NotificationSystems;
using Lab2.Recipients;
using Lab2.ValueObjects;

namespace Lab2;

internal class Program
{
    private static void Main()
    {
        Console.WriteLine("Corporate Message Distribution System");
        Console.WriteLine();

        var alice = new User("Alice");
        var bob = new User("Bob");

        var aliceRecipient = new UserRecipient(alice);
        var bobRecipient = new UserRecipient(bob);

        var marketingGroup = new GroupRecipient("Marketing");
        marketingGroup.AddRecipient(aliceRecipient);
        marketingGroup.AddRecipient(bobRecipient);

        var announcementsTopic = new Topic("Announcements", new[] { marketingGroup });

        var alertSystem = new TextNotificationSystem();
        var suspiciousWords = new List<string> { "urgent", "confidential", "password" };
        var alertRecipient = new NotificationSystemRecipient(alertSystem, suspiciousWords);

        var memoryArchiver = new InMemoryArchiver();
        var archiveRecipient = new ArchiveRecipient(memoryArchiver);

        var consoleFormatter = new ConsoleFormatter();
        var formattingArchiver = new FormattingArchiver(consoleFormatter);
        var formattingArchiveRecipient = new ArchiveRecipient(formattingArchiver);

        Console.WriteLine("All components initialized successfully!");
        Console.WriteLine("Run unit tests to verify functionality.");
    }
}