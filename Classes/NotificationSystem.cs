namespace HelloWorld.Classes
{
    // S: Interface for sending notifications (single responsibility)
    public interface INotificationSender
    {
        void Send(string message);
    }

    // I: Small, focused interface for logging (segregated from sending)
    public interface INotificationLogger
    {
        void Log(string message);
    }

    // S: Concrete class for email notifications (single responsibility)
    public class EmailNotificationSender : INotificationSender
    {
        public void Send(string message)
        {
            Console.WriteLine($"Sending email: {message}");
        }
    }

    // S: Concrete class for Slack notifications (single responsibility)
    public class SlackNotificationSender : INotificationSender
    {
        public void Send(string message)
        {
            Console.WriteLine($"Sending Slack message: {message}");
        }
    }

    // S: Concrete class for logging notifications (single responsibility)
    public class ConsoleNotificationLogger : INotificationLogger
    {
        public void Log(string message)
        {
            Console.WriteLine($"Logging notification: {message}");
        }
    }

    // S: Notification service handles orchestration (single responsibility)
    // O: Open for extension via INotificationSender (no modification needed for new senders)
    // D: Depends on abstractions (INotificationSender, INotificationLogger)
    public class NotificationService
    {
        private readonly INotificationSender _sender;
        private readonly INotificationLogger _logger;

        public NotificationService(INotificationSender sender, INotificationLogger logger)
        {
            _sender = sender ?? throw new ArgumentNullException(nameof(sender));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Notify(string message)
        {
            _logger.Log(message);
            _sender.Send(message);
        }
    }

    // L: Base class for notification processors (ensures substitutability)
    public abstract class NotificationProcessor
    {
        public virtual void ProcessNotifications(IEnumerable<string> messages)
        {
            foreach (var message in messages)
            {
                ProcessSingleNotification(message);
            }
        }

        protected abstract void ProcessSingleNotification(string message);
    }

    // L: Concrete processor using NotificationService (substitutable for base class)
    public class StandardNotificationProcessor : NotificationProcessor
    {
        private readonly NotificationService _service;

        public StandardNotificationProcessor(NotificationService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        protected override void ProcessSingleNotification(string message)
        {
            _service.Notify(message);
        }
    }

    // public class Program
    // {
    //     public static void Main()
    //     {
    //         // D: Dependency injection to provide abstractions
    //         INotificationSender emailSender = new EmailNotificationSender();
    //         INotificationLogger logger = new ConsoleNotificationLogger();
    //         NotificationService emailService = new NotificationService(emailSender, logger);

    //         // L: Polymorphic use of processor
    //         NotificationProcessor processor = new StandardNotificationProcessor(emailService);

    //         // Process notifications
    //         var messages = new[] { "Hello, user!", "Meeting at 2 PM" };
    //         processor.ProcessNotifications(messages);

    //         // O: Easily extend by switching to Slack without modifying NotificationService
    //         INotificationSender slackSender = new SlackNotificationSender();
    //         NotificationService slackService = new NotificationService(slackSender, logger);
    //         NotificationProcessor slackProcessor = new StandardNotificationProcessor(slackService);
    //         slackProcessor.ProcessNotifications(messages);
    //     }
    // }
}