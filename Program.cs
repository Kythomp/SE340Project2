using System;
using System.Collections.Generic;

// Event class representing a stock price change event
public class StockPriceChangeEvent
{
    public string StockSymbol { get; set; }
    public decimal CurrentPrice { get; set; }
}

// Subscriber interface
public interface ISubscriber
{
    void HandleEvent(StockPriceChangeEvent notification);
}

// Subscriber for the stock price change event
public class StockPriceChangeNotificationHandler : ISubscriber
{
    public void HandleEvent(StockPriceChangeEvent notification)
    {
        Console.WriteLine($"Stock {notification.StockSymbol} price has reached ${notification.CurrentPrice}. Sending notification.");
    }
}

// User class for subscribers with custom conditions and responses
public class User : ISubscriber
{
    public string Name { get; set; }
    public decimal PriceThreshold { get; set; }
    public string Response { get; set; }

    public void HandleEvent(StockPriceChangeEvent notification)
    {
        if (notification.CurrentPrice <= PriceThreshold)
        {
            Console.WriteLine($"User {Name} received notification: Stock {notification.StockSymbol} price has fallen below ${PriceThreshold}. {Response}");
        }
    }
}

// Event bus class
public class EventBus
{
    private List<ISubscriber> subscribers = new List<ISubscriber>();

    public void Subscribe(ISubscriber subscriber)
    {
        subscribers.Add(subscriber);
    }

    public void Unsubscribe(ISubscriber subscriber)
    {
        subscribers.Remove(subscriber);
    }

    public void Publish(StockPriceChangeEvent notification)
    {
        foreach (var subscriber in subscribers)
        {
            subscriber.HandleEvent(notification);
        }
    }
}


/// <summary>
/// For the purpose of this program the Program class is the publisher
/// </summary>
class Program
{
    static void Main()
    {
        // Create event bus
        var eventBus = new EventBus();

        // Create a handler and subscribe it to the event bus
        var handler = new StockPriceChangeNotificationHandler();
        eventBus.Subscribe(handler);

        // Create a user with a custom condition and response
        var user = new User
        {
            Name = "Wim Reuvekamp",
            PriceThreshold = 40.0m,
            Response = "Email has been sent!"
        };

        var user2 = new User
        {
            Name = "Kyle Thompson",
            PriceThreshold = 20.0m,
            Response = "Email has been sent!"
        };


        // Subscribe the user to the event bus
        eventBus.Subscribe(user);
        eventBus.Subscribe(user2);

        var stockSymbol = "XYZ";
        var currentPrice = 35.0m;

        // Simulate a stock price change event
        var notification = new StockPriceChangeEvent { StockSymbol = stockSymbol, CurrentPrice = currentPrice };

        // Publish the event to the event bus
        eventBus.Publish(notification);

        Console.ReadLine();
    }
}
